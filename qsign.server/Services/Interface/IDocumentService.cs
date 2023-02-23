using Microsoft.AspNetCore.Mvc;

namespace qsign.server.Services;

public interface IDocumentService
{
    public Task<IActionResult> UploadDocument(HttpContext httpContext, IFormFile file);
    public Task<IActionResult> GetUsersDocuments(HttpContext httpContext);
    public Task<IActionResult> DownloadDocument(Guid DocumentId);
    public Task<IActionResult> GetDocumentInfo(Guid DocumentId);
}