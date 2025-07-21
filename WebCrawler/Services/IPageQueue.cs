using System.Collections.Concurrent;

namespace WebCrawler.Services;

public interface IPageQueue
{
    void Enqueue(Uri uri);
    bool TryDequeue(out Uri uri);
    int Count();
}

public class PageQueue : IPageQueue
{
    private readonly ConcurrentQueue<Uri> _pagesToVisit = new();
    public void Enqueue(Uri uri)
    {
        _pagesToVisit.Enqueue(uri);
    }

    public bool TryDequeue(out Uri uri)
    {
        return _pagesToVisit.TryDequeue(out uri);
    }

    public int Count()
    {
        return _pagesToVisit.Count;
    }
}