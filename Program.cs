using CP.Api.Extensions;

#region builder

var builder = WebApplication.CreateBuilder(args);
var builderEnv = builder.Environment;
var builderConf = builder.Configuration;

// Add services to the container.
builder.Services.RegisterCommonServices();

builder.Services.RegisterAuthService(configurations: builderConf);

builder.Services.RegisterDbContextService(environments: builderEnv, configurations: builderConf);

#endregion

#region app
var app = builder.Build();

app.RegisterCommonServices();

app.RegisterAuthServices();

app.Run();

#endregion