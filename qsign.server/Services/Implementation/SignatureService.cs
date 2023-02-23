using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qsign.server.Context;
using qsign.server.Models;

namespace qsign.server.Services;

public class SignatureService : ISignatureService
{
    private DataContext _context;
    private readonly IHelperFunctions _helper;
    private readonly ICryptoECDSAService _crypto;

    public SignatureService(IHelperFunctions helper, DataContext context, ICryptoECDSAService crypto)
    {
        _helper = helper;
        _crypto = crypto;
        _context = context;
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
            DocumentId = TheDocumentInfo.Id,
            DocumentHash = TheDocumentInfo.Hash,
            Signature = TheSignatureString,
            SubjectPublicKey = TheUser.PublicKey,
            IssuedAt = DateTime.UtcNow
        };

        await _context.SignatureInfos.AddAsync(TheNewSignatureObject);
        await _context.SaveChangesAsync();
        
        return new CreatedAtActionResult("GetSignature", "Signature", new { SignatureId = TheNewSignatureObject.Id }, TheNewSignatureObject);
    }
    
    public async Task<IActionResult> GetSignature(Guid SignatureId)
    {
        var TheSignature = await _context.SignatureInfos.FindAsync(SignatureId);
        if (TheSignature == null) return new NotFoundObjectResult(null);
        return new OkObjectResult(TheSignature);
    }

    public async Task<IActionResult> GetDocumentSignatures(Guid DocumentId)
    {
        var TheDocument = await _context.DocumentInfos.FindAsync(DocumentId);
        if(TheDocument == null) return new NotFoundObjectResult(null);

        var TheSignatures = await _context.SignatureInfos.Where(p => p.DocumentId == TheDocument.Id).ToListAsync();

        return new OkObjectResult(TheSignatures);
    }
}