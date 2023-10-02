using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mitra.Api.Common;
using Mitra.Api.DBModel;
using Mitra.Api.Models.DBModel;
using custom = Mitra.Api.Models.DBModel;

namespace Mitra.Api.Seeding;

public class SeedDB
{
    public static readonly List<ApplicationUser> users = new List<ApplicationUser>()
    {
        new ApplicationUser{FirstName = "Mithun", LastName = "Howlader", UserName = "mithunh", PhoneNumber = "01710025023", Country = "Bangladesh"},
        new ApplicationUser{FirstName = "Mithun Chandra", LastName = "Howlader", UserName = "mithunch", PhoneNumber = "01710025023", Country = "Bangladesh"}
    };

    public static readonly string[] roles = new[] { "SuperAdmin", "Admin", "User", "Developer" };

    public static readonly List<custom.Action> Actions = new List<Models.DBModel.Action>()
    {
        new custom.Action{ Name = "Password Change", IsRemoved = false },
        new custom.Action{ Name = "Username change", IsRemoved = false },
        new custom.Action {  Name= "Add User", IsRemoved = false },
        new custom.Action {  Name= "Delete User", IsRemoved = false }
    };

    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager,
                                        RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
    {
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole()
                {
                    Name = role
                });
            }
        }

        foreach (var user in users)
        {
            if (await userManager.FindByNameAsync(user.UserName) == null)
            {
                if (user.UserName == "mithunch")
                {
                    user.EmailConfirmed = true;
                    user.TwoFactorEnabled = true;
                }

                var result = await userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, user.UserName == "mithunch" ? "MCH123456789" : user.UserName == "mithunh" ? "MH123456789" : "D123456789");
                    await userManager.AddToRoleAsync(user, user.UserName == "mithunh" ? "Admin" : user.UserName == "mithunch" ? "SuperAdmin" : "Developer");
                }
            }
        }

        foreach (var action in Actions)
        {
            if (!await context.Actions.AnyAsync(x => x.Name == action.Name))
            {
                await context.Actions.AddAsync(action);
            }
        }

        await context.SaveChangesAsync();
        var userList = await context.Users.Where(x => !x.IsRemoved).ToListAsync();

        foreach (var user in userList)
        {
            if (user.EmailOption.IsNotNullOrEmpty())
            {
                user.EmailOption = _EnumObjects.EmailOptions.NoEmail.ToString();
                await context.SaveChangesAsync();
            }
        }
    }
}