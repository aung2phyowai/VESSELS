#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
#endregion

namespace VESSELS
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {   
        #region Initialization
        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("VESSELS:")
        {
            title2 = "Visual Evoked Stimulation and SELection Software";
            // Create our menu entries.
            MenuEntry WifiRobotMenuEntry = new MenuEntry("Wifi - Robot");
            MenuEntry PracticeMenuEntry = new MenuEntry("Practice (FeedBack)");
            MenuEntry MazeGameMenuEntry = new MenuEntry("PacMan BCI");
            MenuEntry BingMapsMenuEntry = new MenuEntry("Google Maps");
            MenuEntry City3dMenuEntry = new MenuEntry("Virtual Environment");
            MenuEntry RingAppMenuEntry = new MenuEntry("Ring Stimulator");
            MenuEntry optionsMenuEntry = new MenuEntry("Options / Configurations");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");
            MenuEntry SOCIMenuEntry = new MenuEntry("SOCS Interface");

            // Hook up menu event handlers.
            SOCIMenuEntry.Selected += SOCIMenuEntrySelected;
            WifiRobotMenuEntry.Selected += WifiRobotMenuEntrySelected;
            PracticeMenuEntry.Selected += PracticeMenuEntrySelected;
            City3dMenuEntry.Selected += City3dMenuEntrySelected;
            BingMapsMenuEntry.Selected += BingMapsMenuEntrySelected;
            MazeGameMenuEntry.Selected += MazeGameMenuEntrySelected;
            RingAppMenuEntry.Selected += RingAppMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(SOCIMenuEntry);
            MenuEntries.Add(WifiRobotMenuEntry);
            MenuEntries.Add(PracticeMenuEntry);
            MenuEntries.Add(MazeGameMenuEntry);
            MenuEntries.Add(BingMapsMenuEntry);
            MenuEntries.Add(City3dMenuEntry);
            MenuEntries.Add(RingAppMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Play Game 1 menu entry is selected.
        /// </summary>
        void SOCIMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            toggleFullScreen(); 
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new SOCIScreen());
        }

        /// <summary>
        /// Event handler for when the Play Game 1 menu entry is selected.
        /// </summary>
        void WifiRobotMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            toggleFullScreen();
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new AndroidAppScreen());
        }

        /// <summary>
        /// Event handler for when the Play Game 1 menu entry is selected.
        /// </summary>
        void RingAppMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            toggleFullScreen();
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new RingAppGameScreen());
        }

        /// <summary>
        /// Event handler for when the Play Game 1 menu entry is selected.
        /// </summary>
        void PracticeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            toggleFullScreen();
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new PracticeScreen());
        }

        /// <summary>
        /// Event handler for when the Play Game 1 menu entry is selected.
        /// </summary>
        void City3dMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            toggleFullScreen();
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new City3DGamePlayScreen());
        }

        /// <summary>
        /// Event handler for when the Play Game 1 menu entry is selected.
        /// </summary>
        void BingMapsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            toggleFullScreen();
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new BingMapsPlayScreen());
        }
        
        /// <summary>
        /// Event handler for when the Play Game 1 menu entry is selected.
        /// </summary>
        void MazeGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            toggleFullScreen();
            ScreenManager.AddScreen(new PacManOptionsScreen(), e.PlayerIndex);

            //LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
            //                   new MazeGamePlayScreen(1,1));
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //toggleFullScreen();
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this Application?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        //Go back to full screen mode before starting an application
        void toggleFullScreen()
        {
            //Adjust windows form parameters for full-screen mode
            ScreenManager.form.ClientSize = new System.Drawing.Size(ScreenManager.maxWidth, ScreenManager.maxHeight);
            ScreenManager.form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            ScreenManager.form.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            //Adjust directx graphics engine for full-screen mode
            if(Globals.OS_VERSION.Minor == 2)
            {
                ScreenManager.form.AllowTransparency = true;
                ScreenManager.form.TransparencyKey = System.Drawing.Color.Gray;
            }
            else
            {
                ScreenManager.form.AllowTransparency = false;
                ScreenManager.form.TransparencyKey = ScreenManager.form.BackColor;
            }
            
            ScreenManager.graphics.PreferredBackBufferHeight = ScreenManager.maxHeight;
            ScreenManager.graphics.PreferredBackBufferWidth = ScreenManager.maxWidth;
            ScreenManager.graphics.ApplyChanges();
        }

       

        #endregion
    }
}
