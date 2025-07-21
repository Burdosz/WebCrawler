using Microsoft.Extensions.DependencyInjection;
using WebCrawler;
using WebCrawler.Services;
using WebCrawler.Startup;

var targetSite = args[0]; // first argument for the console app should be the website to crawl.

if (!Uri.TryCreate(targetSite, UriKind.Absolute, out var targetSiteUri))
{
    Console.WriteLine("Invalid uri provided");
    return;
}

var serviceProvider = Startup.BuildServiceProvider();
var crawler = serviceProvider.GetRequiredService<ICrawler>();
var queue = serviceProvider.GetRequiredService<IPageQueue>();

queue.Enqueue(targetSiteUri);
await crawler.Crawl();
