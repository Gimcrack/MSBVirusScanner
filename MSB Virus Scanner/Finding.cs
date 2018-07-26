using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace MSB_Virus_Scanner
{
    public class Finding
    {
        public string pattern { get; set; }
        public string path { get; set; }
        public finding_type type { get; set; }

        public string registry_key { get; set; }

        public string registry_value { get; set; }

        public registry_type reg_type { get; set; }

        public string status { get; set; } = "Cleanup Not Attempted";

        public bool cleaned { get; set; } = false;

        public enum finding_type
        {
            File,
            Registry
        };

        public enum registry_type
        {
            Users,
            LocalMachine
        };

        public void Clean()
        {
            if ( this.type == finding_type.File )
            {

                if ( File.Exists(path) )
                {
                    File.Delete(path);

                    if ( ! File.Exists(path))
                    {
                        status = "File deleted";
                        cleaned = true;
                    } 
                    else
                    {
                        StreamWriter sw = new StreamWriter(path, false);
                        Process.Start("erase", path);

                        if (!File.Exists(path))
                        {
                            status = "File deleted";
                            cleaned = true;
                        }
                        else
                        {
                            status = "Could not delete file";
                            cleaned = false;
                        }
                    }

                }
                else
                {
                    status = "File does not exist.";
                    cleaned = true;
                }
                    

            }

            if ( this.type == finding_type.Registry )
            {
                if ( this.reg_type == registry_type.Users )
                {
                    var reg = Registry.Users.OpenSubKey(registry_key, true);
                    reg.DeleteValue(registry_value);

                    status = (string) reg.GetValue(registry_value, "deleted");
                }

                if( this.reg_type == registry_type.LocalMachine )
                {
                    var hklm = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                    hklm.DeleteValue(registry_value);

                    status = (string)hklm.GetValue(registry_value, "deleted");
                }


                if (status == "deleted")
                {
                    cleaned = true;
                    status = "Registry value deleted";
                }
                else
                {
                    cleaned = false;
                    status = "Could not delete registry value";
                }
            }
        }

        public string GetFormatted()
        {
            if ( this.type == finding_type.File )
                return String.Format("{0} {1} -- Pattern: {2}{1} -- Cleaned : {3}{1} -- Status : {4}{1}", this.path, Environment.NewLine, this.pattern, this.cleaned, this.status);

            else if (this.reg_type == registry_type.Users)
            {
                return String.Format(@"HKU\{5}:{0} {1} -- Pattern: {2}{1} -- Cleaned : {3}{1} -- Status : {4}{1}", this.path, Environment.NewLine, this.pattern, this.cleaned, this.status, this.registry_key);
            }

            //else
            return String.Format(@"HKLM\Run:{0} {1} -- Pattern: {2}{1} -- Cleaned : {3}{1} -- Status : {4}{1}", this.path, Environment.NewLine, this.pattern, this.cleaned, this.status);
        }
    }
}
