using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;

namespace MSB_Virus_Scanner
{
    public static class RegistryHelper
    {
        private static string _path = Path.Combine(@"C:\temp\MSB_Virus_Sentry\Results\", Environment.MachineName.ToString() + "_registry_run.log");


        public static IEnumerable<Finding> GetRunValues()
        {
            var run_values = new List<Finding>();

            // look at the Run settings for each user profile
            foreach (string user in GetSids())
            {
                string reg_key = String.Format(@"{0}\Software\Microsoft\Windows\CurrentVersion\Run", user);
                var key = Registry.Users.OpenSubKey(reg_key,true);

                if (key == null)
                    continue;

                foreach( string name in key.GetValueNames())
                {
                    var value = key.GetValue(name).ToString();

                    if (value.Trim().Length == 0)
                        continue;

                    if ( ! Whitelist.registry_run.Any( v => value.Contains(v) ) )
                        run_values.Add( new Finding() { type = Finding.finding_type.Registry, reg_type = Finding.registry_type.Users, path = value, registry_key = reg_key, registry_value = name });
                }
            }

            // look at the run settings for the local machine
            var hklm = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            foreach (string name in hklm.GetValueNames())
            {
                var value = hklm.GetValue(name).ToString();

                if (!Whitelist.registry_run.Any(v => value.Contains(v)))
                    run_values.Add( new Finding() { type = Finding.finding_type.Registry, reg_type = Finding.registry_type.LocalMachine, path = value, registry_value = name });
            }

            return run_values;
        }

        public static IEnumerable<string> GetSids()
        {
            var key = Registry.Users;

            return key.GetSubKeyNames();
        }
    }
}
