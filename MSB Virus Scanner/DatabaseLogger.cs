using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSB_Virus_Scanner
{
    class DatabaseLogger : ILogger
    {

        public void write(string text)
        {
            Model.LogEntry(text, "", "Information");
        }

        public void write_infection(string text)
        {
            Console.WriteLine(text);
            Model.LogEntry(text, "Scan","Infected");
        }

        public void write_infection(IEnumerable<string> list)
        {
            write_infection("Infected file found: " + Environment.NewLine + String.Join(Environment.NewLine, list));
        }

        public void write_infection(IEnumerable<string> list, string matched_pattern)
        {
            write_infection(String.Format("Infected file found: {0} Pattern: {1} {2} {3}",
                Environment.NewLine,
                matched_pattern,
                Environment.NewLine,
                String.Join(Environment.NewLine, list)
                )
            );
        }

        public void tear_down()
        {
            
        }

        public string get()
        {
            return "";
        }

        private void cleanup()
        {
            
        } 
    }
}
