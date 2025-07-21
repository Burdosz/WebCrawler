namespace WebCrawler.Services;

public interface ICrawledUriBuilder
{
    Uri? Build(Uri parentPage, string link);
}

public class CrawledUriBuilder : ICrawledUriBuilder
{
    public Uri? Build(Uri parentPage, string link)
    {
        if (string.IsNullOrWhiteSpace(link)) 
            return null;
        
        if (link.StartsWith("/"))
        {
            return new Uri(new Uri(parentPage.GetLeftPart(UriPartial.Authority)), link);
        }

        return null;
    }
}