using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.IO;

namespace MSB_Virus_Scanner
{
    public static class ProfileHelper
    {
        public static IEnumerable<string> GetWeirdFolders(string profile)
        {
            return Directory.EnumerateDirectories(profile)
                .Where(p => ! p.Contains(@"."))
                .Where(p => ! Whitelist.profile_folders.Any(norm => p.EndsWith(norm)));
        }
    }
}
