using System;
using System.Threading;

namespace PQueueLib.Extensions
{
    public static class EventArgsExtensions
    {
        public static void Raise<TEventArgs>(this TEventArgs eventArgs, object sender,
            ref EventHandler<TEventArgs> eventHandler) where TEventArgs : System.EventArgs
        {
            eventHandler?.Invoke(sender, eventArgs);
        }
    }
}
