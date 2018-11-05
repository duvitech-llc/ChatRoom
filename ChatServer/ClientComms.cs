using ChatLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ClientComms : MarshalByRefObject, ICallsToServer
    {
        /// <summary>
        /// Function to call the server from the client. This is not in the GUI thread.
        /// </summary>
        /// <param name="n">Some number</param>
        /// <returns>Some interesting text</returns>
        public string SomeSimpleFunction(int n)
        {
            Console.Write("  Client sent : {0}      \r", n);
            return "Server says : " + n.ToString();
        }

        /// <summary>
        /// Local copy of event holding a collection
        /// </summary>
        private static event NotifyCallback s_notify;

        /// <summary>
        /// Add or remove callback destinations on the client
        /// </summary>
        public event NotifyCallback Notify
        {
            add { s_notify = value; }
            remove { Console.WriteLine("TODO : Notify remove."); }
        }

        /// <summary>
        /// Call this method to send the string to the client. This call will throw an exception
        /// if the client has gone or network is down
        /// </summary>
        /// <param name="s">Some test to send to client</param>
        public static void FireNewBroadcastedMessageEvent(string s)
        {
            Console.WriteLine("Broadcasting... Sending : {0}", s);
            s_notify(s);
        }
    }
}
