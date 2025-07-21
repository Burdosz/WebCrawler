using System.Reflection;

namespace WebCrawler.Services;

public interface ICrawlRequestSender
{
    bool TrySend(Uri parentUri, string link);
}

public class CrawlRequestSender : ICrawlRequestSender
{
    private readonly IReadVisited _visitedRepository;
    private readonly IPageQueue _pageQueue;
    private readonly ICrawledUriBuilder _uriBuilder;

    public CrawlRequestSender(IReadVisited visitedRepository, IPageQueue pageQueue, ICrawledUriBuilder uriBuilder)
    {
        _visitedRepository = visitedRepository;
        _pageQueue = pageQueue;
        _uriBuilder = uriBuilder;
    }
    
    public bool TrySend(Uri parentUri, string link)
    {
        var pageToDispatchUri = _uriBuilder.Build(parentUri, link);
        if (pageToDispatchUri is not null && !_visitedRepository.Exists(pageToDispatchUri))
        {
            _pageQueue.Enqueue(pageToDispatchUri);
            return true;
        }
        
        return false;
    }
}