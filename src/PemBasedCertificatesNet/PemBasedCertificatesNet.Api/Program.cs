using PemBasedCertificatesNet.Api.Extensions;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigurePemCertificates();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/dummy", () => Results.Ok(new { Message = "Hello from PEM based API" }))
    .WithName("Dummy endpoint")
    .WithOpenApi();

app.Run();