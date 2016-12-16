using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSB_Virus_Scanner
{
    public static class Model
    {
        public static void LogEntry( string message, string action = "", string result = "" )
        {
            Database.Statement("INSERT INTO [dbo].[log] ( [computername],[action],[result],[comments] ) VALUES (@computername,@action,@result,@comments)",
                new Dictionary<string, string>()
                {
                    {"@computername",Environment.MachineName},
                    {"@action",action},
                    {"@result",result},
                    {"@comments",message},
                }
            );
        }
    }
}
