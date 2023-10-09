using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Mitra.Api.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

    #region Private Method
    private string GetTokenFor(string userId, string UserRole, int expiredInMinutes, byte[] key, string refreshToken)
    {
        var expires = DateTime.UtcNow.AddMinutes(expiredInMinutes);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userId),
                new Claim(ClaimTypes.Name, UserRole),
                new Claim("RefreshToken", refreshToken)
            }),
            Expires = expires,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var output = tokenHandler.WriteToken(token);
        return output;
    }
    #endregion
    public string GetToken(string userId, string userRole, string refreshToken)
    {
        return GetTokenFor(userId, userRole, ExpiresInMinutes, Key, refreshToken);
    }
}
