using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Validations.Rules;
using Mitra.Api.Common;
using Mitra.Api.Common.Configuration;
using Mitra.Api.DBModel;
using Mitra.Api.Seeding;
using Mitra.Api.Services;
using System.Configuration;
using System.Text;

namespace Mitra.Api.Startup;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        IdentityModelEventSource.ShowPII = true;
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<ApplicationDbContext>(option => option
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<ITokenServices, TokenServices>();
        services.AddScoped<ILoginHistoryServices, LoginHistoryServices>();
        services.AddScoped<JWTServices>();

        services.AddOptions<JWTSettingConfig>()
                .BindConfiguration("JWTSettingConfig")
                .ValidateDataAnnotations()
                .ValidateOnStart();
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<JWTSettingConfig>>().Value);

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

        #region Autheticaion and Authorization
        var key = Encoding.ASCII.GetBytes(configuration.GetSection("JWTSettingConfig")["Secret"]);

        #region Authetication Module
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("X-Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                }
            };
        });

        #endregion

        services.AddAuthorization(options =>
        {
            options.AddPolicy("User", policy => policy.RequireRole("User"));
            options.AddPolicy("SuperAdmin", policy => policy.RequireRole("SuperAdmin"));
        });

        #endregion

        #region Swagger Region
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Rest API",
                Version = "v1",
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = "Enter the request header in the following box to add Jwt to grant authorization Token: Bearer Token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{}

                }
            });
        });
        #endregion

        return services;
    }
}