using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.IO;

namespace MSB_Virus_Scanner
{
    public static class ScheduledTaskHelper
    {
        private static string _path = Path.Combine(@"C:\temp\MSB_Virus_Sentry\Results\", Environment.MachineName.ToString() + "_tasks.log");

        public static List<string> GetWeirdTasks()
        {
            List<string> tasks = new List<string>();

            DirSearch(@"C:\Windows\System32\Tasks", tasks);

            //Program.log.WriteTask(tasks);

            return tasks;
        }

        public static void DirSearch(string sDir, List<string> tasks)
        {
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    if ( ! Whitelist.tasks.Any( pattern => Path.GetFullPath(f).Contains(pattern) ) )
                    {
                        tasks.Add(Path.GetFullPath(f));
                    }
                }

                foreach (string d in Directory.EnumerateDirectories(sDir))
                {
                    DirSearch(d, tasks);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
    }
}
