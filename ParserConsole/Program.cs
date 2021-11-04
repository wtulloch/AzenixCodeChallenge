// See https://aka.ms/new-console-template for more information
using LogParser;
using ParserConsole;
using static  System.Console;

var filePath = "example-data.log";
var baseUrl = "http://example.com";

try
{
    var logSource = new FileLogSource(filePath);
    var logEntries = new WebLogGenerator().GetLogEntries(logSource).Result;

    var logQueries = new LogQuery();
    
    WriteLine("Start");
    WriteLine();

    var numberOfUniqueClientIpAddresses = logQueries.GetNumberOfUniqueClientIpAddresses(logEntries);
    
    WriteLine($"Unique client IP Addresses is: {numberOfUniqueClientIpAddresses}");
    WriteLine();

    var topMostVistedUrls = logQueries.GetTopThreeMostVisitedUrls(logEntries, baseUrl);
    
    WriteLine("Top 3 most visited URLs");
    foreach(var url in topMostVistedUrls)
    {
        WriteLine($"\t {baseUrl}{url}");
    }
    WriteLine();

    var topThreeClientIpAddresses = logQueries.GetTopThreeMostActiveClientIpAddresses(logEntries);
    WriteLine("Top 3 Active client IP Addresses");
    foreach (var ipAddress in topThreeClientIpAddresses)
    {
        WriteLine($"\t{ipAddress}");
    }
    WriteLine();


}
catch (Exception ex)
{

    WriteLine(ex.Message);
   
}

WriteLine("press any key to close");
ReadLine();


