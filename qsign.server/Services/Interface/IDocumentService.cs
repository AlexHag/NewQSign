using Microsoft.AspNetCore.Mvc;

namespace qsign.server.Services;

public interface IDocumentService
{
    // TODO: Add status to check if they're signed
    public Task<IActionResult> GetUserDocuments(HttpContext httpContext);
    public Task<IActionResult> UploadDocument(HttpContext httpContext, IFormFile file);
    // TODO: Add signatures to response
    public Task<IActionResult> GetDocumentInfo(Guid DocumentId);
    public Task<IActionResult> DownloadDocument(Guid DocumentId);
    public Task<IActionResult> SignDocument(HttpContext httpContext, Guid DocumentId);
}