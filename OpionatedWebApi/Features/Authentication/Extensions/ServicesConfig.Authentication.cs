using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace OpionatedWebApi;

public static partial class ServicesConfig
{
    // Auth-only service wiring kept in a separate partial file for template readability.
    static partial void AddAuthenticationServices(this WebApplicationBuilder builder)
    {
        var jwtSection = builder.Configuration.GetSection("Authentication:Jwt");
        var enforceSection = jwtSection.GetSection("Enforce");

        var mode = builder.Configuration["Authentication:Mode"];
        var issuer = jwtSection["Issuer"];
        var audience = jwtSection["Audience"];
        var hs256Secret = jwtSection.GetSection("Hs256")["Secret"]
            ?? builder.Configuration["Jwt:Key"];

        var requireIssuer = enforceSection.GetValue("RequireIssuer", true);
        var requireAudience = enforceSection.GetValue("RequireAudience", true);
        var requireExpiration = enforceSection.GetValue("RequireExpirationTime", true);
        var requireSignedTokens = enforceSection.GetValue("RequireSignedTokens", true);
        var validateIssuerSigningKey = enforceSection.GetValue("ValidateIssuerSigningKey", true);

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = requireIssuer,
                    ValidIssuer = requireIssuer ? issuer : null,
                    ValidateAudience = requireAudience,
                    ValidAudience = requireAudience ? audience : null,
                    ValidateLifetime = requireExpiration,
                    RequireExpirationTime = requireExpiration,
                    RequireSignedTokens = requireSignedTokens,
                    ValidateIssuerSigningKey = validateIssuerSigningKey,
                    ClockSkew = TimeSpan.Zero
                };

                if (string.Equals(mode, "HS256", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(hs256Secret))
                {
                    tokenValidationParameters.IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(hs256Secret));
                }

                if (string.Equals(mode, "HS256", StringComparison.OrdinalIgnoreCase)
                    && validateIssuerSigningKey
                    && string.IsNullOrWhiteSpace(hs256Secret))
                {
                    throw new InvalidOperationException("HS256 mode requires Authentication:Jwt:Hs256:Secret when signing key validation is enabled.");
                }

                options.TokenValidationParameters = tokenValidationParameters;
            });
    }

    static partial void ConfigureOpenApiSecurity(AspNetCoreOpenApiDocumentGeneratorSettings options)
    {
        options.AddSecurity("Bearer", [], new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'"
        });

        options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
    }
}
