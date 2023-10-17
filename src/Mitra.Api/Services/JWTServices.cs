using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Mitra.Api.Common;
using Mitra.Api.Common.Configuration;
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
    public string GetToken(string userId, string userRole, string refreshToken)
    {
        return GetTokenFor(userId, userRole, ExpiresInMinutes, Key, refreshToken);
    }
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        return GetPrincipalFromToken(token, Key);
    }

    private IConfiguration Configuration { get; }
    private readonly IOptionsSnapshot<JWTSettingConfig> _jwtSettingConfig; 
    private byte[] Key;
    private DateTime Expires { get; }
    private int ExpiresInMinutes {  get; }

    #region Private Method
    private string GetTokenFor(string userId, string UserRole, int expiredInMinutes, byte[] key, string refreshToken)
    {
        try
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
        catch (Exception)
        {
            throw;
        } 
    }
    private ClaimsPrincipal GetPrincipalFromToken(string token, byte[] key)
    {
        try
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid Token");
            return principal;

        }
        catch (SecurityTokenException ex)
        {
            throw;
        }
    }
    #endregion
}
