namespace WebCrawler.Services;

public interface ICrawledUriBuilder
{
    Uri? Build(Uri parentPage, string link);
}

public class CrawledUriBuilder : ICrawledUriBuilder
{
    public Uri? Build(Uri parentPage, string link)
    {
        //It would be nice to have a separate service to validate the URI is satisfying the criteria.
        if (string.IsNullOrWhiteSpace(link))
            return null;
        
        if (link.StartsWith("/"))
        {
            return new Uri(new Uri(parentPage.GetLeftPart(UriPartial.Authority)), link);
        }

        return null;
    }
}