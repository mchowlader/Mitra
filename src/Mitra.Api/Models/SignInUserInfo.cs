namespace Mitra.Api.Models
{
    public class SignInUserInfo
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
