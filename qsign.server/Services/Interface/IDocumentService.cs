using Microsoft.AspNetCore.Mvc;

namespace qsign.server.Services;

public interface IDocumentService
{
    public Task<IActionResult> GetUserDocuments(HttpContext httpContext);
    public Task<IActionResult> UploadDocument(HttpContext httpContext, IFormFile file);
    public Task<IActionResult> GetDocumentInfo(Guid DocumentId);
    public Task<IActionResult> DownloadDocument(Guid DocumentId);
}