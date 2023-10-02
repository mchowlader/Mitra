using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Mitra.Api.Common;

public static class Utilities
{
    public static string GetRequestResponseTime()
    {
        return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
    }
    public static DateTime GetDateTime()
    {
        return DateTime.Now;
    }

    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using(var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            var s = Convert.ToBase64String(randomNumber);
            var randomNumberString = Regex.Replace(s, @"[$&+,:;=?@#|'<>/\\.^*()%!-]", "");
            return randomNumberString;
        }
    }
}