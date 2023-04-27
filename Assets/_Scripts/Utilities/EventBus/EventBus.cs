using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _Scripts.Utilities.EventBus
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, ClassMap> ClassRegisterMap = new Dictionary<Type, ClassMap>();
        private static readonly Dictionary<Type, Action<IEvent>> CachedRaise = new Dictionary<Type, Action<IEvent>>();

        private class BusMap
        {
            public Action<IEventReceiverBase> Register;
            public Action<IEventReceiverBase> Unregister;
        }

        private class ClassMap
        {
            public BusMap[] Buses;
        }

        public static void Initialize()
        {
        }

        static EventBus()
        {
            var busRegisterMap = new Dictionary<Type, BusMap>();

            var delegateType = typeof(Action<>);
            var delegateGenericRegister = delegateType.MakeGenericType(typeof(IEventReceiverBase));
            var delegateGenericRaise = delegateType.MakeGenericType(typeof(IEvent));

            var types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var t in types)
            {
                if (t == typeof(IEvent) || !typeof(IEvent).IsAssignableFrom(t)) continue;
                var eventHubType = typeof(EventBus<>);
                var genMyClass = eventHubType.MakeGenericType(t);
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(genMyClass.TypeHandle);

                var busMap = new BusMap()
                {
                    Register =
                        Delegate.CreateDelegate(delegateGenericRegister, genMyClass.GetMethod("Register")) as
                            Action<IEventReceiverBase>,
                    Unregister =
                        Delegate.CreateDelegate(delegateGenericRegister, genMyClass.GetMethod("UnRegister")) as
                            Action<IEventReceiverBase>
                };

                busRegisterMap.Add(t, busMap);

                var method = genMyClass.GetMethod("RaiseAsInterface");
                CachedRaise.Add(t, (Action<IEvent>) Delegate.CreateDelegate(delegateGenericRaise, method));
            }

            foreach (var t in types)
            {
                if (!typeof(IEventReceiverBase).IsAssignableFrom(t) || t.IsInterface) continue;
                var interfaces = t.GetInterfaces().Where(x =>
                    x != typeof(IEventReceiverBase) && typeof(IEventReceiverBase).IsAssignableFrom(x)).ToArray();

                var map = new ClassMap()
                {
                    Buses = new BusMap[interfaces.Length]
                };

                for (var i = 0; i < interfaces.Length; i++)
                {
                    var arg = interfaces[i].GetGenericArguments()[0];
                    map.Buses[i] = busRegisterMap[arg];
                }

                ClassRegisterMap.Add(t, map);
            }
        }

        public static void Register(IEventReceiverBase target)
        {
            var t = target.GetType();
            var map = ClassRegisterMap[t];

            foreach (var busMap in map.Buses)
                busMap.Register(target);
        }

        public static void UnRegister(IEventReceiverBase target)
        {
            var t = target.GetType();
            var map = ClassRegisterMap[t];

            foreach (var busMap in map.Buses)
                busMap.Unregister(target);
        }

        public static void Raise(IEvent ev) => CachedRaise[ev.GetType()](ev);
    }
}