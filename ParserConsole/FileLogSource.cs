using LogParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserConsole
{
    internal class FileLogSource : ILogSource
    {
        private string _filePath;

        public FileLogSource(string filePath)
        {
            _filePath = filePath;
        }

        public List<string> ReadLogs()
        {

            try
            {
                using var stream = File.OpenRead(_filePath);

                using var reader = new StreamReader(stream);
                string? currentLine;
                var logs = new List<string>();

                while ((currentLine = reader.ReadLine()) != null)
                {
                    logs.Add(currentLine);
                }

                var line = reader.ReadLine();

                return logs;
            }
            catch (Exception)
            {

                throw;
            }
           
        }


    }
}

