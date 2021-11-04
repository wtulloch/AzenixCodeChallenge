// See https://aka.ms/new-console-template for more information
using LogParser;
using ParserConsole;
using static  System.Console;

var filePath = "example-data.log";

try
{
    var logSource = new FileLogSource(filePath);
    var logEntries = new WebLogGenerator().GetLogEntries(logSource).Result;

    var logQueries = new LogQuery();

    var numberOfUniqueClientIpAddresses = logQueries.GetNumberOfUniqueClientIpAddresses(logEntries);

    WriteLine($"Unique client IP Addresses is: {numberOfUniqueClientIpAddresses}");

    var topMostVistedUrls = logQueries.GetTopThreeMostVisitedUrls(logEntries);
    WriteLine();
    WriteLine("Top 3 most visited URLs");
    foreach(var url in topMostVistedUrls)
    {
        WriteLine($"\t {url}");
    }

}
catch (Exception ex)
{

    WriteLine(ex.Message);
   
}

WriteLine("press any key to close");
ReadLine();


