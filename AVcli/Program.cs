using AVcli.Graphics.UserPanel;
using AvCore.Application.Interfaces;
using AvCore.Application.Services;
using AvCore.Domain.Entities.policies;
using AvCore.Infrastructure.Security;
using AvCore.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
// Time wasted : 18hrs
class Program
{
    public static IServiceProvider ServiceProvider { get; private set; }
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        var app = ServiceProvider.GetRequiredService<UserPanel>();

        app.Welcome();

    }
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IFileScanner, FileScanner>();
        services.AddTransient<UserPanel>();
        services.AddTransient<IHasher, Hasher>();
        services.AddTransient<ZipPolicy>();
        services.AddTransient<IZipArcvhiveService, ZipArchiveService>();
        services.AddTransient<IOpenRead, OpenRead>();



    }
}
