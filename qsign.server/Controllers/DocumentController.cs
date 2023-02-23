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
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _service;
    
    public DocumentController(IDocumentService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUsersDocumentsAction()
    {
        return await _service.GetUserDocuments(HttpContext);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadDocumentAction(IFormFile file)
    {
        return await _service.UploadDocument(HttpContext, file);
    }

    [HttpGet("{DocumentId}")]
    public async Task<IActionResult> GetDocumentInfoAction(Guid DocumentId)
    {
        return await _service.GetDocumentInfo(DocumentId);
    }

    [HttpGet("{DocumentId}/download")]
    public async Task<IActionResult> DownloadDocumentAction(Guid DocumentId)
    {
        return await _service.DownloadDocument(DocumentId);
    }
}