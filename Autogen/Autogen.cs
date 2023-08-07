using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Autogen
{
    public static class Autogen
    {
        public static void GenerateCode()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var pathsToManagers = GetPathsToManagers(assembly);
        }

        private static IEnumerable<string> GetPathsToManagers(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(type => ImplementsInterface(type, "IManager"))
                .Select(type => assembly.Location)
                .Distinct()
                .ToList();
        }

        private static bool ImplementsInterface(Type type, string interfaceFullName)
        {
            return type.GetInterfaces().Any(i => i.FullName == interfaceFullName);
        }
    }
}