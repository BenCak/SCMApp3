using Microsoft.AspNetCore.Authentication.Negotiate;
using SCMApp3.Web.Infrastructure;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/scmapp-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.AddApplicationServices();
    builder.AddInfrastructureServices();
    builder.AddWebServices();

    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddAuthentication("DevAuth")
            .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, DevAuthSchemeHandler>("DevAuth", null);
    }
    else
    {
        builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();
    }

    builder.Services.AddAuthorizationBuilder()
        .SetDefaultPolicy(new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build());

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseMiddleware<DevAuthMiddleware>();
    }
    else
    {
        app.UseHsts();
    }

    app.UseMiddleware<UserActivityMiddleware>();

    app.UseHttpsRedirection();
    app.UseCors(static b => b.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseFileServer();

    app.MapOpenApi();
    app.MapScalarApiReference();

    app.UseExceptionHandler(options => { });

    app.MapEndpoints(typeof(Program).Assembly);

    app.MapFallbackToFile("index.html");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
