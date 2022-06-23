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
                defaultScheme: JwtBearerDefaults.AuthenticationScheme
            )
            .AddJwtBearer
            (
                configureOptions: options =>
                {
                    var jwtConf = configurations
                        .GetSection(key: "Jwt")
                        .Get<JWTModel>();

                    options.RequireHttpsMetadata = false;

                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true, // Validate the issuer
                        ValidateAudience = true, // Validate the audience
                        ValidAudience = jwtConf.Audience, // The valid audience
                        ValidIssuer = jwtConf.Issuer, // The valid issuer
                        IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(s: jwtConf.Key)) // The key to sign the token
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
            optionsAction: optionsBuilder =>
            {
                if (environments.IsDevelopment())
                {
                    optionsBuilder.UseNpgsql(connectionString: configurations.GetConnectionString(name: "DefaultConnection"));
                }
                if (!environments.IsDevelopment())
                {
                    var connectionUrl = Environment.GetEnvironmentVariable(variable: "DATABASE_URL")!;

                    var databaseUri = new Uri(uriString: connectionUrl!);

                    var databaseName = databaseUri
                        .LocalPath
                        .TrimStart(trimChar: '/');

                    var userInfo = databaseUri
                        .UserInfo
                        .Split
                        (
                            separator: ':',
                            options: StringSplitOptions.RemoveEmptyEntries
                        );

                    optionsBuilder.UseNpgsql
                    (
                        connectionString:
                            $"User ID={userInfo[0]};" +
                            $"Password={userInfo[1]};" +
                            $"Host={databaseUri.Host};" +
                            $"Port={databaseUri.Port};" +
                            $"Database={databaseName};" +
                            $"Pooling=true;" +
                            $"SSL Mode=Require;" +
                            $"Trust Server Certificate=True;"
                    );
                }
            }
        );
        return services;
    }

    static IServiceCollection AddBuiltInServices
    (
        this IServiceCollection services
    )
    {
        services
            .AddControllers()
            .AddJsonOptions
            (
                configure: options =>
                {
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.WriteIndented = true;
                }
            )
            .ConfigureApiBehaviorOptions
            (
                setupAction: options =>
                { }
            );

        services.AddEndpointsApiExplorer();

        services.AddCors
        (
            setupAction: options =>
            { }
        );
        return services;
    }

    static IServiceCollection AddProductServices
    (
        this IServiceCollection services
    )
    {
        services.TryAdd(descriptors: ProductServices.Services);
        return services;
    }

    static IServiceCollection AddThirdPartyServices
    (
        this IServiceCollection services
    )
    {
        services.AddAutoMapper(configAction: Profiles.Profiles.AddProfile);

        services.AddSwaggerGen
        (
            setupAction: options =>
            {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(filePath: Path.Combine(path1: AppContext.BaseDirectory, path2: xmlFilename));

                // Bearer token authentication
                options.AddSecurityDefinition
                (
                    name: "Bearer",
                    securityScheme: new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        BearerFormat = "JWT",
                        Scheme = "bearer",
                        Description = "Specify the authorization token.",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                    }
                );

                // Make sure swagger UI requires a Bearer token specified
                options.AddSecurityRequirement
                (
                    securityRequirement: new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
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