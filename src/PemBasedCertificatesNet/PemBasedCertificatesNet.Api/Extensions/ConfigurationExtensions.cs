using System.Collections.Immutable;
using System.Net;
using PemBasedCertificatesNet.Api.Options;

namespace PemBasedCertificatesNet.Api.Extensions;

public static class ConfigurationExtensions
{
    public static PemCertificates GetPemCertificates(this IConfiguration configuration)
    {
        var pemCertificates = configuration
            .GetSection(PemCertificates.SectionKey)
            .Get<PemCertificates>();

        return pemCertificates switch
        {
            null => throw new InvalidOperationException($"No Section {PemCertificates.SectionKey} is defined"),
            not null when string.IsNullOrWhiteSpace(pemCertificates.CertPath)
                => throw new InvalidOperationException("No certificate path is defined"),
            not null when string.IsNullOrWhiteSpace(pemCertificates.KeyPath)
                => throw new InvalidOperationException("No key path is defined"),
            not null => pemCertificates,
        };
    }

    public static ImmutableArray<AspNetUrl> GetAspNetUrls(this IConfiguration configuration)
    {
        var urls = configuration["ASPNETCORE_URLS"];
        if (string.IsNullOrWhiteSpace(urls))
        {
            throw new InvalidOperationException("Urls for app is not defined. Please define ASPNETCORE_URLS somewhere");
        }

        var aspNetUrls = urls
            .Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(
                url => url
                    .Split([":", "//"], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Where(part => !part.Equals("//", StringComparison.InvariantCulture))
                    .Select(part => part switch
                    {
                        "localhost" => "127.0.0.1",
                        _ => part
                    })
                    .ToArray()
            )
            .Select(url => new AspNetUrl
            {
                IsSecure = url[0].Equals("https", StringComparison.InvariantCultureIgnoreCase),
                Host = url[1],
                Port = int.Parse(url[2])
            })
            .ToImmutableArray();

        return aspNetUrls;
    }
}