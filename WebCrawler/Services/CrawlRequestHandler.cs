using HtmlAgilityPack;

namespace WebCrawler.Services;

public interface ICrawlRequestHandler
{
    Task Handle(Uri page, HtmlWeb web);
}

public class CrawlRequestHandler : ICrawlRequestHandler
{
    private readonly IWriteVisited _visitedRepository;
    private readonly IWriter _writer;
    private readonly ICrawlRequestSender _crawlRequestSender;

    public CrawlRequestHandler(IWriteVisited visitedRepository, IWriter writer, ICrawlRequestSender crawlRequestSender)
    {
        _visitedRepository = visitedRepository;
        _writer = writer;
        _crawlRequestSender = crawlRequestSender;
    }
    
    public async Task Handle(Uri page, HtmlWeb web)
    {
        if (!_visitedRepository.TryAdd(page)) 
            return;
                    
        var pageDocument = await web.LoadFromWebAsync(page.AbsoluteUri);
        var links = pageDocument.DocumentNode.SelectNodes("//a[@href]");
        if (links == null)
            return;
                    
        var subPages = links
            .Select(l =>
            {
                var link = l.GetAttributeValue("href", string.Empty);
                _crawlRequestSender.TrySend(page, link);
                
                return link;
            })
            .Where(l => !string.IsNullOrEmpty(l))
            .ToList();

        _writer.PrintVisited(page, subPages);
    }
}