namespace Mitra.Api.Common
{
    public class JWTSettingConfig
    {
        public string Secret { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}
