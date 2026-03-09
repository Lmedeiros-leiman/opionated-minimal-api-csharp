using Serilog;

namespace TemplateNormal;

public static class ApplicationConfig
{
    public static void Configure(this WebApplication app)
    {

        if (app.Environment.IsDevelopment())
        {
            // puts a development UI for the API.
            app.UseOpenApi();
            app.UseSwaggerUi();
            app.UseDeveloperExceptionPage();
        }

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();
        
        // This works when you configure an authentication service
        // Either pure JWT or ASP.NET Identity framework
        // But by default, does nothing.
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapEndpoints();
    }
}