using qsign.server.Models;

namespace qsign.server.Services;

public interface IHelperFunctions
{
    public Guid GetRequestUserId(HttpContext context);
}