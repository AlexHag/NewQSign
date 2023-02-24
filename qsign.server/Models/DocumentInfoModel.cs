
namespace qsign.server.Models;

public class DocumentInfo
{
    public Guid Id { get; set; }
    public Guid SubjectUserId { get; set; }
    public string SubjectFirstName { get; set; }
    public string SubjectLastName { get; set; }
    public string SubjectEmail { get; set; }
    public string Filename {get; set; }
    public string Hash { get; set; }
}