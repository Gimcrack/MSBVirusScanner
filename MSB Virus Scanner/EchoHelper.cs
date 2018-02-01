using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Reactive;
using ServiceStack.Text;
//using Newtonsoft.Json;

namespace MSB_Virus_Scanner
{
    public class EchoHelper
    {
        private Listeners _events;

        protected static Dictionary<String, Dictionary<string, Action<string>>> _catchall = new Dictionary<string, Dictionary<string, Action<string>>>
        {
            { "catchall",
                new Dictionary<string, Action<string>>{
                    { "catchall", Listeners.DoNothing },
                }
            }
        }; 

        protected static Dictionary<String, Dictionary<String, Action<string>>> _listeners = 
            new Dictionary<string, Dictionary<string, Action<string>>>
        {   
            // Channel
            { "clients",

                // Client Events
                new Dictionary<string, Action<string>>{
                    { "NewBuild", Listeners.UpgradeClient },
                }
            },

            // Channel
            { "clients.{0}", 
                
                // Client Events
                new Dictionary<string, Action<string>>{
                    //{ "ClientWasUpdated", Listeners.Clients.ClientWasUpdated },
                    { "ClientShouldUpgrade", Listeners.UpgradeClient },
                    { "ClientShouldScan", Listeners.ScanClient },
                    { "ClientShouldSendHeartbeat", Listeners.SendHeartbeat },
                    { "ClientPasswordResetWasRequested", Listeners.ResetPassword }
                }
            },  
            
            // Channel
            { "exemptions",

                // Exemption Events
                new Dictionary<string, Action<string>>{
                    { "ExemptionWasCreated", Listeners.FetchNewDefinitions },
                    { "ExemptionWasUpdated", Listeners.FetchNewDefinitions },
                    { "ExemptionWasDestroyed", Listeners.FetchNewDefinitions },
                }
            },   
            
            // Channel
            { "definitions",

                // Exemption Events
                new Dictionary<string, Action<string>>{
                    { "DefinitionsWereUpdated", Listeners.FetchNewDefinitions },
                }
            },   
            
            // Channel
            { "patterns",

                // Exemption Events
                new Dictionary<string, Action<string>>{
                    { "PatternWasCreated", Listeners.FetchNewDefinitions },
                    { "PatternWasUpdated", Listeners.FetchNewDefinitions },
                    { "PatternWasDestroyed", Listeners.FetchNewDefinitions },
                }
            }    
            
            // Channel
            //{ "clients.{0}.matches",

            //    // MatchedFile Events
            //    new Dictionary<string, Action<string>>{
            //        { "MatchedFileWasCreated", Listeners.Matches.MatchedFileWasCreated },
            //        { "MatchedFileWasUpdated", Listeners.Matches.MatchedFileWasUpdated },
            //        { "MatchedFileWasMuted", Listeners.Matches.MatchedFileWasMuted },
            //        { "MatchedFileWasUnmuted", Listeners.Matches.MatchedFileWasUnmuted },
            //    }
            //}, 
        };

        public void init()
        {
            _events = new Listeners();
        }

        class Listeners
        {
            public Listeners()
            {
                foreach( var chan in _listeners )
                {
                   Program.redis.Subscriber().Subscribe( FormatChannel(chan.Key), Route);
                }
            }

            public void Route(RedisChannel channel, RedisValue msg)
            {
                try
                {
                    // get an object from the payload message
                    var payload = JsonSerializer.DeserializeFromString<JsonObject>(msg.ToString());

                    // get the friendly name of the event (without the namespace)
                    string evnt = payload["event"].Split('\\').Last();

                    // get the action corresponding to the event
                    var action = _listeners
                        
                        .Where(chan => { return ParseChannel(chan.Key) == channel.ToString(); })
                        .DefaultIfEmpty(_catchall.First()) // get the channel
                        .First()

                    .Value
                        .Where(ev => { return ev.Key == evnt; })
                        .DefaultIfEmpty( _catchall.First().Value.First() )
                        .First()
                        .Value; // get the listener



                    // trigger the action and pass in the payload
                    action(msg);
                }

                catch( Exception e )
                {
                    Console.WriteLine(e.Message);
                }
                
            }

            public static string ParseChannel(string stub)
            {
                return String.Format(stub, Environment.MachineName.ToLower());
            }

            public static RedisChannel FormatChannel(string stub)
            {
                return new RedisChannel( ParseChannel(stub) , RedisChannel.PatternMode.Literal);
            }

            // Event Listeners
            public static void FetchNewDefinitions(string msg)
            {
                Program.dashboard.Fetch();
            }

            public static void UpgradeClient(string msg)
            {
                Console.WriteLine("Client Updating");
                if ( Program.scanner != null )
                {
                    Program.scanner.Stop();
                    Thread.Sleep(5000);
                }
                Utility.KillAndUpdate();
            }

            public static void ScanClient(string msg)
            {
                Console.WriteLine("Client Scanning");

                Program.ScanAll();
            }

            public static void SendHeartbeat(string msg)
            {
                Program.dashboard.Heartbeat();
            }

            public static void ResetPassword(string msg)
            {
                ClientNewPasswordEvent evnt = JsonSerializer.DeserializeFromString<ClientNewPasswordEvent>(msg.ToString());
                Console.WriteLine("Resetting Password to {0}",evnt.data.password);
                Utility.ResetPassword(evnt.data.password);

                Program.dashboard.PasswordResetComplete();
            }

            public static void DoNothing(string msg)
            { }
        }


        public abstract class Event
        {
            private string _event;
            public string @event
            {
                get
                {
                    return _event;
                }
                set
                {
                    _event = value.Replace(@"\", ".").Split('.').Last();
                }
            }
        }

        public class ClientEvent : Event
        {
            public Data data { get; set; }

            public class Data
            {
                public Models.Client client { get; set; }
            }
        }

        public class ClientNewPasswordEvent : Event
        {
            public Data data { get; set; }

            public class Data
            {
                public Models.Client client { get; set; }
                public string password { get; set; }
            }
        }

        public class MatchedFileEvent : Event
        {
            public Data data { get; set; }

            public class Data
            {
                public Models.MatchedFile matched_file { get; set; }
            }
        }

        public class PatternEvent : Event
        {
            public Data data { get; set; }

            public class Data
            {
                public Models.Pattern pattern { get; set; }
            }
        }

        public class ExemptionEvent : Event
        {
            public Data data { get; set; }

            public class Data
            {
                public Models.Exemption exemption { get; set; }
            }
        }

        public class DefinitionsEvent : Event
        {

        }
    }
}
