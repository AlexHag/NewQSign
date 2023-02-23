using qsign.server.Models;
using Microsoft.AspNetCore.Mvc;

namespace qsign.server.Services;

public interface IAuthenticationService
{
    public Task<IActionResult> RegisterUser(UserRegisterDTO UserRegisterRequest);
    public Task<IActionResult> LoginUser(UserLoginDTO UserLoginRequest);
    // public Task<IActionResult> GetUserInfo(HttpContext httpContext);
}