using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mitra.Api.Common;
using Mitra.Api.DBModel;
using Mitra.Api.Models;
using Mitra.Api.Models.DBModel;
using System.Linq;
namespace Mitra.Api.Services;

public class UserServices : IUserServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly string requestTime = Utilities.GetRequestResponseTime();
    private readonly IConfiguration _configuration;
    private readonly ITokenServices _tokenServices;
    private readonly ILoginHistortServices _loginHistortServices;
    private JWTServices JWTServices { get; }
    DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

    public UserServices(UserManager<ApplicationUser> userManager
        , RoleManager<ApplicationRole> roleManager
        , IConfiguration configuration
        , ITokenServices tokenServices
        , ILoginHistortServices loginHistortServices
        , JWTServices jwtServices)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenServices = tokenServices;
        _configuration = configuration;
        _loginHistortServices = loginHistortServices;
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        JWTServices = jwtServices;
    }
    public async Task<ServiceResponse<SignInResponseDTO>> SignIn(SignInDTO model)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            #region Bad Login Attemots
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (user == null || !isPasswordValid) return ServiceResponse<SignInResponseDTO>.Unauthorizes(401, "Invalid login credentials");

            var role = _userManager.GetRolesAsync(user);
            //if ((role.Result[0] == "Developer" || role.Result[0] == "User" || role.Result[0] == "Admin") && !user.EmailConfirmed)
            //{
            //    return ServiceResponse<SignInDTO>.Unauthorizes(501, "Please confirm your email before logging in.");
            //}
            #endregion

            if (user.Email.IsNotNullOrEmpty())
            { 
                return await ValidResponse(user); 
            };
            return ServiceResponse<SignInResponseDTO>.Success("SignIn Successfully.");

        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public async Task<ServiceResponse<ApplicationUser>> SignUp(SignUpDTO model, string identityUserId)
    {
        try
        {
            var user = new ApplicationUser();
            var isNoExiting = await _userManager.FindByEmailAsync(model.Email) == null &&
                                await _userManager.FindByNameAsync(model.UserName) == null;
            if (isNoExiting)
            {
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;
                user.PhoneNumber = model.PhoneNumber;
                user.Country = model.Country;
                user.IsRemoved = false;
                user.EmailOption = _EnumObjects.EmailOptions.NoEmail.ToString();

                var createUser = await _userManager.CreateAsync(user);
                if (!createUser.Succeeded)
                    return ServiceResponse<ApplicationUser>
                           .Error("User Create Failed: " + createUser.Errors
                           .Select(x => x.Description
                           .Replace("User name", "UserName")).ToList()
                           .ToCommaSeparatedString());

                var createPaasword = await _userManager.AddPasswordAsync(user, model.Password);
                if (!createPaasword.Succeeded)
                    return ServiceResponse<ApplicationUser>
                           .Error("Password Create Failed: " + createPaasword.Errors
                           .Select(x => x.Description.Replace("User name", "Username")).ToList()
                           .ToCommaSeparatedString());

                var addRole = await _userManager.AddToRoleAsync(user, "User");
                if (!addRole.Succeeded)
                    return ServiceResponse<ApplicationUser>
                           .Error("Role Create Failed: " + addRole.Errors
                           .Select(x => x.Description.Replace("User name", "Usernaem")).ToList()
                           .ToCommaSeparatedString());

                user.PasswordHash = null;
                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var action = await context
                                       .Actions
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.Name == "Add User");

                    UserAuditLog userAuditLog = new UserAuditLog()
                    {
                        ActionMoment = Utilities.GetDateTime(),
                        IpAddress = model.IpAddress,
                        ByUserId = identityUserId.ToInt32(),
                        AffectedUserId = user.Id,
                        ActionId = action.Id
                    };

                    await context.AddAsync(userAuditLog);
                    await context.SaveChangesAsync();

                    return ServiceResponse<ApplicationUser>.Success("User has been create successfully.", user);
                }
            }
            else
            {
                return ServiceResponse<ApplicationUser>
                       .Error("User creation failed as username or email has already been taken");
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<ServiceResponse<SignInResponseDTO>> ValidResponse(ApplicationUser user)
    {
        var roles = _userManager.GetRolesAsync(user);
        SignInUserInfo userInfo = new SignInUserInfo();
        userInfo.UserId = user.Id;
        userInfo.FirstName = user.FirstName;
        userInfo.LastName = user.LastName;
        userInfo.UserName = user.UserName;
        userInfo.EmailConfirmed = user.EmailConfirmed;
        userInfo.Role = roles.Result.FirstOrDefault();
        await _tokenServices.DeleteRefreshTokenByUserId(user.Id.ToString());
        await _loginHistortServices.SaveLoginHistory(user.Id);

        var refreshToken = new RefreshToken()
        {
            IdentityrUserId = user.Id.ToString(),
            Token = Utilities.GenerateRefreshToken(),
            DeviceId = user.Id.ToString(),
        };

        var data = await _tokenServices.SaveRefreshToken(refreshToken);
        var response = new SignInResponseDTO()
        {
            successResponse = new SuccessfullSignInResponse
            {
                Token = JWTServices.GetToken(user.Id.ToString(), roles.Result.FirstOrDefault(), refreshToken.Token),
                RefreshToken = refreshToken.Token,
                User = userInfo
            }
        };
        return ServiceResponse<SignInResponseDTO>.Success("" ,response);
        //return ServiceResponse<ApplicationUser>.Success("", null);
    }
}

public interface IUserServices
{
    Task<ServiceResponse<SignInResponseDTO>> SignIn(SignInDTO model);
    Task<ServiceResponse<ApplicationUser>> SignUp(SignUpDTO model, string identityUserId);
}