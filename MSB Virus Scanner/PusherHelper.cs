using PusherClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSB_Virus_Scanner
{
    class PusherHelper
    {
        static Pusher _pusher = null;
        static Channel _myChannel = null;


        public static void InitPusher()
        {
            _pusher = new Pusher( GetPusherKey() );

            _pusher.ConnectionStateChanged += _pusher_ConnectionStateChanged;
            _pusher.Error += _pusher_Error;

            // Subscribe to the channel
            _myChannel = _pusher.Subscribe("updates");
            _myChannel.Subscribed += _myChannel_Subscribed;

            // Inline binding!
            _myChannel.Bind("pause", (dynamic data) =>
            {
                Utility.PauseUpdates();
            });

            _pusher.Connect();
        }

        static void _pusher_Error(object sender, PusherException error)
        {
            Program.log.Write("Pusher Error: " + error.ToString());
        }

        static void _pusher_ConnectionStateChanged(object sender, ConnectionState state)
        {
            Program.log.Write("Connection state: " + state.ToString());
        }

        static void _myChannel_Subscribed(object sender)
        {
            Program.log.Write("Subscribed to Pusher Channel!");
        }

        static string GetPusherKey()
        {
            return Program.config["pusher_key"];
        }

    }
}
