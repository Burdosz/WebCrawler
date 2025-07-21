using System.Collections.Concurrent;

namespace WebCrawler.Services;

public interface IWriteVisited
{
    bool TryAdd(Uri uri);
}

public interface IReadVisited
{
    bool Exists(Uri uri);
}

public class VisitedRepository : IReadVisited, IWriteVisited
{
    private readonly ConcurrentDictionary<string, byte> _visitedPages = new();
    
    public bool Exists(Uri uri)
    {
        return _visitedPages.ContainsKey(GetKey(uri));
    }

    public bool TryAdd(Uri uri)
    {
        return _visitedPages.TryAdd(GetKey(uri), Byte.MinValue); // .NET doesn't offer ConcurrentHashSet, so we use dictionary instead. We don't care about the value for the key.
    }
    
    private static string GetKey(Uri uri)
    {
        return uri.AbsoluteUri;
    }
}
