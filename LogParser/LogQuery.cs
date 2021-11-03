using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
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
            return new List<string>();
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
}
