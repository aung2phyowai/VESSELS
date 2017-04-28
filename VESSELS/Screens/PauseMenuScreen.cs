#region File Description
//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace VESSELS
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen()
            : base("Paused")
        {
            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Demo");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Demo");
            
            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this Demo?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            toggleSmallScreen();
            LoadingScreen.Load(ScreenManager, false, null, new MainMenuScreen());
        }

        //Go back to full screen mode before starting an application
        void toggleSmallScreen()
        {
            //Adjust windows form parameters for normal-screen mode
            ScreenManager.form.ClientSize = new System.Drawing.Size(ScreenManager.smallWidth, ScreenManager.smallHeight);
            ScreenManager.form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            ScreenManager.form.WindowState = System.Windows.Forms.FormWindowState.Normal;

            //Adjust directx graphics engine for normal-screen mode
            if(Globals.OS_VERSION.Minor==2)
                ScreenManager.form.AllowTransparency = true;
            else
                ScreenManager.form.AllowTransparency = false;
            ScreenManager.graphics.PreferredBackBufferHeight = ScreenManager.smallHeight;
            ScreenManager.graphics.PreferredBackBufferWidth = ScreenManager.smallWidth;
            ScreenManager.graphics.ApplyChanges();
        }

        #endregion
    }
}
