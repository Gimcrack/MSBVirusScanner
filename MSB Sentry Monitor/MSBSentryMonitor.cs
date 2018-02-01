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
using System.Management;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace MSB_Sentry_Monitor
{
    partial class MSBSentryMonitor : ServiceBase
    {
        public MSBSentryMonitor()
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


            this.SetupTimers();


            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        public System.Timers.Timer getServiceStatusTimer = new System.Timers.Timer();

        private void SetupTimers()
        {
            // Check for available updates timer    
            getServiceStatusTimer.Interval = 15 * 1000; // 15 sec
            getServiceStatusTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnGetServiceStatusTimer);
            getServiceStatusTimer.Enabled = true;
            getServiceStatusTimer.Start();
        }


        public void OnGetServiceStatusTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            getServiceStatusTimer.Interval = 5 * 60 * 1000; // 5 min
            Program.Serv.CheckService();
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
