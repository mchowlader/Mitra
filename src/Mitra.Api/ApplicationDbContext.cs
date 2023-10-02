using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mitra.Api.DBModel;
using Mitra.Api.Models.DBModel;

namespace Mitra.Api
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
   
        public DbSet<Models.DBModel.Action> Actions { get; set; }
        public DbSet<UserAuditLog> UserAuditLog { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
    }
}