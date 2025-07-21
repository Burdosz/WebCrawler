namespace WebCrawler.Config;

public class CrawlerSettings
{
    public const string SectionName = "Crawler";
    public int MaxConcurrentScraping { get; set; } = 4;
}