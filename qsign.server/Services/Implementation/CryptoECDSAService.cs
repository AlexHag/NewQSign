using System.Security.Cryptography;
using System.Text;

namespace server.Services;

public class CryptoECDSAService : ICryptoECDSAService
{
    public ECDSAKeyPair GenerateKeyPair()
    {
        using(ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256))
        {
            var KeyParameters = ecdsa.ExportParameters(true);
            var PrivateKey = BitConverter.ToString(KeyParameters.D).Replace("-", "");

            byte[] xCoord = KeyParameters.Q.X;
            byte[] yCoord = KeyParameters.Q.Y;

            // Compress points in the future
            // byte prefix = (byte)((yCoord[yCoord.Length - 1] & 1) == 0 ? 0x02: 0x03);
            byte[] PublicPoint = new byte[xCoord.Length + yCoord.Length + 1];
            PublicPoint[0] = 0x04;
            Buffer.BlockCopy(xCoord, 0, PublicPoint, 1, xCoord.Length);
            Buffer.BlockCopy(yCoord, 0, PublicPoint, xCoord.Length + 1, yCoord.Length);
            var PublicKey = BitConverter.ToString(PublicPoint).Replace("-", "");

            return new ECDSAKeyPair
            {
                PrivateKey = PrivateKey,
                PublicKey = PublicKey
            };
        }
    }

    public string SignString(string DataString, string PrivateKey)
    {
        using(ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256))
        {
            ecdsa.ImportParameters(new ECParameters{Curve = ECCurve.NamedCurves.nistP256, D = StringToByteArray(PrivateKey)});
            var MessageBytes = Encoding.ASCII.GetBytes(DataString);
            
            var SignatureBytes = ecdsa.SignData(MessageBytes, HashAlgorithmName.SHA256);
            return BitConverter.ToString(SignatureBytes).Replace("-", string.Empty);
        }
    }

    public bool VerifySignature(string DataString, string Signature, string SubjectPublicKey)
    {
        using (ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256))
        {
            ecdsa.ImportParameters(new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                Q = new ECPoint
                {
                    X = StringToByteArray(SubjectPublicKey).Skip(1).Take(32).ToArray(),
                    Y = StringToByteArray(SubjectPublicKey).Skip(33).ToArray()
                }
            });
            return ecdsa.VerifyData(Encoding.ASCII.GetBytes(DataString), StringToByteArray(Signature), HashAlgorithmName.SHA256);
        }
    }

    private static byte[] StringToByteArray(string hex) {
    return Enumerable.Range(0, hex.Length)
                        .Where(x => x % 2 == 0)
                        .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                        .ToArray();
    }
}