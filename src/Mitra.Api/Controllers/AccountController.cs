using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mitra.Api.Common;
using Mitra.Api.DBModel;
using Mitra.Api.Models;
using Mitra.Api.Models.DBModel;
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
            var response = await _services.SignUp(model, User != null ? User.Identity.Name : "");

            return Ok(new PayloadResponse<ApplicationUser>
            {

                Success = response.success,
                Message = response.message,
                Payload = response.data,
                PayloadType = "User SignUp",
                RequestTime = requestTime,
                ResponseTime = Utilities.GetRequestResponseTime()

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
            var response = await _services.SignIn(model);
            return Ok(response);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost]
    [Route("SignOut")]
    public async Task<IActionResult> SignOut([FromBody] SignOutDTO model)
    {
        if (!ModelState.IsValid) return BadRequest();

        try
        {
            var response = await _services.SignOut(model);
            return Ok(new PayloadResponse<SignOutDTO>
            {
                Success = response.success,
                Message = response.message,
                Payload = null,
                PayloadType = "Sign Out",
                RequestTime = requestTime,
                ResponseTime = Utilities.GetRequestResponseTime()
            });
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /*[HttpPost]
    [Route("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshToken model)
    {
        if (!ModelState.IsValid) return BadRequest();
        try
        {
            _services.RefreshTokenValidate(model);
             return Ok();
        }
        catch (Exception ex)
        {       
            throw;
        }
    }*/
}