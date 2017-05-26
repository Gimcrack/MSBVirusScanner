using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;

namespace MSB_Virus_Scanner
{
    public class Scanner
    {
        public Boolean infected;

        public Boolean sentry;

        private Boolean stop_on_find;

        public string matched_pattern;

        public IEnumerable<string> infected_files;

        private IEnumerable<string> non_wildcard_patterns;

        private IEnumerable<string> wildcard_patterns;

        private IEnumerable<string> file_whitelist;

        private IEnumerable<string> global_whitelist;

        public Scanner()
        {
            stop_on_find = (ConfigurationManager.AppSettings["action_on_find"] == "stop");

            file_whitelist = new List<string>()
            {
                @"C:\Program Files (x86)\PostgreSQL\doc\contrib\README.pgcrypto",
                @"C:\Program Files\WindowsApps\Microsoft.FreshPaint_3.1.10156.0_x86__8wekyb3d8bbwe\ViewModels\Tools\!ReadMe.txt",
                @"C:\Program Files\WindowsApps\Microsoft.FreshPaint_3.1.10156.0_x86__8wekyb3d8bbwe\Views\Controls\!ReadMe.txt",
                @"CS-USAH.cry",
                @"C:\Program Files\Git\usr\share\vim\vim74\doc\recover.txt",
                @"TEDefault.scl",
                @"TEScale.scl",
                @"!ReadMe.txt",
                @"vim\vim74\doc\recover.txt",
                @"vim\vim80\doc\recover.txt",
                @"htbasic.crypt",
                @"C:\Program Files\Wireshark\radius\dictionary.alcatel-lucent.aaa",
                @"C:\Program Files (x86)\PFU\ScanSnap\Receipt\SsReceipt.exe",
                @"C:\Program Files (x86)\PFU\ScanSnap\Receipt\SsReceipt.exe.config",
                @"C:\Program Files (x86)\Visioneer\OneTouch 4.0\Profiles\Visioneer Strobe XP 470.isis",
                @"deps\v8\Makefile.android",
                @"loading_animat_4b41fc5ceedc968a4ee527dc6061dbdc[1].atlas"
            };

            global_whitelist = new List<string>() 
            {
                "*.AFD",
                "*decipher*",
                "*.abc",
                "*_recover_*.*",
                "*+recover+*.*",
                "*-recover-*.*",
                "*cpyt*",
                "*.*locked",
                "locked.bmp",
                "*.xxx",
                "message.txt",
                "*.ttt",
                "*.ccc",
                "*.adr",
                ".~",
                "*.btc",
                "*.xyz",
                "*.cbf",
                "*.zzz",
                "*.stn",
                "*.one",
                "*.1",
                "*.tmp.exe",
                "tor.exe",
                "*.adr",
                "*.gg",
                "_ryp",
                "*.ico"

            };

            getPatterns();
        }

        public void Scan()
        {

            foreach (DriveInfo drive in Program.GetLocalDrives() )
            {
                if (infected && stop_on_find) break;
                scanFolder(drive.RootDirectory.ToString());
            }

            if (infected)
            {
                Program.log.Write("Infection Found" + Environment.NewLine + "Infected Files" + Environment.NewLine + String.Join(Environment.NewLine, infected_files));

                Console.WriteLine("Infected files");
                
                foreach (string file in infected_files)
                {
                    Console.WriteLine(file);
                }
            }
        }

        private void getPatterns()
        {
            string url = "https://fsrm.experiant.ca/api/v1/combined";

            var userdefined_patterns = (ConfigurationManager.AppSettings["patterns"].Length > 0) ?
                    ConfigurationManager.AppSettings["patterns"].Split('|').ToList() :
                    new List<string>() { "aeraimangangaingaiegannnanrg" };

            var whitelist = (ConfigurationManager.AppSettings["whitelist"].Length > 0) ?
                ConfigurationManager.AppSettings["whitelist"].Split('|').ToList() :
                new List<string>() { "aeraimangangaingaiegannnanrg" };

            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(url);
                var filters = JsonConvert.DeserializeObject<Filters>(json).filters;

                var all_patterns = filters.Union(userdefined_patterns).Except(whitelist).Except(global_whitelist);

                non_wildcard_patterns = all_patterns.Where(s => !s.Contains("*"));

                wildcard_patterns = all_patterns.Except(non_wildcard_patterns);
            }

            Program.log.Write("Pattern Definitions Refreshed.");
        }

        public void scanFolder(string path, bool recurse = true)
        {
            try
            {
                // scan the root folder of path
                if (infected && stop_on_find) return;

                Console.WriteLine("Scanning " + path);

                // low hanging fruit, find simple patterns that do not contain wildcards
                var files = Directory
                    .EnumerateFiles(path)
                    .Where(file => non_wildcard_patterns.Any(file.Contains))
                    .Where(file => !file_whitelist.Contains(file))
                    .Where(file => !file_whitelist.Any(file.Contains));

                if (files.Count() > 0)
                {
                    infected = true;
                    this.infected_files = files;
                    this.matched_pattern = non_wildcard_patterns.First(files.First().Contains);

                    Program.log.WriteInfection(files, this.matched_pattern);
                    if (stop_on_find) return;
                }

                // deeper scan, scans the folder once per pattern.
                foreach (string pattern in wildcard_patterns)
                {
                    if (infected && stop_on_find) break;

                    Boolean special = this.IsSpecialPattern(pattern);

                    files = Directory
                        .EnumerateFiles(path, pattern)
                        .Where(file => !file_whitelist.Contains(file))
                        .Where(file => !file_whitelist.Any(file.Contains))
                        .Where(file => !special || file.EndsWith(pattern.Replace("*", String.Empty)));

                    if (files.Count() > 0)
                    {
                        infected = true;
                        this.infected_files = files;
                        this.matched_pattern = pattern;

                        Program.log.WriteInfection(files, this.matched_pattern);
                        if (stop_on_find) break;
                    }
                }


                if (infected && stop_on_find) return;


                if (recurse)
                {
                    foreach (string dir in Directory.EnumerateDirectories(path))
                    {
                        if (infected && stop_on_find) break;

                        scanFolder(dir);
                    }
                }

            }

            

            catch (System.IO.PathTooLongException e) // ignore
            { Console.WriteLine(e.Message); }

            catch (System.IO.DirectoryNotFoundException e) // ignore
            { Console.WriteLine(e.Message); }

            catch (System.IO.IOException e) // ignore
            { Console.WriteLine(e.Message); }


            catch (System.UnauthorizedAccessException e) // ignore
            { Console.WriteLine(e.Message); }

            catch (System.InvalidOperationException e) // ignore
            { Console.WriteLine(e.Message); }

            catch(System.ArgumentNullException e) // ignore
            { Console.WriteLine(e.Message); }

            catch (System.Exception e)
            {
                Program.log.Write(String.Format("Problem Scanning Computer {0} \n\r Error {1}", e.GetType().ToString(), e.Message));
                Console.WriteLine(e.Message);
            }
        }

        public void Reset()
        {
            infected = false;
            matched_pattern = null;
            infected_files = null;
        }


        /**
         *    Handle special cases where the pattern 
         *     is of the form *.XXX to avoid matching
         *     files like *.XXXABC
         *    
         */
        private Boolean IsSpecialPattern(string pattern)
        {
            return (pattern.LastIndexOf("*.") == (pattern.Length - 5)) ? true : false;
        }
    }

    public class Filters
    {
        public IEnumerable<string> filters { get; set; }
    }
}
