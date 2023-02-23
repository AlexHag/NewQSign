
namespace qsign.server.Services;

public interface ICryptoECDSAService
{
    // Have this take a entropy random client input
    public ECDSAKeyPair GenerateKeyPair();

    public string SignHash(string DataHash, string PrivateKey);
    public bool VerifySignatureHash(string DataHash, string Signature, string SubjectPublicKey);
}

public class ECDSAKeyPair
{
    public string PrivateKey { get; set; }
    public string PublicKey { get; set; }
}