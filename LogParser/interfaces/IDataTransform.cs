using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogParser
{
    public interface IDataTransform<T>
    {
        T Apply(string input);
    }
}
