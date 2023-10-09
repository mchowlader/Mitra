using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Mitra.Api.DBModel;
using Mitra.Api.Seeding;
using Mitra.Api.Services;
using System.Configuration;

namespace Mitra.Api.Startup;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<ApplicationDbContext>(option=> option
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<ITokenServices, TokenServices>();
        services.AddScoped<ILoginHistortServices, LoginHistortServices>();
        services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
        {
            option.Password.RequireDigit = false;
            option.Password.RequiredLength = 9;
            option.Password.RequireNonAlphanumeric = false;
            option.Password.RequireUppercase = false;
            option.Password.RequireLowercase = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();

        services.AddOptions<ApplicationOptions>()
                .BindConfiguration("JWTSettingConfig")
                .ValidateDataAnnotations()
                .ValidateOnStart();

        return services;
    }
}