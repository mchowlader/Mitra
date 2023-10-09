namespace Mitra.Api.Common.Configuration;
public class JWTSettingConfig
{
    public string Secret { get; set; }
    public int ExpiresMinutes { get; set; }
}