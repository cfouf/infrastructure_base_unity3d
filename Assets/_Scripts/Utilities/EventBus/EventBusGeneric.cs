using System;
using System.Collections.Generic;

namespace _Scripts.Utilities.EventBus
{
    public interface IEvent
    {
    }

    public interface IEventReceiverBase
    {
    }

    public interface IEventReceiver<in T> : IEventReceiverBase where T : struct, IEvent
    {
        void OnEvent(T e);
    }

    public static class EventBus<T> where T : struct, IEvent
    {
        private static IEventReceiver<T>[] buffer;
        private static int count;
        private const int blockSize = 256;

        private static readonly HashSet<IEventReceiver<T>> Hash;

        static EventBus()
        {
            Hash = new HashSet<IEventReceiver<T>>();
            buffer = Array.Empty<IEventReceiver<T>>();
        }

        public static void Register(IEventReceiverBase handler)
        {
            count++;
            Hash.Add(handler as IEventReceiver<T>);
            if (buffer.Length < count)
                buffer = new IEventReceiver<T>[count + blockSize];


            Hash.CopyTo(buffer);
        }

        public static void UnRegister(IEventReceiverBase handler)
        {
            Hash.Remove(handler as IEventReceiver<T>);
            Hash.CopyTo(buffer);
            count--;
        }

        public static void Raise(T e = default)
        {
            for (var i = 0; i < count; i++)
                buffer[i].OnEvent(e);
        }

        public static void RaiseAsInterface(IEvent e) => Raise((T) e);

        public static void Clear() => Hash.Clear();
    }
}