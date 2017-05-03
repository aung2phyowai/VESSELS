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
            string version_str;
            Version version = new Version();
            for (int i=0; i<os_version_split.Length; i++)
            {
                try
                {
                    version_str = os_version_split[i];
                    version = Version.Parse(version_str);
                }
                catch (Exception e) { }
            }
            Globals.OS_VERSION = version;
            Console.WriteLine(version.Minor.ToString());

            // start main application
            using (Game1 game = new Game1())
            {
                game.Run();
            }
            
        }
    }
#endif
}

