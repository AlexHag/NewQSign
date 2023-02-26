
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

public class CommunicationInfo
{
    public Guid Id { get; set; }
    public Guid RecipientId { get; set; }
    public Guid DocumentId { get; set; }
    public DateTime TimeSend { get; set; }
    public bool IsSigned { get; set; }
    public DateTime TimeSigned { get; set; }
}