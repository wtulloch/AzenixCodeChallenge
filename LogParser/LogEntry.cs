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
        public IPAddress IpAddress { get; set; }
        public DateTime? Timestamp { get; set; }
        public string HttpVerb { get; set; }
        public string Resource { get; set; }
        public int  HttpStatusCode { get; set; }

    }
}
