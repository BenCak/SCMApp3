using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SCMApp3.Application.Common.Interfaces;

namespace SCMApp3.Application.FunctionalTests.Infrastructure;

public class WebApiFactory(string connectionString) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:SCMApp3Db", connectionString);

        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<IUser>()
                .AddTransient(_ =>
                {
                    var mock = new Mock<IUser>();
                    mock.SetupGet(x => x.UserName).Returns(TestApp.GetCurrentUser());
                    mock.SetupGet(x => x.Roles).Returns(Array.Empty<string>());
                    mock.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(false);
                    return mock.Object;
                });
        });
    }
}
