using CP.Api.Extensions;

#region builder

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment builderEnv = builder.Environment;
ConfigurationManager builderConf = builder.Configuration;

// Add services to the container.
builder.Services.RegisterCommonServices();

builder.Services.RegisterAuthService(builderConf);

builder.Services.RegisterDbContextService(builderEnv, builderConf);

#endregion

#region app

WebApplication app = builder.Build();

app.RegisterCommonServices();

app.RegisterAuthServices();

app.Run();

#endregion