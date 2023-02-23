using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using qsign.server.Models;

namespace qsign.server.Services;

public class HelperFunctions : IHelperFunctions
{
    private readonly IConfiguration _config;

    public HelperFunctions(IConfiguration config) 
    {
         _config = config;
    }

    public Guid GetRequestUserId(HttpContext context)
    {
        var identity = context.User.Identity as ClaimsIdentity;
        if(identity == null)
        {
            throw new Exception("Identity is null. This should not happen and be handeled by the [Authorize] attribute.");
        }

        IEnumerable<Claim> claims = identity.Claims; 
        var claimId = identity.FindFirst("Id")?.Value;
        
        if(claimId == null)
        {
            throw new Exception("Identity is null. This should not happen and be handeled by the [Authorize] attribute.");
        }

        return Guid.Parse(claimId);
    }
}