using Microsoft.Extensions.DependencyInjection;
using WebCrawler;

var targetSite = args[0]; // first argument for the console app should be the website to crawl.

if (!Uri.TryCreate(targetSite, UriKind.Absolute, out var targetSiteUri))
{
    Console.WriteLine("Invalid uri provided");
    return;
}

var serviceProvider = Startup.BuildServiceProvider();
var crawler = serviceProvider.GetRequiredService<ICrawler>();

crawler.Crawl(targetSiteUri);
