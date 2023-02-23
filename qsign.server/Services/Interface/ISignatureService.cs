using qsign.server.Models;
using Microsoft.AspNetCore.Mvc;

namespace qsign.server.Services;

public interface ISignatureService
{
    // Create
    public Task<IActionResult> SignDocument(HttpContext httpContext, Guid DocumentId);
    
    // Read
    public Task<IActionResult> GetSignature(Guid SignatureId);
    public Task<IActionResult> GetDocumentSignatures(Guid DocumentId);
}