using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQueueLib.Tests
{
    internal static class PQueueSetHelper
    {
        public static PQueue<T> CreatePQueueObject<T>() => new();
        public static PQueue<T> CreatePQueueObject<T>(int capacity) => new(capacity);

        public static PQueue<T> CreatePQueueObjectWithElements<T>(params T[] elements)
        {
            var pQueue = CreatePQueueObject<T>();
            foreach (var elem in elements)
            {
                pQueue.Enqueue(elem);
            }

            return pQueue;
        }
    }
}
