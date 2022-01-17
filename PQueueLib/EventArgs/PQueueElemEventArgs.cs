using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQueueLib.EventArgs
{
    public class PQueueElemEventArgs<T>: PQueueEventArgs
    {
        public T Element { get; }
        public PQueueElemEventArgs(string message, T element) : base(message)
        {
            Element = element;
        }
    }
}
