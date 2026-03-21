using AVcli.Graphics.UserPanel;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
// Time wasted : 16hrs
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
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<UserPanel>();


    }
}
