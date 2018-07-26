using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Linq;

namespace MSB_Virus_Scanner
{
    public static class UserHelper
    {
        public static IEnumerable<string> GetSids()
        {
            var sids = new List<string>();
            ManagementObjectCollection users = new ManagementObjectSearcher(@"SELECT * FROM Win32_UserAccount").Get();

            Console.WriteLine("Found {0} Users", users.Count);
            Console.ReadLine();

            foreach(ManagementObject user in users)
            {
                try
                {
                    Console.WriteLine("Username {0}, SID {1}", user["AccountName"], user["SID"]);
                    sids.Add(user["SID"].ToString());

                }

                catch(System.Management.ManagementException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.ReadLine();

            return sids;
        }
    }
}
