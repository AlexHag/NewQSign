using Microsoft.AspNetCore.Mvc;

using qsign.blob.Context;
using qsign.blob.Models;

namespace qsign.blob.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{
    private readonly ILogger<DocumentController> _logger;
    private readonly DataContext _context;

    public DocumentController(ILogger<DocumentController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("{FileId}")]
    public async Task<IActionResult> Get(string FileId)
    {
        var documentObject = await _context.DocumentObjects.FindAsync(FileId);
        if(documentObject == null) return NotFound();

        var FileData = await System.IO.File.ReadAllBytesAsync("./DocumentFiles/" + FileId);
        return File(FileData, documentObject.ContentType, documentObject.FileName);
    }

    [HttpPost()]
    public async Task<IActionResult> UploadDocument(IFormFile file)
    {
        var FileId = Guid.NewGuid().ToString();
        var stream = file.OpenReadStream();
        
        stream.Position = 0;
        byte[] buffer = new byte[stream.Length];
        for (int totalBytesCopied = 0; totalBytesCopied < stream.Length; )
            totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
        
        await System.IO.File.WriteAllBytesAsync("./DocumentFiles/" + FileId, buffer);
        
        await _context.DocumentObjects.AddAsync(new DocumentObject 
        { 
            Id = FileId,
            FileName = file.FileName,
            ContentType = file.ContentType
        });

        await _context.SaveChangesAsync();
    
        return Ok(FileId);
    }

    // private byte[] ToByteArray(this Stream stream)
    // {
    //     stream.Position = 0;
    //     byte[] buffer = new byte[stream.Length];
    //     for (int totalBytesCopied = 0; totalBytesCopied < stream.Length; )
    //         totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
    //     return buffer;
    // }
}
