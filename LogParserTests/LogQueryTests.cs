﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogParser;
using Xunit;
using Shouldly;
using System.Net;

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
            var logEntries = new List<LogEntry> { CreateRequestInfo("127.1.120.125", DateTime.Now,"/home/"),
                 CreateRequestInfo("127.1.120.125", DateTime.Now,"/home/personal/"),
                  CreateRequestInfo("182.1.200.101", DateTime.Now,"/home/")
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

        private LogEntry CreateRequestInfo(string ipAddress, DateTime timetamp, string resourcePath)
        {
            return new LogEntry
            {
                ClientIpAddress = IPAddress.Parse(ipAddress),
                Timestamp = timetamp,
                HttpStatusCode = 200,
                HttpVerb = "GET",
                Resource = resourcePath
            };
        }
    }
}
