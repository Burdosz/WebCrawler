using Microsoft.Extensions.Options;
using WebCrawler.Config;

namespace WebCrawler.Services;

public interface ICrawler
{
    public Task Crawl();
}

public class CrawlRequestDispatcher : ICrawler
{
    private readonly IPageQueue _pageQueue;
    private readonly ICrawlRequestHandler _crawlRequestHandler;
    private readonly CrawlerSettings _crawlerSettings;

    public CrawlRequestDispatcher(IPageQueue pageQueue, ICrawlRequestHandler crawlRequestHandler,
        IOptions<CrawlerSettings> crawlerSettings)
    {
        _pageQueue = pageQueue;
        _crawlRequestHandler = crawlRequestHandler;
        _crawlerSettings = crawlerSettings.Value;
    }
    public async Task Crawl()
    {
        var tasks = new List<Task>();
        
        while (_pageQueue.Count() > 0)
        {
            while (tasks.Count < _crawlerSettings.MaxConcurrentScraping && _pageQueue.TryDequeue(out var page))
            {
                tasks.Add(Task.Run(() => _crawlRequestHandler.Handle(page)));
            }

            await Task.WhenAny(tasks);
            tasks.RemoveAll(t => t.IsCompleted);
        }
    }
}