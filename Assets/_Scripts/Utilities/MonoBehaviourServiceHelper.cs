using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _Scripts.Utilities
{
    public static class MonoBehaviourServiceHelper
    {
        public static List<T> GetAllInstances<T>()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly
                    .GetTypes(), (assembly, type) => new {assembly, type})
                .Where(t =>
                    typeof(T)
                        .IsAssignableFrom(t.type) && !t.type.IsInterface &&
                    t.type != typeof(MonoBehaviourService<>))
                .Select(t => typeof(MonoBehaviourService<>)
                    .MakeGenericType(t.type))
                .Select(genericType => genericType
                    .GetMethod("Get", BindingFlags.Static | BindingFlags.Public))
                .Where(method => method != null)
                .Select(method => method
                    .Invoke(null, new object[] {true}))
                .Where(instance => instance != null)
                .Select(instance => (T) instance)
                .ToList();
        }
    }
}