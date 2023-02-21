using server.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.Services;

public interface IAuthenticationService
{
    public Task<IActionResult> RegisterUser(UserRegisterDTO UserRegisterRequest);
    public Task<IActionResult> LoginUser(UserLoginDTO UserLoginRequest);
    // public Task<IActionResult> GetUserInfo(HttpContext httpContext);
}