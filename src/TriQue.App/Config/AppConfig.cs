using Microsoft.Extensions.Configuration;
using System.IO;

public static class AppConfig
{
    public static IConfiguration Configuration { get; private set; }

    static AppConfig()
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;

        Configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
    }
}