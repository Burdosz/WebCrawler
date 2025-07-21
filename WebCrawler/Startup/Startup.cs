using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Config;
using WebCrawler.Services;

namespace WebCrawler.Startup;

public class Startup
{
    public static ServiceProvider BuildServiceProvider()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        
        var services = new ServiceCollection();
        
        services.Configure<CrawlerSettings>(configuration.GetSection(CrawlerSettings.SectionName));
        
        services.AddSingleton<VisitedRepository>();
        services.AddSingleton<HtmlWeb>(_ => new HtmlWeb());
        services.AddTransient<IWriteVisited>(sp => sp.GetRequiredService<VisitedRepository>());
        services.AddTransient<IReadVisited>(sp => sp.GetRequiredService<VisitedRepository>());
        services.AddSingleton<IPageQueue, PageQueue>();
        services.AddSingleton<ICrawlRequestSender, CrawlRequestSender>();
        services.AddSingleton<ICrawlRequestHandler, CrawlRequestHandler>();
        services.AddSingleton<ICrawler, CrawlRequestDispatcher>();
        services.AddSingleton<IWriter, ConsoleWriter>();
        services.AddSingleton<ICrawledUriBuilder, CrawledUriBuilder>();
        
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}