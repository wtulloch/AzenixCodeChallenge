using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LogParser
{
    public record LogEntry
    {
        public IPAddress ClientIpAddress { get; init; }
        public DateTime? Timestamp { get; init; }
        public string HttpVerb { get; init; }
        public string Resource { get; init; }
        public int  HttpStatusCode { get; init; }

    }
}
