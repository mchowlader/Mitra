namespace Mitra.Api.Models.DBModel
{
    public class LoginHistory : BaseModel
    {
        public DateTime? LoginTime { get; set; }
        public DateTime? LoginOut { get; set; }
        public string Status { get; set; }
    }
}
