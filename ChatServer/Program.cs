using ChatLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {

            // Creating a custom formatter for a TcpChannel sink chain.
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;

            // Creating the IDictionary to set the port on the channel instance.
            IDictionary props = new Hashtable();
            props["port"] = 8392;                   // This must match number on client

            // Pass the properties for the port setting and the server provider
            TcpChannel m_tcpChannel = new TcpChannel(props, null, provider);
            ChannelServices.RegisterChannel(m_tcpChannel, false);

            // Create the server object for clients to connect to
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(ClientComms),
                "RemoteServer",
                WellKnownObjectMode.Singleton);

            // Loop here until the uers is done
            Console.WriteLine("Type some text to send to the client, or just press enter to exit");
            string s;
            while ((s = Console.ReadLine()) != "")
                ClientComms.FireNewBroadcastedMessageEvent(s);

            Console.WriteLine("Sooo long and thanks for all the fish.");
        }
    }

}
