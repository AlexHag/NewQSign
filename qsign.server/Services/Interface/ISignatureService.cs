using qsign.server.Models;
using Microsoft.AspNetCore.Mvc;

namespace qsign.server.Services;

public interface ISignatureService
{
    // Create
    public Task<IActionResult> SignDocument();
    

    // Read
    public Task<IActionResult> GetSignature();
}