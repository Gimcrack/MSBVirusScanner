using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSB_Virus_Scanner
{
    class LogHandler
    {
        public List<ILogger> logs;

        public LogHandler()
        {
            logs = new List<ILogger>();

            Add(new EventLogger());

            if (Program.database_logging) 
                Add(new DatabaseLogger());
        }

        // add a Logger 
        public void Add( ILogger Logger )
        {
            logs.Add(Logger);
        }
    
        // clean up
        public void CleanUp()
        {
            foreach(var l in logs)
            {
                l.tear_down();
            }
        }

        public void Write(string message)
        {
            foreach (var l in logs)
            {
                try
                {
                    l.write(message);
                }

                catch(Exception e)
                {
                    // don't worry about these.
                }
                
            }
        }

        public void WriteInfection(string message)
        {
            foreach (var l in logs)
            {
                try
                {
                    l.write_infection(message);
                }

                catch (Exception e)
                {
                    // don't worry about these.
                }
            }
        }

        public void WriteInfection(IEnumerable<string> list)
        {
            foreach (var l in logs)
            {
                try
                {
                    l.write_infection(list);
                }

                catch (Exception e)
                {
                    // don't worry about these.
                }
            }
        }

        public void WriteInfection(IEnumerable<string> list, string matched_pattern)
        {
            foreach (var l in logs)
            {
                try
                {
                    l.write_infection(list, matched_pattern);
                }

                catch (Exception e)
                {
                    // don't worry about these.
                }
                
            }
        }

        public string Get()
        {
            string ret = "";

            foreach(var l in logs)
            {
                ret += l.get();
            }

            return ret;
        }

        
    }
}
