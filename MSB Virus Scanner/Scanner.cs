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

        private int _scanned_files_count;
        private int _prev_files_count;
        private bool _send_counts = false;
        private bool _stop = false;

        public IEnumerable<string> infected_files;

        private IEnumerable<string> non_wildcard_patterns;

        private IEnumerable<string> dot_patterns;

        private IEnumerable<string> non_dot_patterns;

        private IEnumerable<string> wildcard_patterns;

        private IEnumerable<string> file_whitelist;

        public Scanner()
        {
            stop_on_find = (ConfigurationManager.AppSettings["action_on_find"] == "stop");

            getPatterns();
        }

        public Scanner(Responder r)
        {
            _responder = r;

            stop_on_find = (ConfigurationManager.AppSettings["action_on_find"] == "stop");

            getPatterns();
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

            _scanned_files_count = 0;
            _prev_files_count = 0;
            _send_counts = true;
            _stop = false;

            ReportCurrent();

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

            ReportCount();
            _send_counts = false;
        }

        public void AdvanceCount(int count)
        {
            _scanned_files_count += count;

            if (_scanned_files_count - _prev_files_count > 1000)
            {
                ReportCurrent();
                _prev_files_count = _scanned_files_count;
            }
        }

        public void ReportCurrent()
        {
            if (_send_counts) 
                Program.dashboard.FilesCurrent(_scanned_files_count);
        }

        public void ReportCount()
        {
            Program.dashboard.FilesCount(_scanned_files_count);
        }

        private void getPatterns()
        {
            var patterns = Program.dashboard.Patterns.Where(pat => { return pat.published_flag; }).Select( pat => { return pat.name; });
            var definitions = Program.dashboard.Definitions.Where( def => { return def.status == "active"; }).Select( def => { return def.pattern; } );
            var exemptions = Program.dashboard.Exemptions.Where(ex => { return ex.published_flag; }).Select( ex => { return ex.pattern; });
            
            var all_patterns = patterns.Union(definitions).Except(exemptions);

                non_wildcard_patterns = all_patterns.Where(s => !s.Contains("*"));

                wildcard_patterns = all_patterns.Except(non_wildcard_patterns);

                file_whitelist = exemptions.Where(s => !s.Contains("*"));

            Program.log.Write("Pattern Definitions Refreshed.");
            Console.WriteLine("Definitions Refreshed");
        }

        public void scanFolder(string path, bool recurse = true)
        {
            try
            {
                // scan the root folder of path
                if (infected && stop_on_find) return;
                if (_stop) return;

                //Console.WriteLine("Scanning " + path);
                Console.WriteLine("Scanned Files {0}", _scanned_files_count.ToString());

                AdvanceCount( Directory.EnumerateFiles(path).Count() );

                // low hanging fruit, find simple patterns that do not contain wildcards
                var files = Directory
                    .EnumerateFiles(path)
                    .Where(file => non_wildcard_patterns.Any( Path.GetFileName(file).Equals ) )
                    .Where(file => !file_whitelist.Contains(file))
                    .Where(file => !file_whitelist.Any(file.Contains));

                if (files.Count() > 0)
                {
                    infected = true;
                    this.infected_files = files;
                    this.matched_pattern = non_wildcard_patterns.First( Path.GetFileName(files.First()).Equals );

                    //Program.log.WriteInfection(files, this.matched_pattern);
                    if (stop_on_find) return;

                    if ( _responder != null )
                    {
                        _responder.respond(this);
                        Reset();
                    }
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
                        .Where(file => !special || file.ToLower().EndsWith(pattern.ToLower().Replace("*", String.Empty)));

                    if (files.Count() > 0)
                    {
                        infected = true;
                        this.infected_files = files;
                        this.matched_pattern = pattern;

                        //Program.log.WriteInfection(files, this.matched_pattern);
                        if (stop_on_find) break;

                        if (_responder != null)
                        {
                            _responder.respond(this);
                            Reset();
                        }
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
