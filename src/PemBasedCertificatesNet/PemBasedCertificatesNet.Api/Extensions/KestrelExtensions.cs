using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using PemBasedCertificatesNet.Api.Options;

namespace PemBasedCertificatesNet.Api.Extensions;

public static class KestrelExtensions
{
    public static WebApplicationBuilder ConfigurePemCertificates(this WebApplicationBuilder builder)
    {
        builder.WebHost.UseKestrel(kestrel =>
        {
            var pemCertificates = builder.Configuration.GetPemCertificates();
            var aspNetUrls = builder.Configuration.GetAspNetUrls();

            foreach (var aspNetUrl in aspNetUrls)
            {
                if (aspNetUrl.IsSecure)
                {
                    kestrel.ListenHttpsWithPem(aspNetUrl.Host, aspNetUrl.Port, pemCertificates);
                }
                else
                {
                    kestrel.Listen(
                        IPAddress.TryParse(aspNetUrl.Host, out var addr)
                            ? addr
                            : IPAddress.Any,
                        aspNetUrl.Port
                    );
                }
            }
        });

        return builder;
    }

    private static void ListenHttpsWithPem(
        this KestrelServerOptions kestrel,
        string endPoint,
        int port,
        PemCertificates pemCertificates
    )
    {
        kestrel.Listen(
            IPAddress.TryParse(endPoint, out var addr) ? addr : IPAddress.Any,
            port,
            cfg =>
            {
                var certPem = File.ReadAllText(pemCertificates.CertPath);
                var keyPem = File.ReadAllText(pemCertificates.KeyPath);
                var x509Cert = X509Certificate2.CreateFromPem(certPem, keyPem);

                cfg.UseHttps(x509Cert);
            }
        );
    }
}