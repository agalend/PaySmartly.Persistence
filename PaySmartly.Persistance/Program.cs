using PaySmartly.Persistance.Mongo;
using PaySmartly.Persistance.Repository;
using PaySmartly.Persistance.Services;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using PaySmartly.Persistance.Env;

string ServiceName = "Persistance Service";

var builder = WebApplication.CreateSlimBuilder(args);
AddOpenTelemetryLogging(builder);

IConfigurationSection dbSettings = builder.Configuration.GetSection("PaySlipsDatabase");
builder.Services.Configure<PaySlipsDatabaseSettings>(dbSettings);
builder.Services.AddSingleton<IEnvProvider, EnvProvider>();
builder.Services.AddSingleton<IRepository<MongoRecord>, MongoRepository>();
builder.Services.AddGrpc();

AddOpenTelemetryService(builder);

var app = builder.Build();
app.MapGrpcService<PersistanceService>();
app.MapGet("/health", () => Results.Ok());

app.Run();

void AddOpenTelemetryLogging(WebApplicationBuilder builder)
{
    builder.Logging.AddOpenTelemetry(options =>
    {
        ResourceBuilder resourceBuilder = ResourceBuilder.CreateDefault().AddService(ServiceName);

        options.SetResourceBuilder(resourceBuilder).AddConsoleExporter();
    });
}

void AddOpenTelemetryService(WebApplicationBuilder builder)
{
    OpenTelemetryBuilder openTelemetryBuilder = builder.Services.AddOpenTelemetry();

    openTelemetryBuilder = openTelemetryBuilder.ConfigureResource(resource => resource.AddService(ServiceName));

    openTelemetryBuilder = openTelemetryBuilder.WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation().AddConsoleExporter();
    });
    openTelemetryBuilder = openTelemetryBuilder.WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation().AddConsoleExporter();
    });
}
