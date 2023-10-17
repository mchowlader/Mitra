namespace Mitra.Api.Models.DBModel
{
    public class LoginHistory : BaseModel
    {
        public DateTime? LogInTime { get; set; }
        public DateTime? LogOutTime { get; set; }
        public string Status { get; set; }
    }
}
