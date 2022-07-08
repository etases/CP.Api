using System.Reflection;
using System.Text;
using System.Text.Json;

using CP.Api.Context;
using CP.Api.Core.Models;
using CP.Api.Core.Resources.Static;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CP.Api.Extensions;

#pragma warning disable CS1591
public static class BuilderExtension
{
    public static IServiceCollection RegisterCommonServices
    (
        this IServiceCollection services
    )
    {
        services.AddBuiltInServices();

        services.AddProductServices();

        services.AddThirdPartyServices();

        return services;
    }

    public static IServiceCollection RegisterAuthService
    (
        this IServiceCollection services,
        ConfigurationManager configurations
    )
    {
        services
            .AddAuthentication
            (
                JwtBearerDefaults.AuthenticationScheme
            )
            .AddJwtBearer
            (
                options =>
                {
                    JWTModel? jwtConf = configurations
                        .GetSection("Jwt")
                        .Get<JWTModel>();

                    options.RequireHttpsMetadata = false;

                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // Validate the issuer
                        ValidateAudience = true, // Validate the audience
                        ValidAudience = jwtConf.Audience, // The valid audience
                        ValidIssuer = jwtConf.Issuer, // The valid issuer
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConf.Key)) // The key to sign the token
                    };
                }
            );
        return services;
    }

    public static IServiceCollection RegisterDbContextService
    (
        this IServiceCollection services,
        IWebHostEnvironment environments,
        ConfigurationManager configurations
    )
    {
        services.AddDbContextPool<ApplicationDbContext>
        (
            optionsBuilder =>
            {
                if (environments.IsDevelopment())
                {
                    optionsBuilder.UseNpgsql(configurations.GetConnectionString("DefaultConnection"));
                }

                if (!environments.IsDevelopment())
                {
                    string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL")!;

                    Uri databaseUri = new Uri(connectionUrl!);

                    string databaseName = databaseUri
                        .LocalPath
                        .TrimStart('/');

                    string[] userInfo = databaseUri
                        .UserInfo
                        .Split
                        (
                            ':',
                            StringSplitOptions.RemoveEmptyEntries
                        );

                    optionsBuilder.UseNpgsql
                    (
                        $"User ID={userInfo[0]};" +
                        $"Password={userInfo[1]};" +
                        $"Host={databaseUri.Host};" +
                        $"Port={databaseUri.Port};" +
                        $"Database={databaseName};" +
                        "Pooling=true;" +
                        "SSL Mode=Require;" +
                        "Trust Server Certificate=True;"
                    );
                }
            }
        );
        return services;
    }

    private static IServiceCollection AddBuiltInServices
    (
        this IServiceCollection services
    )
    {
        services
            .AddControllers()
            .AddJsonOptions
            (
                options =>
                {
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.WriteIndented = true;
                }
            )
            .ConfigureApiBehaviorOptions
            (
                options =>
                {
                }
            );

        services.AddEndpointsApiExplorer();

        services.AddCors
        (
            options =>
            {
            }
        );
        return services;
    }

    private static IServiceCollection AddProductServices
    (
        this IServiceCollection services
    )
    {
        services.TryAdd(ProductServices.Services);
        return services;
    }

    private static IServiceCollection AddThirdPartyServices
    (
        this IServiceCollection services
    )
    {
        services.AddAutoMapper(Profiles.Profiles.AddProfile);

        services.AddSwaggerGen
        (
            options =>
            {
                // Set the comments path for the Swagger JSON and UI.
                string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                // Bearer token authentication
                options.AddSecurityDefinition
                (
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        BearerFormat = "JWT",
                        Scheme = "bearer",
                        Description = "Specify the authorization token.",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http
                    }
                );

                // Make sure swagger UI requires a Bearer token specified
                options.AddSecurityRequirement
                (
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme, Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
                );
            }
        );
        return services;
    }
}
#pragma warning restore CS1591