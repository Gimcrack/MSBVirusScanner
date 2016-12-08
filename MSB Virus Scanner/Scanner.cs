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

        public List<DriveInfo> localDrives;

        public IEnumerable<string> easy_patterns; // low-hanging fruit

        public IEnumerable<string> wildcard_patterns;

        public IEnumerable<string> patterns;

        public IEnumerable<string> additional_patterns;

        public IEnumerable<string> whitelist;

        public IEnumerable<string> global_whitelist = new List<string>() {
            "*.AFD",
            "*decipher*",
            "*.abc",
            "*_recover_*.*",
            "*+recover+*.*",
            "*cpyt*",
            "*.*locked",
            "locked.bmp",
            "*.xxx",
            "message.txt",
            "*.ttt"
        };

        public Boolean infected;

        public Boolean stop_on_find = (ConfigurationManager.AppSettings["action_on_find"] == "stop");

        public IEnumerable<string> infected_files;

        public void Scan()
        {
            localDrives = getLocalDrives();

            getPatterns();

            additional_patterns = (ConfigurationManager.AppSettings["patterns"].Length > 0) ?
                ConfigurationManager.AppSettings["patterns"].Split('|').ToList() :
                new List<string>() { "aeraimangangaingaiegannnanrg" };

            whitelist = (ConfigurationManager.AppSettings["whitelist"].Length > 0) ?
                ConfigurationManager.AppSettings["whitelist"].Split('|').ToList() :
                new List<string>() { "aeraimangangaingaiegannnanrg" };


            foreach (DriveInfo drive in localDrives)
            {
                if (infected && stop_on_find) break;
                scanFolder(drive.RootDirectory.ToString());

            }

            if (infected)
            {
                Program.log.write("Infection Found" + Environment.NewLine + "Infected Files" + Environment.NewLine + String.Join(Environment.NewLine, infected_files));

                Console.WriteLine("Infected files");
                foreach (string file in infected_files)
                {
                    Console.WriteLine(file);
                }
            }
        }

        private List<DriveInfo> getLocalDrives()
        {
            List<DriveInfo> drives = new List<DriveInfo>();

            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                switch (d.DriveType)
                {
                    case DriveType.Fixed:
                    case DriveType.Removable:
                        drives.Add(d);
                        break;
                }
            }
            return drives;
        }

        private void getPatterns()
        {
            string url = "https://fsrm.experiant.ca/api/v1/combined";

            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(url);
                var filters = JsonConvert.DeserializeObject<Filters>(json).filters;
                var patterns =  this.additional_patterns ?? new List<string>() { "aeraimangangaingaiegannnanrg" };
                var exceptions = this.whitelist ?? new List<string>() { "aeraimangangaingaiegannnanrg" };

                //Program.log.write("Downloaded JSON From Pattern API");
                //Program.log.write(json);

                var all_patterns = filters.Union(patterns).Except(exceptions).Except(global_whitelist);

                easy_patterns = all_patterns.Where(s => !s.Contains("*"));
                wildcard_patterns = all_patterns.Except(easy_patterns);
            }
        }

        public void scanFolder(string path)
        {
            try
            {
                // scan the root folder of path
                if (infected && stop_on_find) return;

                Console.WriteLine("Scanning " + path);
                Program.log.write("Scanning " + path);

                // low hanging fruit, find simple patterns that do not contain wildcards
                var files = Directory
                    .EnumerateFiles(path)
                    .Where(file => easy_patterns.Any(file.Contains));

                if (files.Count() > 0)
                {
                    infected = true;
                    this.infected_files = files;
                    Program.log.write_infection(files);
                    if (stop_on_find) return;
                }

                // deeper scan, scans the folder once per pattern.
                foreach( string pattern in wildcard_patterns )
                {
                    if (infected && stop_on_find) break;

                    files = Directory
                        .EnumerateFiles(path, pattern);

                    if (files.Count() > 0)
                    {
                        infected = true;
                        this.infected_files = files;
                        Program.log.write_infection(files);
                        if (stop_on_find) break;
                    }
                }


                if (infected && stop_on_find) return;


                foreach (string dir in Directory.EnumerateDirectories(path))
                {
                    if (infected && stop_on_find) break;

                    scanFolder(dir);
                }

            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public class Filters
    {
        public IEnumerable<string> filters { get; set; }
    }
}
