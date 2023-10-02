using System.ComponentModel.DataAnnotations;

namespace Mitra.Api.Models;
public class SignInDTO
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}
public class SuccessfullSignInResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public SignInUserInfo User { get; set; }
}
public class FailedSignInResponse
{
    public int Error { get; set; }
    public string ErrorMessage { get; set; }
    public string Token { get; set; }
    public string DeviceId { get; set; }
}
public class SignInResponseDTO
{
    public SuccessfullSignInResponse successResponse { get; set; }
    public FailedSignInResponse failedResponse { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool TwoFactorCodeSent { get; set; }
    public bool TwoFactorCodeValidated { get; set; }
}