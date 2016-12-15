using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSB_Virus_Scanner
{
    interface ILogger
    {
        void write(string text);

        void write_infection(string text);

        void write_infection(IEnumerable<string> list);

        void write_infection(IEnumerable<string> list, string matched_pattern);

        void tear_down();

        string get();
    }
}
