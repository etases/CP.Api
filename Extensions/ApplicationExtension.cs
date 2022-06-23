namespace CP.Api.Extensions;

#pragma warning disable CS1591
public static class ApplicationExtension
{
    public static WebApplication RegisterCommonServices
    (
        this WebApplication app
    )
    {
        app.AddBuiltInService();

        app.AddThirPartyService();

        return app;
    }

    public static WebApplication RegisterAuthServices
    (
        this WebApplication app
    )
    {
        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }

    static WebApplication AddBuiltInService
    (
        this WebApplication app
    )
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.MapControllers();

        app.UseCors
        (
            configurePolicy: policyBuilder =>
            {
                policyBuilder
                    .WithOrigins
                    (
                        origins: new[]
                        {
                            "http://localhost:*",
                            "https://*.herokuapp.com"
                        }
                    )
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .WithMethods
                    (
                        methods: new[]
                        { "GET", "POST", "PUT", "PATCH", "DELETE" }
                    );
            }
        );
        return app;
    }

    static WebApplication AddThirPartyService
    (
        this WebApplication app
    )
    {
        app.UseSwagger();
        app.UseSwaggerUI
        (
            setupAction: options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "PRN231 API");
                options.RoutePrefix = string.Empty;
            }
        );
        return app;
    }
}
#pragma warning restore CS1591