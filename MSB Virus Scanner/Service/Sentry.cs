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


            

            //Program.log.Write("Sentry Service Started.");

            
            // Setup Timers
            Timer GuardTimer = new Timer()
            {
                Interval = 30 * 1000, // wait 30 sec to start 
                Enabled = true,
            };

            Timer HeartbeatTimer = new Timer()
            {
                Interval = 10 * 60 * 1000,  // send a heartbeat every 10 min
                Enabled = true
            };


            // Timer Handlers
            GuardTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                GuardTimer.Interval = 24 * 60 * 60 * 1000; // renew once per day
                Guard();
            };

            HeartbeatTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                Program.dashboard.Heartbeat();
            };

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }

        protected override void OnStop()
        {
            if (Program.sentry != null)
            {
                Program.sentry.CleanUp();
            }
        }

        public void Guard()
        {
            if (Program.sentry != null)
            {
                Program.sentry.CleanUp();
            }

            Program.sentry = new MSB_Virus_Scanner.Sentry();
            Program.sentry.Init();
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
            public uint dwServiceType;
            public ServiceState dwCurrentState;
            public uint dwControlsAccepted;
            public uint dwWin32ExitCode;
            public uint dwServiceSpecificExitCode;
            public uint dwCheckPoint;
            public uint dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
    }
}
