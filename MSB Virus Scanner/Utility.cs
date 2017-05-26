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
            if ( Program.version != GetCurrentVersion().FileVersion )  // needs updating
            {
                Update();
                return;
            }

            //Program.log.Write("Up To Date");
        }

        public static void Update()
        {
            Program.log.Write(String.Format("Updating To Version {0}",GetCurrentVersion().FileVersion));
            Process.Start( GetProcessPath() );
        }

        public static string GetMsiPath()
        {
            return Program.config["path_to_msi"] ?? @"\\dsjkb\desoft$\MSB_Virus_Sentry\MSB_Virus_Sentry.msi";
        }

        public static string GetExePath()
        {
            return Program.config["path_to_exe"] ?? @"\\dsjkb\desoft$\MSB_Virus_Sentry\bin\MSB Virus Scanner.exe";
        }

        public static string GetProcessPath()
        {
            return String.Format(@"c:\windows\system32\msiexec.exe", @"/i {0} /passive /qn", GetMsiPath());
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
    }
}
