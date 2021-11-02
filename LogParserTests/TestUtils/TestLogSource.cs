using LogParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogParserTests.TestUtils
{
    internal class TestLogSource : ILogSource
    {

        public List<string> ReadLogs()
        {
           using var stream = ReadResource("TestUtils", "example-data.log");
           
            using var reader = new StreamReader(stream);


            var line = reader.ReadLine();

            return new List<string> { line };

        }

        private static Stream ReadResource(string folder, string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var assemblyName = assembly.GetName().Name;

            var resource = $"{assemblyName}.{folder}.{fileName}";

            return assembly.GetManifestResourceStream(resource);
        }
    }
}
