using System;
using System.IO;

namespace TriQue.Helpers
{
    public static class EnvLoader
    {
        public static void Load(string path = ".env")
        {
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

            if (!File.Exists(path))
                throw new FileNotFoundException($".env not found at {path}");

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("#"))
                    continue;

                var parts = line.Split('=', 2);

                if (parts.Length != 2)
                    continue;

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                    continue;

                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
}