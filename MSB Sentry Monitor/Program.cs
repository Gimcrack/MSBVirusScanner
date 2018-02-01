using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;

namespace MSB_Sentry_Monitor
{
    static class Program
    {
        public static ServiceHelper Serv = new ServiceHelper();

        public static MSBSentryMonitor SM;

        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                SM = new MSBSentryMonitor()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
