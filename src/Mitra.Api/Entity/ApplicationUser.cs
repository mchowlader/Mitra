using Microsoft.AspNetCore.Identity;

namespace Mitra.Api.DBModel;

public class ApplicationUser : IdentityUser<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmailOption { get; set; }
    public string? Country { get; set; }
    public bool IsRemoved { get; set; }
}

public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole() : base()
    {
    }
}