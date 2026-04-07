using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection SetupSwagger(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            const string API_DOCUMENTATION_NAME = "Persona Backend API";

            options.EnableAnnotations();

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "Version 1",
                Title = API_DOCUMENTATION_NAME,
            });
        });
        return services;
    }

    public static WebApplication UseCustomSwagger(this WebApplication app, string title)
    {
        app.UseSwagger();

        if (app.Environment.IsDevelopment())
            app.UseSwaggerUI(configuration =>
            {
                configuration.DocumentTitle = $"{title} API Docs";
                configuration.SwaggerEndpoint("/swagger/v1/swagger.json", $"{title} API V1");
                configuration.RoutePrefix = "swagger";
                configuration.DisplayRequestDuration();
                configuration.EnableTryItOutByDefault();
                configuration.DocExpansion(DocExpansion.None);
                configuration.DefaultModelsExpandDepth(-1);
                configuration.EnablePersistAuthorization();
                configuration.EnableFilter();
                configuration.EnableDeepLinking();
                configuration.ShowExtensions();
                configuration.EnableSwaggerDocumentUrlsEndpoint();
            });

        return app;
    }

}
