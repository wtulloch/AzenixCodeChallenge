using LogParser.Transforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogParser
{
    public class WebLogGenerator
    {

        private string  _regExPattern = @"(?<ipAddress>^\d{0,3}.\d{0,3}.\d{0,3}.\d{0,3}) .* \[(?<timestamp>.*)\] (?<request>([""'])(?:(?=(\\?))\2.)*?\1) (?<statusCode>\d{3}) (?<bytesSent>\d{3})";


        public async Task<List<LogEntry>> GetLogEntries(ILogSource logSource)
        {
            var regEx = new Regex(_regExPattern,RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var logEntries = new List<LogEntry>();
            var timestampTransform = new TimestampTransform();
            var requestTransform = new RequestTransform();

            foreach (var log in logSource.ReadLogs())
            {
                if (regEx.IsMatch(log))
                {
                    var groups = regEx.Match(log).Groups;
                    var resourceInfo = requestTransform.Apply(groups["request"].Value);
                    var logEntry = new LogEntry { 
                        IpAddress = IPAddress.Parse(groups["ipAddress"].Value),
                        Timestamp = timestampTransform.Apply(groups["timestamp"].Value),
                        HttpVerb = resourceInfo.HttpVerb,
                        Resource = resourceInfo.Request,
                        HttpStatusCode = int.Parse(groups["statusCode"].Value)
                    };

                    logEntries.Add(logEntry);
                }
            }

            return await Task.FromResult(logEntries);
        }

    }
}
