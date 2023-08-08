namespace FiledGenerator
{
    public static class AssemblyExtensions
    {
        private static IEnumerable<string> GetPathsToManagers(this Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(type => ImplementsInterface(type, "IManager"))
                .Select(type => assembly.Location)
                .Distinct()
                .ToList();
        }
        
        private static string GetPathToGameManager(this Assembly assembly)
        {
            return assembly.GetTypes()
                .FirstOrDefault(type => type.Name == "GameManager");
        }
    }
}