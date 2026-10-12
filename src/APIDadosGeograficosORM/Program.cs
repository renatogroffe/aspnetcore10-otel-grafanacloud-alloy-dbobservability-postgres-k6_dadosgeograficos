using APIDadosGeograficosORM.Data;
using APIDadosGeograficosORM.Tracing;
using Grafana.OpenTelemetry;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

var alloyOtlpEndpoint = builder.Configuration["Alloy:OtlpEndpoint"];
var alloyProtocol = builder.Configuration["Alloy:Protocol"];

if (string.IsNullOrWhiteSpace(alloyOtlpEndpoint))
{
    throw new InvalidOperationException(
        "Configure Alloy:OtlpEndpoint (ex.: http://localhost:4318).");
}

builder.Services.AddSingleton<SqlCommenterInterceptor>();
builder.Services.AddDbContext<DadosGeograficosContext>((serviceProvider, options) =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("BaseDadosGeograficos"));

    options.AddInterceptors(
        serviceProvider.GetRequiredService<SqlCommenterInterceptor>());
});

builder.Services.AddScoped<DadosGeograficosRepository>();

var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService(serviceName: OpenTelemetryExtensions.ServiceName,
        serviceVersion: OpenTelemetryExtensions.ServiceVersion);
builder.Services.AddOpenTelemetry()
    .WithTracing((traceBuilder) =>
    {
        traceBuilder
            .AddSource(OpenTelemetryExtensions.ServiceName)
            .SetResourceBuilder(resourceBuilder)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddConsoleExporter()
            .UseGrafana(settings =>
            {
                settings.ExporterSettings = new OtlpExporter
                {
                    Endpoint = new Uri(alloyOtlpEndpoint),
                    Protocol = alloyProtocol == "Grpc" ? OtlpExportProtocol.Grpc : OtlpExportProtocol.HttpProtobuf
                };
            });
    });
builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(resourceBuilder);
    options.IncludeFormattedMessage = true;
    options.IncludeScopes = true;
    options.ParseStateValues = true;
    options.AttachLogsToActivityEvent();
    options.UseGrafana(settings =>
    {
        settings.ExporterSettings = new OtlpExporter
        {
            Endpoint = new Uri(alloyOtlpEndpoint),
            Protocol = alloyProtocol == "Grpc" ? OtlpExportProtocol.Grpc : OtlpExportProtocol.HttpProtobuf
        };
    });
});


builder.Services.AddOpenApi();



var app = builder.Build();

app.MapOpenApi();

app.UseHttpsRedirection();

app.MapGet("/estados", async (DadosGeograficosRepository repository) =>
{
    var estados = await repository.GetEstados();
    return Results.Ok(estados);
})
.WithName("GetEstados");

app.MapGet("/estados/{siglaEstado}", async (string siglaEstado, DadosGeograficosRepository repository) =>
{
    var estado = await repository.GetInfoEstado(siglaEstado);

    return estado is null
        ? Results.NotFound(new { mensagem = $"Estado '{siglaEstado}' não encontrado." })
        : Results.Ok(estado);
})
.WithName("GetInfoEstado");

app.MapGet("/regioes/{codRegiao}", async (string codRegiao, DadosGeograficosRepository repository) =>
{
    var regiao = await repository.GetDetalhesRegiao(codRegiao);

    return regiao is null
        ? Results.NotFound(new { mensagem = $"Região '{codRegiao}' não encontrada." })
        : Results.Ok(regiao);
})
.WithName("GetDetalhesRegiao");

app.Run();