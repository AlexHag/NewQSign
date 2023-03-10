using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Net.Http.Headers;
using System.Security.Cryptography;

using qsign.server.Services;
using qsign.server.Context;
using qsign.server.Models;
using qsign.server.Controllers;

namespace qsign.server.Services;

public class DocumentService : IDocumentService
{
    private DataContext _context;
    private readonly IHelperFunctions _helper;
    private readonly ICryptoECDSAService _crypto;
    public DocumentService(IHelperFunctions helper, DataContext context, ICryptoECDSAService crypto)
    {
        _helper = helper;
        _context = context;
        _crypto = crypto;
    }

    public async Task<IActionResult> GetUserDocuments(HttpContext httpContext)
    {
        Guid userId = _helper.GetRequestUserId(httpContext);
        var UsersDocuments = await _context.DocumentInfos.Where(p => p.SubjectUserId == userId).ToListAsync();
        return new OkObjectResult(UsersDocuments);
    }

    // TODO: Handle error if document already exists. Sign document self now.
    public async Task<IActionResult> UploadDocument(HttpContext httpContext, IFormFile file)
    {
        Guid userId = _helper.GetRequestUserId(httpContext);
        var TheUser = await _context.UserAccounts.FindAsync(userId);
        if(TheUser == null) throw new Exception("User not found should not happen since JWT is valid.");
        
        // Handle blob upload error
        string fileId = await BlobUpload(file);
        
        var stream = file.OpenReadStream();
        var Hash = BitConverter.ToString(SHA256.Create().ComputeHash(stream)).Replace("-", "").ToLowerInvariant();

        var newDocument = new DocumentInfo
        {
            Id = Guid.Parse(fileId),
            SubjectUserId = userId,
            SubjectFirstName = TheUser.FirstName,
            SubjectLastName = TheUser.LastName,
            SubjectEmail = TheUser.Email,
            Filename = file.FileName,
            Hash = Hash
        };

        await _context.DocumentInfos.AddAsync(newDocument);
        await SignDocument(httpContext, newDocument.Id);
        await _context.SaveChangesAsync();

        return new CreatedAtActionResult("GetDocumentInfoAction", "Document", new { DocumentId = newDocument.Id }, newDocument);
    }

    public async Task<IActionResult> GetDocumentInfo(Guid DocumentId)
    {
        var DocumentInfo = _context.DocumentInfos.Find(DocumentId);
        var DocumentSignatures = await _context.SignatureInfos
            .Where(p => p.DocumentId == DocumentId)
            .ToListAsync();
        if(DocumentInfo == null) return new NotFoundObjectResult(null);
        
        return new OkObjectResult(new
        {
            DocumentInfo = DocumentInfo,
            DocumentSignatures = DocumentSignatures
        });
    }

    public async Task<IActionResult> DownloadDocument(Guid DocumentId)
    {
        var DocumentBlob = await BlobDownload(DocumentId);
        if(DocumentBlob == null) return new NotFoundObjectResult(null);
        
        var FileReturn = new FileContentResult(DocumentBlob.FileBytes, DocumentBlob.ContentType);
        FileReturn.FileDownloadName = DocumentBlob.FileName;
        
        return FileReturn;
    }

    public async Task<IActionResult> SignDocument(HttpContext httpContext, Guid DocumentId)
    {
        Guid userId = _helper.GetRequestUserId(httpContext);
        var TheUser = await _context.UserAccounts.FindAsync(userId);
        if(TheUser == null) throw new Exception("User not found should not happen since JWT is valid.");     

        var TheDocumentInfo = await _context.DocumentInfos.FindAsync(DocumentId);
        if(TheDocumentInfo == null) return new BadRequestObjectResult("Document not found"); // 404 Not found instead?
        
        var SignatureAlreadyExists = _context.SignatureInfos
            .Where(p => p.DocumentId == DocumentId && p.SubjectId == userId).FirstOrDefault();
        if(SignatureAlreadyExists != null) return new BadRequestObjectResult("Document already signed");

        var TheSignatureString = _crypto.SignHash(TheDocumentInfo.Hash, TheUser.PrivateKey);

        var TheNewSignatureObject = new SignatureInfo
        {
            Id = Guid.NewGuid(),
            SubjectId = TheUser.Id,
            SubjectFirstName = TheUser.FirstName,
            SubjectLastName = TheUser.LastName,
            SubjectEmail = TheUser.Email,
            DocumentId = DocumentId,
            DocumentHash = TheDocumentInfo.Hash,
            Signature = TheSignatureString,
            SubjectPublicKey = TheUser.PublicKey,
            IssuedAt = DateTime.UtcNow
        };

        await _context.SignatureInfos.AddAsync(TheNewSignatureObject);
        await _context.SaveChangesAsync();
        
        return new CreatedAtActionResult("GetDocumentInfoAction", "Document", new { DocumentId = DocumentId }, TheNewSignatureObject);
    }
    


    // This is a mock of the Azure Blob Storage and should be changed in the future.
    // Returns ID of the uploaded document. TODO: Handle exception
    private async Task<string> BlobUpload(IFormFile file)
    {
        var client = new HttpClient();

        var formData = new MultipartFormDataContent();
        var streamContent = new StreamContent(file.OpenReadStream());
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType); // add content type header
        formData.Add(streamContent, "file", file.FileName);

        var response = await client.PostAsync("http://localhost:5163/Document", formData);
        return await response.Content.ReadAsStringAsync();
    }

    private async Task<BlobDownloadObject> BlobDownload(Guid DocumentId)
    {
        var client = new HttpClient();
        var response = await client.GetAsync("http://localhost:5163/Document/" + DocumentId);

        if (response.IsSuccessStatusCode)
        {
            if(response.Content.Headers.TryGetValues("Content-Disposition", out var CD))
            {
                if(response.Content.Headers.TryGetValues("Content-Type", out var CT))
                {
                    var contentDisposition = new System.Net.Mime.ContentDisposition(CD.First());
                    var FileName = contentDisposition.FileName;
                    string ContentType = CT.FirstOrDefault();
                    var FileBytes = await response.Content.ReadAsByteArrayAsync();
                    
                    return new BlobDownloadObject
                    {
                        FileName = FileName,
                        ContentType = ContentType,
                        FileBytes = FileBytes
                    };
                }
                throw new Exception("No Content-Type header found");
            }
            throw new Exception("No Content-Disposition header found");
        }
        else
        {
            return null;            
        }
    }
}

public class BlobDownloadObject
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public byte[] FileBytes { get; set; }
}