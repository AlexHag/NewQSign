using System.Security.Cryptography;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using qsign.server.Services;
using qsign.server.Models;
using qsign.server.Context;

namespace qsign.server.Controllers;

[ApiController]
[Route("api")]
public class NewDocumentController : ControllerBase
{
    private readonly IDocumentService _service;
    
    public NewDocumentController(IDocumentService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize]
    [Route("UploadDocument")]
    public async Task<IActionResult> UploadDocument(IFormFile file)
    {
        return await _service.UploadDocument(HttpContext, file);
    }

    [HttpGet]
    [Authorize]
    [Route("GetUserDocuments")]
    public async Task<IActionResult> GetUsersDocuments()
    {
        return Ok();
    }

    [HttpGet]
    [Route("DownloadDocument/{DocumentId}")]
    // [Authorize]
    public async Task<IActionResult> DownloadDocument(Guid DocumentId)
    {
        return await _service.DownloadDocument(DocumentId);
    }

    [HttpGet]
    [Route("GetDocumentInfo/{DocumentId}")]
    // [Authorize]
    public async Task<IActionResult> GetDocumentInfo(Guid DocumentId)
    {
        return await _service.GetDocumentInfo(DocumentId);
    }
}