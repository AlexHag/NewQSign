
namespace qsign.server.Services;

public interface ICryptoECDSAService
{
    // Have this take a entropy random client input
    // Tuple<PrivateKey, PublicKey>
    public ECDSAKeyPair GenerateKeyPair();

    // The imput for this function will be the hash of the document and it's being hashed twice now.
    public string SignString(string DataString, string PrivateKey);
    public bool VerifySignature(string DataString, string Signature, string SubjectPublicKey);
}

public class ECDSAKeyPair
{
    public string PrivateKey { get; set; }
    public string PublicKey { get; set; }
}