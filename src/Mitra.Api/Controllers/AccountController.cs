using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitra.Api.Common;
using Mitra.Api.DBModel;
using Mitra.Api.Models;
using Mitra.Api.Services;

namespace Mitra.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IUserServices _services;
    private readonly string requestTime = Utilities.GetRequestResponseTime();
    public AccountController(IUserServices services)
    {
        _services = services;
    }

    [HttpPost]
    [Route("SignUp")]
    public async Task<IActionResult> SignUp(SignUpDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _services.SignUp(model, User != null ? User.Identity.Name : "");

            return Ok(new PayloadResponse<ApplicationUser>
            {
                Message = result.message,
                Payload = result.data,
                PayloadType = "User SignUp",
                RequestTime = requestTime,
                Success = result.success
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("SignIn")]
    public async Task<IActionResult> SignIn([FromBody]SignInDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _services.SignIn(model);
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}