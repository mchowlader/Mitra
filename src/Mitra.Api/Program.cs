using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mitra.Api.DBModel;
using Mitra.Api.Seeding;
using Mitra.Api.Services;
using Mitra.Api.Startup;

namespace Mitra.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            builder.Services.RegisterServices(configuration);
            var app = builder.Build();
            app.ConfigureSwagger();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            Configure(app);
            app.Run();
        }
        public static void Configure(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            SeedDB.Initialize(serviceProvider, userManager, roleManager, context)
                  .Wait();
        }
    }
}