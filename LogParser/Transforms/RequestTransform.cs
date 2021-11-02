using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogParser.Transforms
{
    public class RequestTransform : IDataTransform<RequestInfo>
    {
        public RequestInfo Apply(string input)
        {
            if (string .IsNullOrEmpty(input))
            {
                return new RequestInfo();
            }

            var split = input.Split(' ');

            return new RequestInfo { HttpVerb = split[0], Request = split[1] };
        }
    }
}
