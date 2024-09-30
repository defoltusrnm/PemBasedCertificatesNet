namespace PemBasedCertificatesNet.Api.Options;

public class AspNetUrl
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required bool IsSecure { get; init; }
}