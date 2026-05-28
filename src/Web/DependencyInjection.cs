using SCMApp3.Application.Common.Interfaces;
using SCMApp3.Web.Infrastructure;
using SCMApp3.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddScoped<IUser, CurrentUser>();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddOpenApi(options =>
        {
            options.AddOperationTransformer<ApiExceptionOperationTransformer>();
        });

        builder.Services.AddCors();

        if (builder.Environment.IsDevelopment())
        {
            var devAuthOptions = builder.Configuration
                .GetSection("DevAuth")
                .Get<DevAuthOptions>() ?? new DevAuthOptions();
            builder.Services.AddSingleton(devAuthOptions);
        }
    }
}
