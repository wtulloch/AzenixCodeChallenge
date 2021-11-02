using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogParser.Transforms
{
    public class TimestampTransform : IDataTransform<DateTime?>
    {
        private string datePattern = "dd/MMM/yyyy:HH:mm:ss zzzz";
        public TimestampTransform()
        {

        }
        public DateTime? Apply(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            return DateTime.ParseExact(input, datePattern, CultureInfo.InvariantCulture);
        }
    }
}
