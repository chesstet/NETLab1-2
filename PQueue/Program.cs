using System;
using System.Collections.Generic;
using System.Linq;
using PQueueLib;
using PQueueLib.EventArgs;
using Console = System.Console;

namespace PQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            PQueue<int> pQueue = new PQueue<int>();
            pQueue.Cleared += ClearQueueHandler;
            pQueue.Added += AddItemHandler;
            pQueue.Removed += RemoveItemQueueHandler;
            pQueue.Enqueue(1);
            pQueue.Enqueue(3);
            pQueue.Enqueue(4);
            pQueue.Enqueue(363);
            pQueue.Enqueue(-2);
            pQueue.Enqueue(3456364);
            pQueue.Enqueue(-3);
            pQueue.Dequeue();
            pQueue.Enqueue(3456364);
            pQueue.Enqueue(3456364);
            pQueue.Enqueue(3456364);
            pQueue.Dequeue();
            pQueue.Dequeue();
            pQueue.Enqueue(-3);
            pQueue.Enqueue(1);
            pQueue.Enqueue(2);
            pQueue.Enqueue(3);

            pQueue.Enqueue(1);
            pQueue.Enqueue(1);
            pQueue.Clear();
            pQueue.ToList().ForEach(Console.WriteLine);
        }

        private static void ClearQueueHandler(object sender, PQueueEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.Message);
        }

        private static void RemoveItemQueueHandler(object sender, PQueueEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.Message);
        }

        private static void AddItemHandler<T>(object sender, PQueueElemEventArgs<T> eventArgs)
        {
            Console.WriteLine($"{eventArgs.Message}. Element: {eventArgs.Element}");
        }
    }
}
