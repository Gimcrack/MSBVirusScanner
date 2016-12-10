using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MSB_Virus_Scanner
{
    public class Logger
    {
        public static string logPath = string.Format(@"C:\temp\MSB_Virus_Scan_{0:yyyy-MM-dd_hh-mm-ss}.log", DateTime.Now);
        public static string infectedPath = string.Format(@"C:\temp\MSB_Virus_Scan_{0:yyyy-MM-dd_hh-mm-ss}_infections.log", DateTime.Now);

        public Logger()
        {
            if (!Directory.Exists(@"C:\temp"))
            {
                Directory.CreateDirectory(@"C:\temp");
            }

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
        }

        public void write(string text)
        {
            using (StreamWriter sw = File.AppendText(logPath))
            {
                sw.WriteLine(DateTime.Now + "  --  " + text);
            }
        }

        public void write_infection( string text )
        {
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

        public string get()
        {
            return File.ReadAllText(logPath).Replace(Environment.NewLine,@"<br/>" + Environment.NewLine);
        }
    }
}
