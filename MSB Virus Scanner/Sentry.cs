﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MSB_Virus_Scanner
{
    public class Sentry
    {
        public List<FileSystemWatcher> ListWatchers;

        public List<string> Infections;
        
        public Scanner scanner;

        public void Init()
        {
            Program.log.Write("Initializing Sentry");

            GetScanner();

            ListWatchers = new List<FileSystemWatcher>();

            Infections = new List<string>();

            FileSystemWatcher watcher;

            foreach (DriveInfo drive in Program.GetLocalDrives())
            {
                // make sure the path is valid e.g. A:\
                if (!Directory.Exists(drive.RootDirectory.ToString()))
                {
                    continue;
                }

                watcher = new FileSystemWatcher()
                {
                    Path = drive.RootDirectory.ToString(),
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                    EnableRaisingEvents = true,
                    IncludeSubdirectories = true
                };

                watcher.Created += Listener;

                watcher.Renamed += Listener;

                ListWatchers.Add(watcher);
            }

            Console.WriteLine("Observing file system for changes.");
            Program.log.Write("Sentry In Position Monitoring File System.");

            if (Environment.UserInteractive)
            {
                Console.ReadLine();
            }
        }

        private Scanner GetScanner()
        {
            scanner = new Scanner();
            return scanner;
        }

        private void Listener(object source, FileSystemEventArgs e)
        {
            Listener(e.FullPath);
        }


        private void Listener(object source, RenamedEventArgs e)
        {
            Listener(e.FullPath);
        }

        private void Listener(string path)
        {
            if ( Infections.Contains(path) ) return; // already reported

            //if (Path.GetDirectoryName(path) == Path.GetDirectoryName(Logger.logPath))

            try
            {
                scanner.scanFolder(Path.GetDirectoryName(path), false);

                if (scanner.infected)
                {
                    Program.responder.respond(scanner);

                    Infections.Add(path);
                }
            }

            catch (System.IO.PathTooLongException e) // ignore
            { Console.WriteLine(e.Message); }
        }

        public void CleanUp()
        {
            foreach( var w in ListWatchers )
            {
                w.Dispose();
            }

            ListWatchers.Clear();
        }
    }
}
