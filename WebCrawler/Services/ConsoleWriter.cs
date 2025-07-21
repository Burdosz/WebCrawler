namespace WebCrawler.Services;

public interface IWriter
{
    public void PrintVisited(Uri visitedLink, IEnumerable<string> subpages);
}

public class ConsoleWriter : IWriter
{
    private readonly object _lockObj = new();
    
    public void PrintVisited(Uri visitedLink, IEnumerable<string> subpages)
    {
        lock (_lockObj)
        {
            Console.WriteLine(visitedLink);
            Console.Write("\t");
            Console.Write(string.Join("\n\t",subpages));
            Console.WriteLine();
        }
    }
}