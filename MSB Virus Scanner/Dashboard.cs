using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using ServiceStack.Text;
using System.Management;
using System.IO;


namespace MSB_Virus_Scanner
{
    class Dashboard
    {
        public Models.Pattern[] Patterns;
        public Models.Definition[] Definitions;
        public Models.Exemption[] Exemptions;
        public Timer FetchTimer;

        public Dashboard()
        {
            FetchTimer = new Timer()
            {
                Interval = 5000 + new Random().Next(0, 10000),
                Enabled = false,
                AutoReset = false
            };

            FetchTimer.Elapsed += PerformFetch;
        }

        public async void Register()
        {
            var payload = new Api.Payload();
            payload.Add("version", Program.version);
            payload.Add("os", os());

            await Program.api.post(
                Url(@"clients/{0}"), 
                payload.Data()
            );
        }

        public void Heartbeat()
        {
            Program.log.Write("Attempting to send heartbeat");

            Program.api.get(
                Url(@"clients/{0}/heartbeat")
            );
        }

        public async void PasswordResetComplete()
        {
            await Program.api.post(
                Url(@"clients/{0}/admin-password-reset-complete")
            );
        }

        public async void FilesCount(int count)
        {
            await Program.api.post(
                Url(@"clients/{0}/count"),
                new Api.Payload("count", count.ToString()).Data()
            );
        }


        public async void FilesCurrent(int count)
        {
            await Program.api.post(
                Url(@"clients/{0}/count_current"),
                new Api.Payload("count", count.ToString()).Data()
            );
        }

        public async void Log(string action, string status)
        {
            var payload = new Api.Payload();
            payload.Add("action", action);
            payload.Add("status", status);

            await Program.api.post(
                Url(@"clients/{0}/logs"), 
                payload.Data()
            );
        }

        public async void MatchedFile(string file, string pattern)
        {
            if ( file.Trim().Length < 2 || pattern.Trim().Length < 2 )
            {
                // do nothing, not a real file pattern combination
                return;
            }

            var f = new FileInfo(file);

            var payload = new Api.Payload();
            payload.Add("file", file);
            payload.Add("pattern", pattern);
            payload.Add("file_created_at", f.CreationTime.ToString("yyyy-MM-dd HH:mm:ss.000"));
            payload.Add("file_modified_at", f.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.000"));


            await Program.api.post(
                Url(@"clients/{0}/matches"),
                payload.Data()
            );
        }

        public string Url(string stub)
        {
            return String.Format(stub, Environment.MachineName.ToLower());
        }

        public void Fetch()
        {
            int interval = 5000 + new Random().Next(0, 10000);

            Console.WriteLine("Fetching definitions in {0} ms", interval.ToString());

            FetchTimer.Interval = interval;
            FetchTimer.Enabled = true;
            FetchTimer.Stop();
            FetchTimer.Start();
        }

        public async void PerformFetch(object sender, ElapsedEventArgs e)
        {
            await GetDefinitions();
            await GetExemptions();
            await GetPatterns();

            // reset the service
            if (Program.sentry != null)
            {
                Console.WriteLine("Restarting Sentry");
                Program.sentry.CleanUp();

                Program.sentry = new Sentry();
                Program.sentry.Init();
            }


            // reset the scanner if it's running
            if (Program.scanner != null)
            {
                Console.WriteLine("Restarting Scanner");
                Program.scanner = new Scanner();
            }
        }

        public async Task GetDefinitions()
        {
            Definitions = await Program.api.get<Models.Definition[]>("definitions");
            Console.WriteLine("Got the definitions");
        }

        public async Task GetExemptions()
        {
            Exemptions = await Program.api.get<Models.Exemption[]>("exemptions");
            Console.WriteLine("Got the exemptions");
        }

        public async Task GetPatterns()
        {
            Patterns = await Program.api.get<Models.Pattern[]>("patterns");
            Console.WriteLine("Got the patterns");
        }

        private string os()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";
        }
    }
}
