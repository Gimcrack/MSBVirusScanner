using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MSB_Virus_Scanner
{
    public class Logger : ILogger
    {
        public static string logPath = string.Format(@"C:\temp\MSB_Virus_Sentry\Results\MSB_Virus_Scan_{0:yyyy-MM-dd_hh-mm-ss}.log", DateTime.Now);
        public static string tasksPath = string.Format(@"\\dsjkb\desoft$\MSB_Virus_Sentry\Results\{1}_{0:yyyy-MM-dd_hh-mm-ss}_tasks.log", DateTime.Now,Environment.MachineName);
        public static string infectedPath = string.Format(@"C:\temp\MSB_Virus_Sentry\Results\MSB_Virus_Scan_{0:yyyy-MM-dd_hh-mm-ss}_infections.log", DateTime.Now);

        public Logger()
        {
            if (!Directory.Exists(@"C:\temp"))
            {
                Directory.CreateDirectory(@"C:\temp");
            }

            if (!Directory.Exists(@"C:\temp\MSB_Virus_Sentry\Results") )
            {
                Directory.CreateDirectory(@"C:\temp\MSB_Virus_Sentry\Results");
            }

            cleanup();

            if (!File.Exists(logPath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(logPath))
                {
                    sw.WriteLine(Environment.NewLine);
                    sw.WriteLine("MSB Virus Scanner");
                    sw.WriteLine(DateTime.Now);
                    sw.WriteLine(Environment.NewLine);
                    sw.WriteLine(Environment.NewLine);
                }
            }



            //if (!File.Exists(tasksPath))
            //{
            //    File.CreateText(tasksPath);
            //}

        }

        public void write(string text)
        {
            using (StreamWriter sw = File.AppendText(logPath))
            {
                sw.WriteLine(DateTime.Now + "  --  " + text);
            }
        }

        public void write_task(string text)
        {
            using (StreamWriter sw = File.AppendText(tasksPath))
            {
                sw.WriteLine(DateTime.Now + "\t" + Environment.MachineName + "\t" + text);
            }
        }

        public void write_task(List<string> text)
        {
            string output = "";

            if (text.Count() < 1)
                return;

            foreach (string line in text)
            {
                output += DateTime.Now + "\t" + Environment.MachineName + "\t" + line.Replace(@"C:\Windows\System32\Tasks", "") + Environment.NewLine;
            }

            Console.WriteLine("Writing to text file");

            Console.WriteLine(output);

            File.AppendAllText(tasksPath, output);
        }

        public void write_infection( string text )
        {
            if (!File.Exists(infectedPath))
            {
                // Create a file to write to.
                using (StreamWriter sw2 = File.CreateText(infectedPath))
                {
                    sw2.WriteLine("MSB Virus Scanner Infections");
                    sw2.WriteLine(DateTime.Now);
                    sw2.WriteLine(Environment.NewLine);
                    sw2.WriteLine(Environment.NewLine);
                }
            }

            using (StreamWriter sw = File.AppendText(infectedPath))
            {
                sw.WriteLine(DateTime.Now + "  --  " + text);
            }
        }

        public void write_infection( IEnumerable<string> list )
        {
            foreach(string file in list)
            {
                write_infection("Infected file found: " + file);
            }
        }

        public void write_infection(IEnumerable<string> list, string matched_pattern) 
        {
            foreach (string file in list)
            {
                write_infection("Infected file found: " + file + " Pattern: " + matched_pattern);
            }
        }

        public void tear_down()
        {
            var l = new FileInfo(logPath);
            if (l.Exists) l.Delete();

            var i = new FileInfo(infectedPath);
            if (i.Exists) i.Delete();
        }

        public string get()
        {
            return File.ReadAllText(logPath).Replace(Environment.NewLine,@"<br/>" + Environment.NewLine);
        }

        private void cleanup()
        {
            Directory.GetFiles(@"C:\temp", "MSB_Virus_Scan*.log")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.CreationTime)
                .Skip(6)
                .ToList()
                .ForEach(f => f.Delete());
        }
    }
}
