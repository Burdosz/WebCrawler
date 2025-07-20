// See https://aka.ms/new-console-template for more information

using System.Text;

Console.WriteLine("Hello, World!");

StringBuilder builder = new();
builder.AppendLine("The following arguments are passed:");

var targetSite = args[0]; // first argument for the console app should be the website to crawl.

//Validate argument
if (!Uri.TryCreate(targetSite, UriKind.Absolute, out var targetSiteUri))
{
    Console.WriteLine("Invalid uri provided");
}

// Setup crawling
// Figure out the domain, set all bells and whistles
