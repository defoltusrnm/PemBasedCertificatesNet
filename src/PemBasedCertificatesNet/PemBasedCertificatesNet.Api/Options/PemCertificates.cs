namespace PemBasedCertificatesNet.Api.Options;

public class PemCertificates
{
    public const string SectionKey = "PemCertificates";
    
    public required string CertPath { get; init; }
    public required string KeyPath { get; init; }
}