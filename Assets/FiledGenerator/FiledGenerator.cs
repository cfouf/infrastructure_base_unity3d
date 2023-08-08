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

            var pathsToManagers = assembly.GetPathsToManagers();

            var pathToManager = assembly.GetPathToGameManager();
            
            return false;
        }

        private static bool ImplementsInterface(Type type, string interfaceFullName)
        {
            return type.GetInterfaces().Any(i => i.FullName == interfaceFullName);
        }
    }
}