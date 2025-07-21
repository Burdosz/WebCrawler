using Moq;
using WebCrawler.Services;

namespace WebCrawler.Tests.Services;

[TestFixture]
public class CrawlRequestSenderTests
{
    private Mock<IReadVisited> _visitedRepository;
    private Mock<IPageQueue> _pageQueue;
    private Mock<ICrawledUriBuilder> _uriBuilder;
    private CrawlRequestSender _sender;

    [SetUp]
    public void SetUp()
    {
        _visitedRepository = new Mock<IReadVisited>();
        _pageQueue = new Mock<IPageQueue>();
        _uriBuilder = new Mock<ICrawledUriBuilder>();

        _sender = new CrawlRequestSender(_visitedRepository.Object, _pageQueue.Object, _uriBuilder.Object);
    }

    [Test]
    public void TrySend_Then_Enqueues_And_ReturnsTrue_When_ValidUri_And_NotVisited()
    {
        var parentUri = new Uri("https://example.com");
        var link = "/about";
        var builtUri = new Uri("https://example.com/about");

        _uriBuilder.Setup(x => x.Build(parentUri, link)).Returns(builtUri);
        _visitedRepository.Setup(x => x.Exists(builtUri)).Returns(false);
        
        var result = _sender.TrySend(parentUri, link);
        
        Assert.That(result, Is.True);
        _pageQueue.Verify(x => x.Enqueue(builtUri), Times.Once);
        _visitedRepository.Verify(x => x.Exists(builtUri), Times.Once);
        _pageQueue.Verify(q => q.Enqueue(builtUri), Times.Once);
    }

    [Test]
    public void TrySend_DoesNotEnqueue_And_ReturnsFalse_When_ValidUri_And_AlreadyVisited()
    {
        var parentUri = new Uri("https://example.com");
        var link = "/about";
        var builtUri = new Uri("https://example.com/about");

        _uriBuilder.Setup(x => x.Build(parentUri, link)).Returns(builtUri);
        _visitedRepository.Setup(x => x.Exists(builtUri)).Returns(true);
        
        var result = _sender.TrySend(parentUri, link);
        
        Assert.That(result, Is.False);
        _visitedRepository.Verify(x => x.Exists(builtUri), Times.Once);
        _pageQueue.Verify(x => x.Enqueue(It.IsAny<Uri>()), Times.Never);
    }

    [Test]
    public void TrySend_DoesNotEnqueue_And_ReturnsFalse_When_UriBuilderReturnsNull()
    {
        var parentUri = new Uri("https://example.com");
        var link = "invalidLink";

        _uriBuilder.Setup(x => x.Build(parentUri, link)).Returns((Uri?)null);
        
        var result = _sender.TrySend(parentUri, link);
        
        Assert.That(result, Is.False);
        _pageQueue.Verify(x => x.Enqueue(It.IsAny<Uri>()), Times.Never);
        _visitedRepository.Verify(x => x.Exists(It.IsAny<Uri>()), Times.Never);
    }
}