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
    public static class System32Helper
    {
        public static IEnumerable<string> GetWeirdFolders()
        {
            var s32 = Directory.EnumerateDirectories(@"C:\Windows\System32")
                .Where(p => !Whitelist.system32_folders.Any(norm => p.EndsWith(norm)));

            var s64 = Directory.EnumerateDirectories(@"C:\Windows\Syswow64")
                .Where(p => !Whitelist.system32_folders.Any(norm => p.EndsWith(norm)));

            return s32.Union(s64);
        }
    }
}
