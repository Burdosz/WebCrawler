using HtmlAgilityPack;

namespace WebCrawler.Services;

public interface ICrawlRequestHandler
{
    Task Handle(Uri page);
}

public class CrawlRequestHandler : ICrawlRequestHandler
{
    private readonly IWriteVisited _visitedRepository;
    private readonly IWriter _writer;
    private readonly ICrawlRequestSender _crawlRequestSender;
    private readonly HtmlWeb _htmlWeb;

    public CrawlRequestHandler(IWriteVisited visitedRepository, IWriter writer, ICrawlRequestSender crawlRequestSender, HtmlWeb htmlWeb)
    {
        _visitedRepository = visitedRepository;
        _writer = writer;
        _crawlRequestSender = crawlRequestSender;
        _htmlWeb = htmlWeb;
    }
    
    public async Task Handle(Uri page)
    {
        if (!_visitedRepository.TryAdd(page)) 
            return;
                    
        var pageDocument = await _htmlWeb.LoadFromWebAsync(page.AbsoluteUri);
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