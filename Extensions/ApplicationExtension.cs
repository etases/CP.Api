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

    private static WebApplication AddBuiltInService
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
            policyBuilder =>
            {
                policyBuilder
                    .WithOrigins
                    (
                        new[] {"http://localhost:*", "https://*.herokuapp.com"}
                    )
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .WithMethods
                    (
                        new[] {"GET", "POST", "PUT", "PATCH", "DELETE"}
                    );
            }
        );
        return app;
    }

    private static WebApplication AddThirPartyService
    (
        this WebApplication app
    )
    {
        app.UseSwagger();
        app.UseSwaggerUI
        (
            options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "PRN231 API");
                options.RoutePrefix = string.Empty;
            }
        );
        return app;
    }
}
#pragma warning restore CS1591