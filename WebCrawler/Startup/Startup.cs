using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Config;

namespace WebCrawler;

public class Startup
{
    public static ServiceProvider BuildServiceProvider()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        
        var services = new ServiceCollection();
        
        services.Configure<CrawlerSettings>(configuration.GetSection(CrawlerSettings.SectionName));
        
        services.AddSingleton<ICrawler, Crawler>();
        
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}