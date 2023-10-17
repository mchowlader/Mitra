using Microsoft.EntityFrameworkCore;
using Mitra.Api.Common;
using Mitra.Api.Models.DBModel;

namespace Mitra.Api.Services;

public class LoginHistoryServices : ILoginHistoryServices
{
    DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
    private readonly IConfiguration _configuration;

    public LoginHistoryServices(IConfiguration configuration)
    {

        _configuration = configuration;
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

    }

    public async Task<ServiceResponse<string>> SaveLoginHistory(int applicationUserId)
    {
        using(var context = new ApplicationDbContext(optionsBuilder.Options))
        {
            try
            {
                if(await context.LoginHistory.AnyAsync(x => x.LogOutTime == null && x.CreateBy == applicationUserId))
                {
                    var historyToUpdate = await context.LoginHistory.Where(x => x.LogOutTime == null && x.CreateBy == applicationUserId).ToListAsync();
                    historyToUpdate.ForEach(x => x.Status = "Logged out due to login from another instance");
                    historyToUpdate.ForEach(x => x.LogOutTime = Utilities.GetDateTime());
                    context.LoginHistory.UpdateRange(historyToUpdate);
                    await context.SaveChangesAsync();
                }

                LoginHistory loginHistory = new LoginHistory()
                {
                    CreateBy = applicationUserId,
                    CreateDate = Utilities.GetDateTime(),
                    IsRemoved = false,
                    LogInTime = Utilities.GetDateTime(),
                    LogOutTime = null,
                    Status = "Active"
                };

                await context.AddAsync(loginHistory);
                await context.SaveChangesAsync();
                return ServiceResponse<string>.Success("Login History Insert Successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public async Task<ServiceResponse<string>> UpdateLoginHistory(int applicationUserId)
    {
        try
        {
            using(var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                var historyToUpdate = await context
                    .LoginHistory
                    .Where(x => x.LogOutTime == null && x.CreateBy == applicationUserId)
                    .OrderByDescending(y => y.Id)
                    .FirstOrDefaultAsync();
                if (historyToUpdate is null) return ServiceResponse<string>.Success("Not SignIn");
                historyToUpdate.UpdateDate = Utilities.GetDateTime();
                historyToUpdate.UpdateBy = historyToUpdate.CreateBy;
                historyToUpdate.LogOutTime = Utilities.GetDateTime();
                context.LoginHistory.Update(historyToUpdate);
                await context.SaveChangesAsync();
                return ServiceResponse<string>.Success("Login History Update Successfully.");
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

public interface ILoginHistoryServices
{
    Task<ServiceResponse<string>> SaveLoginHistory(int applicationUserId);
    Task<ServiceResponse<string>>  UpdateLoginHistory(int applicationUserId);
}