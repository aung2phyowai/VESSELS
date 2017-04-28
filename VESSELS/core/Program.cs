using System;

public static class Globals
{
    public static Version OS_VERSION;
}


namespace VESSELS
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            // Check windows version
            Console.WriteLine(Environment.OSVersion.ToString());
            string os_version = Environment.OSVersion.ToString();
            char delimiter = ' ';
            string[] os_version_split = os_version.Split(delimiter);
            string version_str = os_version_split[os_version_split.Length - 1];
            Version version = Version.Parse(version_str);
            // Windows 10 = 6.2, windows 7 = 6.1 
            Globals.OS_VERSION = version;
            
            // start main application
            using (Game1 game = new Game1())
            {
                game.Run();
            }
            
        }
    }
#endif
}

