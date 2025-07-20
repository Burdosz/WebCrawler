public interface ICrawler
{
    public void Crawl(Uri uri);
}

public class Crawler : ICrawler
{
    public Crawler(IWriter writer)
    {
        
    }
    public void Crawl(Uri uri)
    {
        throw new NotImplementedException();
    }
}