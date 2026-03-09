using TemplateNormal;
using Serilog;
using System.Runtime.CompilerServices;


try
{
    // Initializes the logger
    Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

    // Initializes the container builder to run the API.
    var builder = WebApplication.CreateBuilder(args);

    // Injects the necessary services for the API.


    // Here is space to configure dependency injection for custom services.
    //
    //builder.Services.AddScoped<IMyService, MyService>();
    //

    builder.AddServices();

    var runtimeModeLogEnabled = ReadBooleanValue(
        builder.Configuration["Runtime:LogExecutionModeOnStartup"],
        true);

    if (runtimeModeLogEnabled)
    {
        var runtimeMode = RuntimeFeature.IsDynamicCodeSupported ? "JIT" : "AOT";
        Log.Information(
            "Runtime mode: {RuntimeMode} (DynamicCodeSupported: {DynamicCodeSupported})",
            runtimeMode,
            RuntimeFeature.IsDynamicCodeSupported);
    }

    // Starts the application with the builder's configuration.
    var app = builder.Build();
    app.Configure();


    // Use async, in case the server uses more than one service.
    Log.Information("Starting web application");
    await app.RunAsync();

} catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.Information("Application stopped");
    Log.CloseAndFlush();
}

static bool ReadBooleanValue(string? rawValue, bool defaultValue)
{
    if (string.IsNullOrWhiteSpace(rawValue) || rawValue.StartsWith("__", StringComparison.Ordinal))
    {
        return defaultValue;
    }

    return bool.TryParse(rawValue, out var parsed) ? parsed : defaultValue;
}

