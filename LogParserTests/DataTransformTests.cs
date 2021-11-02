using LogParser.Transforms;
using System;
using Xunit;
using Shouldly;
using LogParser;

namespace LogParserTests
{
    public class DataTransformTests
    {
        [Fact]
        public void TimestampTransform_GivenAnEmptyString_ReturnsNull()
        {
            var transform = new TimestampTransform();

            var result = transform.Apply(string.Empty);

            result.ShouldBeNull();
        }

        [Fact]
        public void TimestampTransform_GivenAValidLogTimestamp_ReturnsExpectedDateTime()
        {
            var expectedDateTime = new DateTime(2020, 11, 1, 10, 15, 30);
            var valueToTransform = expectedDateTime.ToString("dd/MMM/yyyy:HH:mm:ss zzz");

            var transform = new TimestampTransform();

            var result = transform.Apply(valueToTransform);

            result.ShouldBe(expectedDateTime); ;
        }

        [Fact]
        public void RequestTransform_GivenAnEmptyString_ReturnsAnEmptyRequestInfoInstance()
        {
            var transform = new RequestTransform();

            var result = transform.Apply(String.Empty);

            result.ShouldBeOfType<RequestInfo>();
        }

        [Fact]
        public void RequestTransform_GivenAStringWithValidRequestInfo_ReturnsAPopulatedRequestInfoInstance()
        {
            var transform = new RequestTransform();
            var requestValue = "GET /intranet-analytics/ HTTP/1.1";
            
            var result = transform.Apply(requestValue);

            result.HttpVerb.ShouldBe("GET");
            result.Request.ShouldBe("/intranet-analytics/");
        }
    }
}