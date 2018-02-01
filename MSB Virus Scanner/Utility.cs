using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace MSB_Virus_Scanner
{
    class Utility
    {

        public static bool UpdatesPaused = false;

        public static FileVersionInfo GetCurrentVersion()
        {
            return FileVersionInfo.GetVersionInfo(GetExePath());
        }

        public static void CheckForUpdates()
        {
            if (UpdatesPaused) return;

            //Program.log.Write("Checking For Updates");
            Console.WriteLine("Checking For Updates");
            if ( Program.version != GetCurrentVersion().FileVersion )  // needs updating
            {
                Console.WriteLine("Updating...");
                Update();
                return;
            }

            Console.WriteLine("Up To Date");
            //Program.log.Write("Up To Date");

        }

        public static void KillAndUpdate()
        {
            Program.log.Write(String.Format("Updating To Version {0}", GetCurrentVersion().FileVersion));
            Process.Start( GetBatchPath() );
        }

        public static void Update()
        {
            Program.log.Write(String.Format("Updating To Version {0}",GetCurrentVersion().FileVersion));
            Process.Start( GetProcessPath(), GetProcessParams() );
        }

        public static string GetMsiPath()
        {
            return Program.config["path_to_msi"] ?? @"\\dsjkb\desoft$\MSB_Virus_Sentry\MSB_Virus_Sentry.msi";
        }

        public static string GetBatchPath()
        {
            return Program.config["path_to_batch"] ?? @"\\dsjkb\desoft$\MSB_Virus_Sentry\kill.bat";
        }

        public static string GetExePath()
        {
            return Program.config["path_to_exe"] ?? @"\\dsjkb\desoft$\MSB_Virus_Sentry\bin\MSB Virus Scanner.exe";
        }

        public static string GetProcessPath()
        {
            return String.Format(@"c:\windows\system32\msiexec.exe");
        }

        public static string GetProcessParams()
        {
            return String.Format(@"/i {0} /passive /qn", GetMsiPath());
        }

        public static void PauseUpdates()
        {
            Program.log.Write("Pausing Updates");
            UpdatesPaused = true;
        }

        public static void ResumeUpdates()
        {
            Program.log.Write("Resuming Updates");
            UpdatesPaused = false;
        }

        public static void ResetPassword(string newpass)
        {
            var p = new Process();
            p.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            p.StartInfo.Arguments = String.Format(@"/c net user administrator {0}", newpass);
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;

            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            Program.log.Write( String.Format("Resetting Password Result: {0}, Error {1}", output, error) );
            p.WaitForExit();
        }
    }
}
