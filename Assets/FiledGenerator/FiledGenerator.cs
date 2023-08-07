using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FiledGenerator
{
    public class FiledGenerator
    {
        public static bool GenerateFields()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var pathsToManagers = GetPathsToManagers(assembly);
            return false;
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