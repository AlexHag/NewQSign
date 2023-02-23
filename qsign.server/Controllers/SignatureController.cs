using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using qsign.server.Services;

[ApiController]
[Route("api/[controller]")]
public class SignatureController : ControllerBase
{
    private readonly ISignatureService _service;
    public SignatureController(ISignatureService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SignDocument([FromBody] Guid DocumentId)
    {
        return await _service.SignDocument(HttpContext, DocumentId);
    }

    [HttpGet("{SignatureId}")]
    public async Task<IActionResult> GetSignature(Guid SignatureId)
    {
        return await _service.GetSignature(SignatureId);
    }

    [HttpGet("document/{documentId}")]
    public async Task<IActionResult> GetDocumentSignatures(Guid documentId)
    {
        return await _service.GetDocumentSignatures(documentId);
    }
}