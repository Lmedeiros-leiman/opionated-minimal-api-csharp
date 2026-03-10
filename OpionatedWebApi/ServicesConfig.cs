using Serilog;
using Serilog.Events;
using OpionatedWebApi.Common.Serialization;
using Microsoft.EntityFrameworkCore;
using OpionatedWebApi.DatabaseContext;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpionatedWebApi;

public static partial class ServicesConfig
{
    //#if (HasAuthentication)
    // TODO:: Add auth service.
    //#endif
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        //#if (HasAuthentication)
        builder.AddAuthenticationServices();
        //#endif
        builder.AddDatabase();
        builder.AddJsonSerialization();

        builder.AddSerilog();
        if (builder.Environment.IsDevelopment())
        {
            builder.AddDevTools();
        }

        
        //builder.Services.AddAuditLog();
    }

    // Centralizes API JSON options and keeps AOT-generated context as the first resolver.
    private static void AddJsonSerialization(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

            //#if (GenerateAot)
            ConfigureJsonSerialization(options.SerializerOptions);
            //#endif
        });
    }

    // Adds SQL Server database context for runtime data access.
    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        var connectionString =
            builder.Configuration["Database:SqlServer:ConnectionString"]
            ?? builder.Configuration.GetConnectionString("Default");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("SQL connection string not configured. Set Database:SqlServer:ConnectionString or ConnectionStrings:Default.");
        }

        builder.Services.AddDbContext<SqlDbContext>(options => options.UseSqlServer(connectionString));
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
                //#if (HasAuthentication)
                ConfigureOpenApiSecurity(options);
                //#endif

                options.PostProcess = document =>
                {
                    document.Info.Title = apiInfo["Name"] ?? "Unnamed Api";
                    document.Info.Description = apiInfo["Description"] ?? "Nan";
                    document.Info.Version = apiInfo["Version"] ?? "Unspecified";
                };
            });
        }
    }

    //#if (HasAuthentication)
    static partial void AddAuthenticationServices(this WebApplicationBuilder builder);

    static partial void ConfigureOpenApiSecurity(NSwag.Generation.AspNetCore.AspNetCoreOpenApiDocumentGeneratorSettings options);
    //#endif

    //#if (GenerateAot)
    static partial void ConfigureJsonSerialization(JsonSerializerOptions options);
    //#endif



    // Overides Logger with an external configuration, found in appsettings.json.
    // This overrides the original configuration in Program.cs.
    private static void AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            // By default aways uses console as an opt-out, but keeps file sink a opt-in feature
            var loggerSection = context.Configuration.GetSection("Serilog");

            bool isConsoleEnabled = loggerSection.GetValue("enableConsoleSink", true);
            bool isFileEnabled = loggerSection.GetValue("enableFileSink", false);


            configuration
                .ReadFrom.Configuration(context.Configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning);

            if (isConsoleEnabled)
            {
                configuration.WriteTo.Console();
            }

            if (isFileEnabled)
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


}
