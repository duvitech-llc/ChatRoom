using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLib
{

    /// <summary>
    /// Delegate defines the metho call fromthe server to the client
    /// </summary>
    /// <param name="s">Pass a string for testing</param>
    public delegate void NotifyCallback(string s);

    /// <summary>
    /// Defines server interface which will be deployed on every client
    /// </summary>
    public interface ICallsToServer
    {
        /// <summary>
        /// Function to call the server from the client
        /// </summary>
        /// <param name="n">Some number</param>
        /// <returns>Some interesting text</returns>
        string SomeSimpleFunction(int n);

        /// <summary>
        /// Add or remove callback destinations on the client
        /// </summary>
        event NotifyCallback Notify;
    }

    /// <summary>
    /// This class is used by client to provide delegates to the server that will
    /// fire events back through these delegates. Overriding OnServerEvent to capture
    /// the callback from the server
    /// </summary>
    public abstract class NotifyCallbackSink : MarshalByRefObject
    {
        /// <summary>
        /// Called by the server to fire the call back to the client
        /// </summary>
        /// <param name="s">Pass a string for testing</param>
        public void FireNotifyCallback(string s)
        {
            Console.WriteLine("Activating callback");
            OnNotifyCallback(s);
        }

        /// <summary>
        /// Client overrides this method to receive the callback events from the server
        /// </summary>
        /// <param name="s">Pass a string for testing</param>
        protected abstract void OnNotifyCallback(string s);
    }
}
