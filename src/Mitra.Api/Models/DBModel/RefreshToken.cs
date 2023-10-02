using System.ComponentModel.DataAnnotations;

namespace Mitra.Api.Models.DBModel
{
    public class RefreshToken : BaseModel
    {
        [Required(ErrorMessage = "This field ir required")]
        public string IdentityrUserId { get; set; }

        [Required(ErrorMessage = "This field ir required")]
        public string Token { get; set; }
        public DateTime CurrencyTime { get; set; }
        public string DeviceId { get; set; }
    }
}
