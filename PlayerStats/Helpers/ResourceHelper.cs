using System;
using System.IO;
using System.Reflection;

namespace RestoreMonarchy.PlayerStats.Helpers
{
    internal static class ResourceHelper
    {
        internal static string GetResourceFileContent(string fileName)
        {
            Assembly assembly = typeof(ResourceHelper).Assembly;
            string[] resourceNames = assembly.GetManifestResourceNames();

            foreach (string resourceName in resourceNames)
            {
                if (!resourceName.EndsWith(fileName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                using Stream stream = assembly.GetManifestResourceStream(resourceName);
                stream.Position = 0;
                using StreamReader reader = new(stream);
                return reader.ReadToEnd();
            }

            throw new FileNotFoundException("Failed to find the reosurce in the project files", fileName);
        }
    }
}
