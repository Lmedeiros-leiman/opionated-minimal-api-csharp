using Serilog;
using Serilog.Events;
using OpionatedWebApi.Common.Serialization;

namespace OpionatedWebApi;

public static class ServicesConfig
{
    // TODO:: Add auth service.
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
        });

        builder.AddSerilog();
        if (builder.Environment.IsDevelopment())
        {
            builder.AddDevTools();
        }

        
        //builder.Services.AddAuditLog();
        //builder.AddDatabase();
        //builder.AddJsonSerialization();
        //builder.AddJwtAuthentication();
        //builder.Services.AddValidatorsFromAssembly(typeof(ConfigureServices).Assembly);
    }


    // Adds an UI for the OpenAPI docs under Debug.
    private static void AddDevTools(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            // Gets the config from AppSettings
            var apiInfo = builder.Configuration.GetSection("ApiInfo");

            // Add OpenAPI support for the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApiDocument(options =>
            {
                options.PostProcess = document =>
                {
                    document.Info.Title = apiInfo["Name"] ?? "Unnamed Api";
                    document.Info.Description = apiInfo["Description"] ?? "Nan";
                    document.Info.Version = apiInfo["Version"] ?? "Unspecified";
                };
            });
        }
    }


    // Overides Logger with an external configuration, found in appsettings.json.
    // This overrides the original configuration in Program.cs.
    private static void AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            var loggerSection = context.Configuration.GetSection("Serilog");
            var consoleEnabled = ReadBooleanValue(loggerSection, "EnableConsoleSink", true);
            var fileEnabled = ReadBooleanValue(loggerSection, "EnableFileSink", false);

            configuration
                .ReadFrom.Configuration(context.Configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning);

            if (!consoleEnabled && !fileEnabled)
            {
                configuration.WriteTo.Console();
                return;
            }

            if (consoleEnabled)
            {
                configuration.WriteTo.Console();
            }

            if (fileEnabled)
            {
                configuration.WriteTo.File(
                    path: "Logs/app-.log",
                    rollingInterval: RollingInterval.Year,
                    retainedFileCountLimit: 5,
                    fileSizeLimitBytes: 104857600,
                    rollOnFileSizeLimit: true,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}");
            }
        });
    }

    private static bool ReadBooleanValue(IConfigurationSection section, string key, bool defaultValue)
    {
        var rawValue = section[key];

        if (string.IsNullOrWhiteSpace(rawValue) || rawValue.StartsWith("__", StringComparison.Ordinal))
        {
            return defaultValue;
        }

        return bool.TryParse(rawValue, out var parsed) ? parsed : defaultValue;
    }





}
