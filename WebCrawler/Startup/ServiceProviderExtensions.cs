using Microsoft.Extensions.DependencyInjection;

namespace WebCrawler;

public class ServiceProviderExtensions
{
    public static Microsoft.Extensions.DependencyInjection.ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ICrawler, Crawler>();

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}