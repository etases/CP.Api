using CP.Api.Context;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using CP.Api.Profiles;

var builder = WebApplication.CreateBuilder(args);
var buildEnv = builder.Environment;
var buildConf = builder.Configuration;

// Add services to the container.
builder.Services.AddAutoMapper(Profiles.AddProfile);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, // Validate the issuer
        ValidateAudience = true, // Validate the audience
        ValidAudience = builder.Configuration["Jwt:Audience"], // The valid audience
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // The valid issuer
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // The key to sign the token
    };
});

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UseNpgsql(buildConf.GetConnectionString("DefaultConnection"));
    else
    {
        var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL")!;
        var databaseUri = new Uri(connectionUrl!);
        var db = databaseUri.LocalPath.TrimStart('/');
        var userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);
        options.UseNpgsql($"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;");
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Auth middleware
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
