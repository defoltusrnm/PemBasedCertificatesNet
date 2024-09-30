using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using PemBasedCertificatesNet.Api.Options;

namespace PemBasedCertificatesNet.Api.Extensions;

public static class KestrelExtensions
{
    // переебашиваем хосты кестрела
    public static WebApplicationBuilder ConfigurePemCertificates(this WebApplicationBuilder builder)
    {
        builder.WebHost.UseKestrel(kestrel =>
        {
            // забираем пути сертов
            var pemCertificates = builder.Configuration.GetPemCertificates();
            // перепаршиваем ASPNETCORE_URLS для чтобы было пездато
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
                // читаем синхронно серты, иначе кал (нет асинк методов)
                var certPem = File.ReadAllText(pemCertificates.CertPath);
                var keyPem = File.ReadAllText(pemCertificates.KeyPath);
                
                // сдк делает нам сертификат
                var x509Cert = X509Certificate2.CreateFromPem(certPem, keyPem);

                // говорим кестрелу его узать
                cfg.UseHttps(x509Cert);
            }
        );
    }
}