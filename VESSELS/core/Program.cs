using System;
using Forms = System.Windows.Forms;
using VESSELS.WinForms;
using System.Threading;

public static class Globals
{
    public static Version OS_VERSION;
    public static bool APP_RUNNING = true;
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

            // Set these for some reason...
            Forms.Application.EnableVisualStyles();
            Forms.Application.SetCompatibleTextRenderingDefault(false);
            MainMenu mainMenu;
            /*
            // MAIN APPLICATION LOOP
            while (Globals.APP_RUNNING)
            {
                // Initialize main winform menu
                mainMenu = new MainMenu();
                Forms.Application.Run(mainMenu);
                //Thread.Sleep(2000);
                // start main application
                if(Globals.APP_RUNNING)
                {
                    using (Game1 game = new Game1())
                    {
                        game.Run();
                    }
                }
                
            } */
            Game1 game;

            Forms.ApplicationContext ac;

            // practice
            mainMenu = new MainMenu();
            ac = new Forms.ApplicationContext(mainMenu);
            Forms.Application.Run(ac);
            //Forms.Application.Run(mainMenu);
            //mainMenu.ShowDialog();

            Thread.Sleep(1000);

            game = new Game1();
            game.Run();

            Thread.Sleep(1000);

            //Forms.Application.Restart();

            mainMenu = new MainMenu();
            //Forms.Application.Run(mainMenu);
            //mainMenu.ShowDialog();


            ac = new Forms.ApplicationContext(mainMenu);
            Forms.Application.Run(ac);

            

        }
    }
#endif
}

