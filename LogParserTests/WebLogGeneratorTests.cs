using LogParser;
using LogParserTests.TestUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using NSubstitute;
using System.Net;

namespace LogParserTests
{
    public class WebLogGeneratorTests
    {
        private  ILogSource _logSource;
        private WebLogGenerator _webLogGenerator;

        public WebLogGeneratorTests()
        {
            _logSource = Substitute.For<ILogSource>();
            _webLogGenerator = new WebLogGenerator();
        }

        [Fact]
        public async Task GetLogEntries_Given23LogEntries_ShouldReturnAListWithACountOf23()
        {
            _logSource = new TestLogSource();

            var results = await _webLogGenerator.GetLogEntries(_logSource);

            results.Count.ShouldBe(23);

        }

        [Fact]
        public async Task GetLogEntries_GivenASingleRawLogEntry_ReturnsALogEntryWithExpectedValues()
        {
            _logSource.ReadLogs().Returns(new List<string> { 
                "177.71.128.21 - admin [10/Jul/2018:22:21:28 +0200] \"GET /intranet-analytics/ HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7\"" 
            });

            var expectedLogEntry = new LogEntry
            {
                IpAddress = IPAddress.Parse("177.71.128.21"),
                Timestamp = new DateTime(2018, 7, 11, 6, 21, 28),
                HttpVerb = "GET",
                Resource = "/intranet-analytics/",
                HttpStatusCode = 200,
            };

            var results = await _webLogGenerator.GetLogEntries(_logSource);

           results[0].ShouldBe(expectedLogEntry);

        }
    }
}
