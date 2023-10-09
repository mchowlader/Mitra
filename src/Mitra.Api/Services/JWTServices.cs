using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Mitra.Api.Common;
using System.Text;

namespace Mitra.Api.Services;
public class JWTServices
{
    public JWTServices(IConfiguration configuration, IOptionsSnapshot<JWTSettingConfig> jwtSettingConfig) 
    {
        Configuration = configuration;
        _jwtSettingConfig = jwtSettingConfig;
        Key = Encoding.ASCII.GetBytes(_jwtSettingConfig.Value.Secret);
        ExpiresInMinutes = _jwtSettingConfig.Value.ExpiresInMinutes;
    }
    private IConfiguration Configuration { get; }
    private readonly IOptionsSnapshot<JWTSettingConfig> _jwtSettingConfig; 
    private byte[] Key;
    private DateTime Expires { get; }
    private int ExpiresInMinutes {  get; }
}
