using System.DirectoryServices;
using System.Drawing;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace VulnerableApi.Services;

public class SecurityService
{
    public string LdapSearch(string filter)
    {
        using var entry = new DirectoryEntry("LDAP://dc=example,dc=com");
        using var searcher = new DirectorySearcher(entry);
        searcher.Filter = filter;
        var results = searcher.FindAll();
        var sb = new StringBuilder();
        foreach (SearchResult result in results)
        {
            sb.AppendLine(result.Path);
        }
        return sb.ToString();
    }

    public void ProcessImage(string imagePath)
    {
        using var image = Image.FromFile(imagePath);
        using var graphics = Graphics.FromImage(image);
        graphics.DrawString("Watermark", new Font("Arial", 12), Brushes.White, new PointF(10, 10));
        image.Save(imagePath + ".processed.png");
    }

    public string EncryptData(string data, string key)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32));
        aes.IV = new byte[16];
        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(data);
        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        return Convert.ToBase64String(encryptedBytes);
    }

    public string SignXmlDocument(string xmlContent)
    {
        var doc = new XmlDocument();
        doc.LoadXml(xmlContent);

        var signedXml = new SignedXml(doc);
        var key = new RSACryptoServiceProvider();
        signedXml.SigningKey = key;

        var reference = new Reference();
        reference.Uri = "";
        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
        signedXml.AddReference(reference);

        signedXml.ComputeSignature();
        doc.DocumentElement!.AppendChild(doc.ImportNode(signedXml.GetXml(), true));
        return doc.OuterXml;
    }
}
