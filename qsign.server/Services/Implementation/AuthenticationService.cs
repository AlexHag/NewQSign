

using qsign.server.Models;
using qsign.server.Context;
using System.Text;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace qsign.server.Services;

public class AuthenticationService : IAuthenticationService
{
    private DataContext _context;
    private readonly IConfiguration _config;
    private readonly ICryptoECDSAService _crypto;

    public AuthenticationService(DataContext context, IConfiguration config, ICryptoECDSAService crypto) 
    {
        _config = config;
        _context = context;
        _crypto = crypto;
    }

    public async Task<IActionResult> RegisterUser(UserRegisterDTO UserRegisterRequest)
    {
        var existingUser = _context.UsersAccounts
            .FirstOrDefault(u => u.Email == UserRegisterRequest.Email);
        if (existingUser != null) 
        {
            return new BadRequestObjectResult("Email already exists");
        }

        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var userSalt = new string(Enumerable.Repeat(chars, 16)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        
        var passwordHash = HashString(UserRegisterRequest.Password + userSalt);
        var KeyPair = _crypto.GenerateKeyPair();
        var newUser =  new UserAccount 
        {
            Id = Guid.NewGuid(),
            Email = UserRegisterRequest.Email,
            FirstName = UserRegisterRequest.FirstName,
            LastName = UserRegisterRequest.LastName,
            PasswordHash = passwordHash,
            Salt = userSalt,
            PrivateKey = KeyPair.PrivateKey,
            PublicKey = KeyPair.PublicKey,
        };

        await _context.UsersAccounts.AddAsync(newUser);
        await _context.SaveChangesAsync();
        var token = CreateJWT(newUser.Id);
        return new OkObjectResult(new 
        { 
            Authorization = token,
            userInfo = new 
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                PublicKeyPem = newUser.PublicKey,
            } 
        });
    }
    public async Task<IActionResult> LoginUser(UserLoginDTO UserLoginRequest)
    {
        var userSalt = _context.UsersAccounts
            .Where(u => u.Email == UserLoginRequest.Email)
            .Select(u => u.Salt)
            .FirstOrDefault();
        if (userSalt == null) 
        {
            return new BadRequestObjectResult("Wrong username or password");
        }
        var passwordHash = HashString(UserLoginRequest.Password + userSalt);
        
        var existingUser = _context.UsersAccounts
            .Where(u => u.Email == UserLoginRequest.Email && u.PasswordHash == passwordHash)
            .FirstOrDefault();
        
        if (existingUser == null) 
        {
            return new BadRequestObjectResult("Wrong username or password");
        }

        var token = CreateJWT(existingUser.Id);
        
        return new OkObjectResult(new 
        { 
            Authorization = token,
            userInfo = new 
            {
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                Email = existingUser.Email,
                PublicKeyPem = existingUser.PublicKey,
            } 
        });
    }

    // public async Task<IActionResult> GetUserInfo(HttpContext httpContext) 
    // {
    //     var userId = _helper.GetRequestUserId(httpContext);
    //     var user = await _context.Users.FindAsync(userId);
    //     // Throw or log exception error
    //     if (user == null) return new BadRequestObjectResult("User is null eventhough jwt token is valid. This should not happen.");

    //     var userStore = _context.Stores.Where(p => p.StoreOwnerId == user.Id).FirstOrDefault();
    //     return new OkObjectResult(new UserInfoDTO { 
    //         Email = user.Email,
    //         Role = user.Role,
    //         StoreName = userStore == null ? "" : userStore.Name
    //     });
    // }
    
    private string CreateJWT(Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("Id", userId.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string HashString(string input)
    {
        StringBuilder Sb = new StringBuilder();

        using (var hash = SHA256.Create())
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(input));

            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }
}