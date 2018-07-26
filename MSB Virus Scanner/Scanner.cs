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

        private Responder _responder;

        public string matched_pattern;

        private bool _stop = false;

        public IEnumerable<string> infected_files;

        public Scanner()
        {
            stop_on_find = (ConfigurationManager.AppSettings["action_on_find"] == "stop");
        }

        public Scanner(Responder r)
        {
            _responder = r;

            stop_on_find = (ConfigurationManager.AppSettings["action_on_find"] == "stop");
        }

        public Scanner DontStopOnFind()
        {
            stop_on_find = false;

            return this;
        }

        public void Stop()
        {
            _stop = true;
        }

        public void Scan()
        {
            _stop = false;

            Program.log.Write("Scanning Registry");
            // look for weird 'Run' entries in the Registry
            ScanRegistry();
            

            Program.log.Write("Scanning Scheduled Tasks");
            // look for weird scheduled tasks
            ScanScheduledTasks();
            


            Program.log.Write("Scanning User Profiles");
            // scan users Appdata Folder and subfolders for weird files
            ScanProfiles();
            


            Program.log.Write("Scanning Startup Items");
            // look for weird executables in all users startup
            ScanStartup();
            


            Program.log.Write("Scanning System32");
            // look for weird executables in System32
            ScanSystem32();
            


            Program.log.Write("Reporting Findings");
            // report on findings
            Respond();


            Program.log.Write("Done Scanning");
       }

        private void Respond()
        {
            if (Program.findings.HasFindings())
            {
                Program.log.Write(" --==|| Infected || ==-- ");

                infected = true;

                _responder.respond();
            }

            else
            {
                CleanBillOfHealth();
            }
        }

        private void CleanBillOfHealth()
        {
            _responder.respond_clean();
        }

        private void ScanRegistry()
        {
            var values = RegistryHelper.GetRunValues();
            if ( values.Count() > 0 )
                AddFinding(values, "Registry Run Value");
        }

        private void ScanScheduledTasks()
        {
            var weird_tasks = ScheduledTaskHelper.GetWeirdTasks();
            if ( weird_tasks.Count() > 0 )
                AddFinding(weird_tasks, "Weird Task");
        }

        private void ScanStartup()
        {
            ScanPathForExtensions(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp", new string[] { ".exe" });
        }

        private void ScanSystem32()
        {
            foreach( string weird_folder in System32Helper.GetWeirdFolders() )
            {
                ScanPathForExtensions(weird_folder, new string[] { ".exe" });
            }
        }

        private void ScanProfiles()
        {
            foreach (var path in Directory.EnumerateDirectories(@"C:\Users"))
            {
                Program.log.Write( String.Format("Scanning Profile {0}", path) );
                ScanProfile(path);
                
            }
        }

        private bool CheckFolderAccess(string folderPath)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }


        private void ScanProfile(string profile)
        {
            if ( profile.EndsWith("Default User") || profile.EndsWith("All Users") || profile.EndsWith("Public") || profile.EndsWith("jb30111") )
                return;

            foreach ( string subpath in ProfileHelper.GetWeirdFolders(profile) )
            {
                ScanForWeirdExe(subpath);
            }

            ScanPathForExtensions(Path.Combine(profile, "AppData", "Local", "Temp"), new string[] { ".cmd"} );

            ScanPathForExtensions(Path.Combine(profile, "AppData", "Roaming"), new string[] { ".exe" });
        }

        private void ScanPathForExtensions(string path, string[] extensions, bool recurse = true)
        {
            
            var files = GetRecentFiles(path, recurse)
            .Where(x => extensions.Any(ext => ext == Path.GetExtension(x)));


            if (files.Count() > 0)
                AddFinding(files, Path.GetExtension(files.First()));

            if (recurse)
            {
                try
                {
                    var folders = Directory.EnumerateDirectories(path);

                    foreach (string dir in folders)
                    {
                        ScanPathForExtensions(dir, extensions);
                    }
                }
                catch (Exception)
                { }
                
            }

        }

        public void ScanForWeirdExe(string path, bool recurse = true)
        {
            var files = GetRecentFiles(path, recurse)
                    .Where(p => Path.GetExtension(p) == ".exe")
                    .Where(p => Path.GetFileNameWithoutExtension(p).Length < 12 && Path.GetFileNameWithoutExtension(p).Length > 5);

            if ( files.Count() > 0 )
                AddFinding(files, String.Format("Weird exe in {0}",path));

            if (recurse)
            {
                try
                {
                    var folders = Directory.EnumerateDirectories(path);
                    foreach (string dir in folders)
                    {
                        ScanForWeirdExe(dir);
                    }
                }
                catch (Exception)
                { }
            }
        }


        private void AddFinding( IEnumerable<string> files, string pattern )
        {
            Program.findings.Add(files, pattern);
        }

        private void AddFinding( IEnumerable<Finding> findings, string pattern)
        {
            Program.findings.Add(findings, pattern);
        }

        private IEnumerable<string> GetRecentFiles(string path, bool recurse = true)
        {
            try
            {
                Console.WriteLine(String.Format("Scanning Path {0}", path));
                Program.log.Write( String.Format("Scanning Path {0}", path) );

                return Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly)
                    .Where(x => !Whitelist.files.Any(pattern => x.Contains(pattern)))
                    .Where(x => x.Length <= 260)
                    .Where(x => File.GetCreationTime(x) > DateTime.Today.AddDays(-14)); 
            }


            catch (System.IO.PathTooLongException e) // ignore
            {  }

            catch (System.IO.DirectoryNotFoundException e) // ignore
            {  }

            catch (System.IO.IOException e) // ignore
            {  }

            catch (System.UnauthorizedAccessException e) // ignore
            {  }

            catch (System.InvalidOperationException e) // ignore
            {  }

            catch (System.ArgumentNullException e) // ignore
            {  }

            catch (System.Exception e)
            {
                Program.log.Write(String.Format("Problem Scanning Computer {0} \n\r Error {1}", e.GetType().ToString(), e.Message));
                Console.WriteLine(e.Message);
            }

            return new List<string>();
        }

        public void Reset()
        {
            infected = false;
            matched_pattern = null;
            infected_files = null;
        }
    }

    public class Filters
    {
        public IEnumerable<string> filters { get; set; }
    }
}
