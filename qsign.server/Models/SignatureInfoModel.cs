namespace qsign.server.Models;

public class SignatureInfo
{
    public Guid Id { get; set; }
    public Guid SubjectId { get; set; }
    public string SubjectFirstName { get; set; }
    public string SubjectLastName { get; set; }
    public string SubjectEmail { get; set; }
    public Guid DocumentId { get; set; }
    public string DocumentHash { get; set; }
    public string Signature { get; set; }
    public string SubjectPublicKey { get; set; }
    public DateTime IssuedAt { get; set; }
}