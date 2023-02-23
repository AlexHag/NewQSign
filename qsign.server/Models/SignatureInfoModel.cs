namespace qsign.server.Models;

public class SignatureInfo
{
    public Guid Id { get; set; }
    public Guid SubjectPublicId { get; set; }
    public Guid DocumentId { get; set; }
    public string DocumentHash { get; set; }
    public string Signature { get; set; }
    public string SubjectPublicKey { get; set; }
    public DateTime IssuedAt { get; set; }
}