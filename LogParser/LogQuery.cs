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

        public int GetNumberOfUniqueIpAddress(List<LogEntry> logEntries)
        {
            return logEntries.Distinct(new IpAddressComparer()).Count();
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

            return x.IpAddress.Equals(y.IpAddress);
        }

        public int GetHashCode([DisallowNull] LogEntry obj)
        {
           return obj.IpAddress.GetHashCode();
        }
    }
}
