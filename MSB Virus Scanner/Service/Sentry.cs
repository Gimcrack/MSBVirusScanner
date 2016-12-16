using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Timers;

namespace MSB_Virus_Scanner.Service
{
    partial class MSB_Virus_Sentry : ServiceBase
    {
        private MSB_Virus_Scanner.Sentry sentry;

        public MSB_Virus_Sentry()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);


            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            Program.log.Write("Sentry Service Started.");

            
            // Setup Timers
            Timer GuardTimer = new Timer()
            {
                Interval = 24 * 60 * 60 * 1000, // renew once per day
                Enabled = true,
            };


            Timer ScanTimer = new Timer()
            {
                Interval = 2 * 24 * 60 * 60 * 1000, // scan once per 2 days
                Enabled = true,
            };


            // Timer Handlers
            GuardTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                Guard();
            };

            ScanTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                Program.Scan();
            };


            Guard();
            
        }

        protected override void OnStop()
        {
            sentry.CleanUp();
        }

        private void Guard()
        {
            if (sentry != null)
            {
                sentry.CleanUp();
            }
            sentry = new MSB_Virus_Scanner.Sentry();
        }


        /**
         *  Boilerplate below here
         */

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwServiceType;
            public ServiceState dwCurrentState;
            public long dwControlsAccepted;
            public long dwWin32ExitCode;
            public long dwServiceSpecificExitCode;
            public long dwCheckPoint;
            public long dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
    }
}
