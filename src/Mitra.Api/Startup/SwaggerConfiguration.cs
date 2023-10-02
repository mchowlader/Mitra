using Microsoft.AspNetCore.Identity;
using Mitra.Api.DBModel;
using Mitra.Api.Seeding;

namespace Mitra.Api.Startup;

public static class SwaggerConfiguration
{
    public static WebApplication ConfigureSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
      
        return app;
    }
}
