using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using server.Models;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("api")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _service;

    public AuthenticationController(IAuthenticationService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO UserRegisterRequest)
    {
        return await _service.RegisterUser(UserRegisterRequest);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO UserLoginRequest)
    {
        return await _service.LoginUser(UserLoginRequest);
    }

    // [Authorize]
    // [HttpGet]
    // [Route("userinfo")]
    // public async Task<IActionResult> GetUserInfo()
    // {
    //     return await _service.GetUserInfo(HttpContext);
    // }
}