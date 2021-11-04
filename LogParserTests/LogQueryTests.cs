using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogParser;
using Xunit;
using Shouldly;
using System.Net;
using LogParserTests.TestUtils;

namespace LogParserTests
{
    public class LogQueryTests
    {
        private LogQuery _logQuery;

        public LogQueryTests()
        {
            _logQuery = new LogQuery();
        }

        [Fact]
        public void GetNumberOfUniqueClientIpAddress_Given3LogEntries_With2UniqueIpAddresses_ReturnsTheValue2()
        {
            var logEntries = new List<LogEntry> { CreateLogEntry("127.1.120.125", DateTime.Now,"/home/"),
                 CreateLogEntry("127.1.120.125", DateTime.Now,"/home/personal/"),
                  CreateLogEntry("182.1.200.101", DateTime.Now,"/home/")
            };

            var result = _logQuery.GetNumberOfUniqueClientIpAddresses(logEntries);

            result.ShouldBe(2);
        }

        [Fact]
        public void GetNumberOfUniqueClientIpAddress_GivenAnEmptyList_ReturnsZero()
        {
            var logEntries = new List<LogEntry>();

            var result = _logQuery.GetNumberOfUniqueClientIpAddresses(logEntries);

            result.ShouldBe(0);
        }

        [Fact]
        public void GetTopThreeMostVisitedUrls_DoesNotContainNon200Responses()
        {
            var logEntries = new List<LogEntry> {
                CreateLogEntry("127.0.0.1", DateTime.Now, "/redirect", 301),
                CreateLogEntry("127.0.0.1", DateTime.Now,"/")
        };
            var results = _logQuery.GetTopThreeMostVisitedUrls(logEntries);

            results.ShouldHaveSingleItem();
            results[0].ShouldBe("/");
        }

        [Fact]
        public void GetTopThreeMostVisitedUrls_ShouldNotReturnCssOrJsAddresses()
        {
            var logEntries = new List<LogEntry> {
                CreateLogEntry("127.0.0.1", DateTime.Now, "/sample.js"),
                CreateLogEntry("127.0.0.1", DateTime.Now,"/styles.css")
        };
            var results = _logQuery.GetTopThreeMostVisitedUrls(logEntries);

            results.ShouldBeEmpty();
        }

        [Fact]
        public void GetTopThreeMostVisitedUrls_RemovesBaseUrl()
        {
            var baseUrl = "http://example.com";
            var logEntries = new List<LogEntry> {
                CreateLogEntry("127.0.0.1", DateTime.Now, "/blog"),
                CreateLogEntry("127.0.0.1", DateTime.Now,$"{baseUrl}/blog")
            };
            var results = _logQuery.GetTopThreeMostVisitedUrls(logEntries, baseUrl);

            results.ShouldNotContain($"{baseUrl}/blog");
        }

        [Fact]
        public void GetTopThreeMostVisitedUrls_ShouldWorkWithNoBaseUrlProperty()
        {
            var baseUrl = "http://example.com";
            var logEntries = new List<LogEntry> {
                CreateLogEntry("127.0.0.1", DateTime.Now, "/blog"),
                CreateLogEntry("127.0.0.1", DateTime.Now,$"{baseUrl}/blog")
            };
            var results = _logQuery.GetTopThreeMostVisitedUrls(logEntries);

            results.ShouldNotContain($"{baseUrl}/blog");
        }

        [Fact]
        public void GetTopThreeMostVisitedUrls_GivenTwoUniqueUrls_ReturnsTwoUrls()
        {
            var baseUrl = "http://example.com";
            var logEntries = new List<LogEntry> {
                CreateLogEntry("127.0.0.1", DateTime.Now, "/blog"),
                CreateLogEntry("127.0.0.1", DateTime.Now,"/blog/2021"),
                CreateLogEntry("127.0.0.1", DateTime.Now,$"{baseUrl}/faq/test"),
            };
            var results = _logQuery.GetTopThreeMostVisitedUrls(logEntries, baseUrl);

            results.ShouldBe(new[] { "/blog", "/faq" });
        }
        [Fact]
        public void GetTopThreeMostVisitedUrls_GivenFourUniqueUrls_ReturnsTopThree()
        {
            var baseUrl = "http://example.com";
            var logEntries = new List<LogEntry> {
                    CreateLogEntry("127.0.0.1", DateTime.Now, "/blog"),
                    CreateLogEntry("127.0.0.1", DateTime.Now,"/blog/2021"),
                    CreateLogEntry("127.0.0.1", DateTime.Now,$"{baseUrl}/faq/test"),
                    CreateLogEntry("127.0.0.1", DateTime.Now,"/blog/2021"),
                    CreateLogEntry("127.0.0.1", DateTime.Now, "/"),
                    CreateLogEntry("127.0.0.1", DateTime.Now, "/faq/readme"),
                    CreateLogEntry("127.0.0.1", DateTime.Now, "/oops"),
                    CreateLogEntry("127.0.0.1", DateTime.Now, "/"),

                };
            var results = _logQuery.GetTopThreeMostVisitedUrls(logEntries, baseUrl);

            results.ShouldBe(new[] { "/blog", "/faq", "/" });
        }



        [Fact]
        [Trait("Category","integration")]
        public async Task GetThreeMostedVistiedUrls_GivenSampleLog_ShouldWork()
        {
            var logSource = new TestLogSource();
            var logEntries = await new WebLogGenerator().GetLogEntries(logSource);

            var results = _logQuery.GetTopThreeMostVisitedUrls(logEntries);

            results.Count.ShouldBeInRange(0, 3);
        }

        private LogEntry CreateLogEntry(string ipAddress, DateTime timetamp, string resourcePath, int httpStatusCode = 200)
        {
            return new LogEntry
            {
                ClientIpAddress = IPAddress.Parse(ipAddress),
                Timestamp = timetamp,
                HttpStatusCode = httpStatusCode,
                HttpVerb = "GET",
                Resource = resourcePath
            };
        }
    }
}
