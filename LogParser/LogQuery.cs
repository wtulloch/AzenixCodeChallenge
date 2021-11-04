using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogParser
{
    public class LogQuery
    {

        public int GetNumberOfUniqueClientIpAddresses(List<LogEntry> logEntries)
        {
            return logEntries.Distinct(new IpAddressComparer()).Count();
        }

        public List<string> GetTopThreeMostVisitedUrls(List<LogEntry> logEntries, string baseUrl="")
        {
            var rx = new Regex(@"/\w*", RegexOptions.Compiled);

            var urls = logEntries.Where(le => le.HttpStatusCode == 200)
                .Select(le => le.Resource)
                .Where(s => !(s.Contains(".js") || s.Contains(".css")))
                .Select(s => (baseUrl == string.Empty ? s : s.Replace(baseUrl,"")));

           var groupUrls = urls.GroupBy(s => rx.Match(s).Value);

            var returnUrls = groupUrls.OrderByDescending(g => g.Count())
                            .Select(g => g.Key)
                            .Take(3);


            return returnUrls.ToList();
        }

        public List<IPAddress> GetTopThreeMostActiveClientIpAddresses(List<LogEntry> logEntries)
        {
                      
            var topClientIpAddresses = logEntries
                                           .GroupBy(le => le.ClientIpAddress)
                                           .OrderByDescending(g => g.Count())
                                           .Take(3)
                                           .Select(g => g.Key);
            
            return topClientIpAddresses.ToList();
        }
    }

    internal class IpAddressComparer : IEqualityComparer<LogEntry>
    {
        public bool Equals(LogEntry? x, LogEntry? y)
        {
            if( x == null && y == null)
            {
                return false;
            } 
            if( x == null || y == null )
            {
                return false;
            }

            return x.ClientIpAddress.Equals(y.ClientIpAddress);
        }

        public int GetHashCode([DisallowNull] LogEntry obj)
        {
           return obj.ClientIpAddress.GetHashCode();
        }
    }

    internal class DateIpAddressComparer : IEqualityComparer<LogEntry>
    {
        public bool Equals(LogEntry? x, LogEntry? y)
        {
            if (x == null && y == null)
            {
                return false;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return (x.ClientIpAddress.Equals(y.ClientIpAddress) && x.Timestamp == y.Timestamp);
        }

        public int GetHashCode([DisallowNull] LogEntry obj)
        {
            return string.Concat(obj.ClientIpAddress.ToString(), obj.Timestamp.ToString()).GetHashCode() ;
        }
    }
}
