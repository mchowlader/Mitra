using Microsoft.EntityFrameworkCore;
using Mitra.Api.Common;
using Mitra.Api.Models.DBModel;

namespace Mitra.Api.Services
{
    public class LoginHistortServices : ILoginHistortServices
    {
        DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        private readonly IConfiguration _configuration;

        public LoginHistortServices(IConfiguration configuration)
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
                    if(await context.LoginHistory.AnyAsync(x => x.LoginOut == null && x.CreteBy == applicationUserId))
                    {
                        var historyToUpdate = await context.LoginHistory.Where(x => x.LoginOut == null && x.CreteBy == applicationUserId).ToListAsync();
                        historyToUpdate.ForEach(x => x.Status = "Logged out due to login from another instance");
                        historyToUpdate.ForEach(x => x.LoginOut = Utilities.GetDateTime());
                        context.LoginHistory.UpdateRange(historyToUpdate);
                        await context.SaveChangesAsync();
                    }

                    LoginHistory loginHistory = new LoginHistory()
                    {
                        CreteBy = applicationUserId,
                        CreateDate = Utilities.GetDateTime(),
                        IsRemoved = false,
                        LoginTime = Utilities.GetDateTime(),
                        LoginOut = null,
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
    }

    public interface ILoginHistortServices
    {
        Task<ServiceResponse<string>> SaveLoginHistory(int applicationUserId);
    }
}