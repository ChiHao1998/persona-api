using Api.Context.PersonaBackend;
using Api.Interface.Broker;
using Api.Model.Common;
using Api.Service.Broker.VaultHttpClientBrokerService;
using Api.Service.Startup;
using Serilog.Middleware;
using Configuration;
using Cors.Model;
using Postgres.Model;
using Serilog;

const string APPLICATION_NAME = "persona-backend";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.SetHostCurrentDirectory();
builder.Host.SetupSerilog(APPLICATION_NAME);

Log.Information($"Host directory: {Directory.GetCurrentDirectory()}");


builder.InjectAppConfiguration<AppSettings>();
builder.InjectAppConfiguration<PostgresSettings>();


builder.Services.SetupResponseCompression();

builder.Services.AddHttpClientWithPolicies<IVaultHttpClientBrokerService, VaultHttpClientBrokerService>(
    new HttpClientOption()
    {
        BaseUrl = builder.RetrieveAppSetting<AppSettings>().VaultHttpClient.BaseUrl
    }
);

builder.Services.AutoAddScopedServiceByIScopedService();

AppSettings appsettings = await VaultStartupService.RetrieveVaultAppSettingsAsync(builder.Services);

builder.Services.SetupCors(appsettings.CorsPolicyList);
if (appsettings.CorsPolicyList.Length == 0)
    Log.Warning("CORS setup completed with no available policies");
else
    Log.Information($"CORS setup completed with available policies: {string.Join(", ", appsettings.CorsPolicyList.Select(x => x.Policy))}");


builder.Services.SetupApiVersioning();

builder.Services.SetupRequestTimeout(TimeSpan.FromSeconds(30));

builder.Services.SetupRateLimiter([new() { Name = APPLICATION_NAME, RequestLimit = 100, Window = TimeSpan.FromSeconds(10) }]);

builder.Services.SetupController(applicationName: APPLICATION_NAME);

builder.Services.SetupSwagger();

builder.Services.SetupPostgresDatabaseContext<PersonaContext>(appsettings.PostgresSettings);

builder.Services.AddResponseCaching();

builder.Services.SetMaxRequestBodySize(maxRequestBodySizeInMb: 5);

WebApplication app = builder.Build();

await app.Services.CheckAndApplyMigrationsAsync<PersonaContext>();

app.UseHttpsRedirection();

foreach (CorsPolicy corsPolicy in appsettings.CorsPolicyList)
    app.UseCors(corsPolicy.Policy);

app.UseCustomSwagger("Persona");

app.UseRateLimiter();

app.UseResponseCompression();

app.UseResponseCaching();

app.UseMiddleware<HttpInboundLoggingMiddleware>();

app.MapControllers();

app.Run();