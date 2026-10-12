using System.Diagnostics;

namespace APIDadosGeograficosORM.Tracing;

public static class OpenTelemetryExtensions
{
    public static string ServiceName { get; }
    public static string ServiceVersion { get; }
    public static ActivitySource ActivitySource { get; }

    static OpenTelemetryExtensions()
    {
        ServiceName = "APIDadosGeograficosORM";
        ServiceVersion = "1.0.0";
        ActivitySource = new ActivitySource(ServiceName, ServiceVersion);
    }
}