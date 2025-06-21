using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace VMTS.API.Helpers;

internal sealed class BearerSecuritySchemeTransformer(
    IAuthenticationSchemeProvider authenticationSchemeProvider
) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        var authSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

        if (authSchemes.Any(auth => auth.Name == JwtBearerDefaults.AuthenticationScheme))
        {
            // Define multiple security schemes
            var securitySchemes = new Dictionary<string, OpenApiSecurityScheme>
            {
                ["Admin"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Description = "JWT for regular users",
                },
                ["Manager"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Description = "JWT for Manager users",
                },
                ["Driver"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Description = "JWT for Driver users",
                },
            };

            // Ensure components are initialized
            document.Components ??= new OpenApiComponents();

            foreach (var kvp in securitySchemes)
            {
                document.Components.SecuritySchemes[kvp.Key] = kvp.Value;
            }

            // Add security requirements (both will show in Scalar)
            document.SecurityRequirements ??= new List<OpenApiSecurityRequirement>();

            foreach (var schemeKey in securitySchemes.Keys)
            {
                document.SecurityRequirements.Add(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = schemeKey,
                                },
                            },
                            Array.Empty<string>()
                        },
                    }
                );
            }
        }
    }
}
