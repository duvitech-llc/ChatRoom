using ChatLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    class Program
    {
        static void Main(string[] args)
        {

            // Sleep for a moment to let the sewrver start
            System.Threading.Thread.Sleep(2000);

            // Use a defined port to receive callbacks or 0 to receive available port
            TcpChannel m_TcpChan = new TcpChannel(0);
            ChannelServices.RegisterChannel(m_TcpChan, false);

            // Create the object for calling into the server
            ICallsToServer m_RemoteObject = (ICallsToServer)
                Activator.GetObject(typeof(ICallsToServer),
                "tcp://127.0.0.1:8392/RemoteServer");      //  Must match IP and port on server

            // Define sink for events
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(NotifySink),
                "ServerEvents",
                WellKnownObjectMode.Singleton);
            NotifySink sink = new NotifySink();

            // Assign the callback from the server to here
            m_RemoteObject.Notify += new NotifyCallback(sink.FireNotifyCallback);

            // Keep calling till the server is gone (This is not necessary)
            Console.WriteLine("Client is now ready to send and receive data...");
            int n = 1;
            while (true)
            {
                try
                {
                    m_RemoteObject.SomeSimpleFunction(n++);
                    System.Threading.Thread.Sleep(1000);
                }
                catch
                {
                    Console.WriteLine("Server is not responding. Go home.");
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Derived from the wellknown object shared by the two applications. This is used to expose
    /// methods for calling backing into client from the server
    /// </summary>
    class NotifySink : NotifyCallbackSink
    {
        /// <summary>
        /// Events from the server call into here. This is not in the GUI thread.
        /// </summary>
        /// <param name="s">Pass a string for testing</param>
        protected override void OnNotifyCallback(string s)
        {
            Console.WriteLine("Message from the server : {0}", s);
        }
    }
}
