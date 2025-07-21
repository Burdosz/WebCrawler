using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebCrawler.Config;
using WebCrawler.Services;

namespace WebCrawler.Tests.Integration
{
    public class CrawlerTests
    {
        private ServiceCollection _services;
        private Mock<IWriter> _writer;

        [SetUp]
        public void Setup()
        {
            _services = new ServiceCollection();
            RegisterServices(_services);
        }
        
        [Test]
        public async Task Crawler_Visits_Main_And_Subpages()
        {
            var mainUrl = "http://test.com";
            GivenWebsiteWithTwoSubpages(mainUrl);
            GivenMockedIWriter();
            var serviceProvider = _services.BuildServiceProvider();

            var crawler = serviceProvider.GetRequiredService<ICrawler>();
            var queue = serviceProvider.GetRequiredService<IPageQueue>();

            queue.Enqueue(new Uri(mainUrl));
            await crawler.Crawl();
            
            _writer.Verify(writer => writer.PrintVisited(It.IsAny<Uri>(), It.IsAny<string[]>()), Times.Exactly(3));
        }

        private void GivenWebsiteWithTwoSubpages(string mainUrl)
        {
            var mockHtmlWeb = new Mock<HtmlWeb>();

            var subpage1Url = $"{mainUrl}/subpage1";
            var subpage2Url = $"{mainUrl}/subpage2";
        
            // Main page HTML with 2 links
            var mainHtml = @"<html><body>
                <a href='http://test.com/subpage1'>Subpage 1</a>
                <a href='http://test.com/subpage2'>Subpage 2</a>
            </body></html>";
        
            // Subpage HTMLs
            var subpage1Html = "<html><body>Subpage 1 Content</body></html>";
            var subpage2Html = "<html><body>Subpage 2 Content</body></html>";
            
            mockHtmlWeb.Setup(x => x.Load(mainUrl)).Returns(CreateHtmlDoc(mainHtml));
            mockHtmlWeb.Setup(x => x.Load(subpage1Url)).Returns(CreateHtmlDoc(subpage1Html));
            mockHtmlWeb.Setup(x => x.Load(subpage2Url)).Returns(CreateHtmlDoc(subpage2Html));

            _services.AddSingleton<HtmlWeb>(_ => mockHtmlWeb.Object);
        }

        private void GivenMockedIWriter()
        {
            _writer = new Mock<IWriter>();
            _services.AddSingleton<IWriter>(_ => _writer.Object);
        }

        private HtmlDocument CreateHtmlDoc(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }
        
        private static void RegisterServices(ServiceCollection services)
        {
            services.Configure<CrawlerSettings>(_ => new CrawlerSettings());
        
            services.AddSingleton<VisitedRepository>();
            services.AddTransient<IWriteVisited>(sp => sp.GetRequiredService<VisitedRepository>());
            services.AddTransient<IReadVisited>(sp => sp.GetRequiredService<VisitedRepository>());
            services.AddSingleton<IPageQueue, PageQueue>();
            services.AddSingleton<ICrawlRequestSender, CrawlRequestSender>();
            services.AddSingleton<ICrawlRequestHandler, CrawlRequestHandler>();
            services.AddSingleton<ICrawler, CrawlRequestDispatcher>();
            services.AddSingleton<ICrawledUriBuilder, CrawledUriBuilder>();
        }
    }
}