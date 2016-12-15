using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MSB_Virus_Scanner
{
    class EventLogger : ILogger
    {

        private EventLog Events = new EventLog("Application");

        public EventLogger()
        {
            Events.Source = "MSB Virus Sentry";
        }

        public void write(string text)
        {
            Console.WriteLine(text);
            Events.WriteEntry(text, EventLogEntryType.Information);
        }

        public void write_infection(string text)
        {
            Console.WriteLine(text);
            Events.WriteEntry(text, EventLogEntryType.Error);
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
            return "Review Event Log: MSB, Source: Virus Scanner";
        }

        private void cleanup()
        {
            
        } 
    }
}
