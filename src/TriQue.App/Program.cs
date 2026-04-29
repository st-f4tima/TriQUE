using TriQue.Data.Database;
using TriQue.Forms;
using TriQue.Helpers;


namespace TriQue
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var databaseHelper = new DatabaseHelper();
            var dbInitializer = new DatabaseInitializer(databaseHelper);
            dbInitializer.Initialize();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            EnvLoader.Load();
            Console.WriteLine("API KEY: " + Environment.GetEnvironmentVariable("TOMTOM_API_KEY"));
            ApplicationConfiguration.Initialize();
            Application.Run(new LoginForm());
        }
    }
}