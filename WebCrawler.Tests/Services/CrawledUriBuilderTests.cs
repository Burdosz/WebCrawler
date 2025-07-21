using WebCrawler.Services;

namespace WebCrawler.Tests.Services;

[TestFixture]
public class CrawledUriBuilderTests
{
    private CrawledUriBuilder _builder;

        [SetUp]
        public void SetUp()
        {
            _builder = new CrawledUriBuilder();
        }

        [Test]
        public void Build_RelativePath_ReturnsExpectedUri()
        {
            var parentUri = new Uri("https://example.com/page");
            var link = "/about";
            
            var result = _builder.Build(parentUri, link);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(new Uri("https://example.com/about")));
        }
        
        [Test]
        public void Build_RelativePathWithSubdirectory_ReturnsCorrectUri()
        {
            var parentUri = new Uri("https://example.com/foo/bar");
            var link = "/contact";
            
            var result = _builder.Build(parentUri, link);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(new Uri("https://example.com/contact")));
        }
        
        [Theory]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("https://other.com/page")]
        [TestCase("https://subdomain.example.com/")]
        [TestCase("about")]
        public void Build_NullOrEmptyLink_ReturnsNull(string? link)
        {
            var parentUri = new Uri("https://example.com");

            var result = _builder.Build(parentUri, link!);
            
            Assert.That(result, Is.Null);
        }
}