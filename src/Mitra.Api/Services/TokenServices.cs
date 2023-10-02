using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mitra.Api.Common;
using Mitra.Api.DBModel;
using Mitra.Api.Models.DBModel;

namespace Mitra.Api.Services
{
    public class TokenServices : ITokenServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly string requestTime = Utilities.GetRequestResponseTime();
        private readonly IConfiguration _configuration;
        DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        public TokenServices(UserManager<ApplicationUser> userManager , RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

            optionsBuilder = optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<ServiceResponse<object>> DeleteRefreshTokenByUserId(string userName)
        {
            try
            {
                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var tokenDelete = await context.RefreshTokens.Where(x => x.IdentityrUserId == userName).Select(x => x).ToListAsync();
                    if (tokenDelete.Count > 0) await context.RefreshTokens.ExecuteDeleteAsync();
                    return ServiceResponse<object>.Success("Token Delete Successfully");
                    //return new PayloadResponse<object>
                    //{
                    //    Message = new List<string> { "Token Delete Successfully" },
                    //    Payload = null,
                    //    PayloadType = "Delete Refresh Token By User",
                    //    RequestTime = requestTime,
                    //    ResponseTime = Utilities.GenerateRefreshToken(),
                    //    Success = true
                    //};
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<ServiceResponse<object>> SaveRefreshToken(RefreshToken refreshToken)
        {
            try
            {
                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    #region Delete Exiting Refresh Token
                    var OlderRefreshTokenList = context.RefreshTokens
                        .Where(x => x.IdentityrUserId == refreshToken.IdentityrUserId)
                        .AsNoTracking()
                        .ToList();

                    if (OlderRefreshTokenList.Count > 0) context.RefreshTokens.RemoveRange(OlderRefreshTokenList);
                    #endregion

                    #region Save New Refresh Token
                    refreshToken.CreteBy = Convert.ToInt32(refreshToken.IdentityrUserId);
                    refreshToken.CreateDate = Utilities.GetDateTime();
                    await context.AddAsync(refreshToken);
                    var data = await context.SaveChangesAsync();
                    #endregion

                    return ServiceResponse<object>.Success("Refresh Token Save Successfully", data);
                    //return new PayloadResponse<object>()
                    //{
                    //    Message= new List<string> { "Refresh Token Save Successfully" },
                    //    Payload = data,
                    //    PayloadType = "Save Refresh Token",
                    //    RequestTime = requestTime,
                    //    ResponseTime = Utilities.GenerateRefreshToken(),
                    //    Success = true
                    //};
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public interface ITokenServices
    {
        Task<ServiceResponse<object>> DeleteRefreshTokenByUserId(string userName);
        Task<ServiceResponse<object>> SaveRefreshToken(RefreshToken refreshToken);
    }
}
