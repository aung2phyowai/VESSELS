using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VESSELS
{
    class PacManOptionsScreen : MenuScreen
    {
        MenuEntry GameVariantMenuEntry;
        MenuEntry MazeLevelMenuEntry;
        MenuEntry StartGameMenuEntry;

        enum Levels
        {   
            Easy,
            Medium,
            Hard,
        }

        enum Variants
        {
            Standard,
            Dynamic,
            Google_Glass,
            Low_Fatigue,
        }

        Levels currentMazeLevel = Levels.Medium;
        Variants currentVariant = Variants.Standard;

        //Constructor - fill in menu contents
        public PacManOptionsScreen()
            : base ("Pac-Man BCI")
        {
            GameVariantMenuEntry = new MenuEntry(string.Empty);
            MazeLevelMenuEntry = new MenuEntry(string.Empty);
            StartGameMenuEntry = new MenuEntry("Start Pac-Man");
            SetMenuEntryText();
            MenuEntry back = new MenuEntry("Back");
            // Hook up menu event handlers.
            MazeLevelMenuEntry.Selected += MazeLevelMenuEntrySelected;
            GameVariantMenuEntry.Selected += GameVariantMenuEntrySelected;
            StartGameMenuEntry.Selected += GameStartMenuEntrySelected;
            back.Selected += OnCancel;

            // Add entries to the menu
            MenuEntries.Add(GameVariantMenuEntry);
            MenuEntries.Add(MazeLevelMenuEntry);
            MenuEntries.Add(StartGameMenuEntry);
            MenuEntries.Add(back);
        }

        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            GameVariantMenuEntry.Text = "Game Variant: " + currentVariant;
            MazeLevelMenuEntry.Text = "Maze Difficulty: " + currentMazeLevel;
        }

        /// <summary>
        /// Event handler for when the maze level menu entry is selected.
        /// </summary>
        void MazeLevelMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentMazeLevel++;
            if (currentMazeLevel > Levels.Hard)
            {
                currentMazeLevel = 0;
            }
            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Game Variant menu entry is selected.
        /// </summary>
        void GameVariantMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentVariant++;
            if (currentVariant > Variants.Low_Fatigue)
            {
                currentVariant= 0;
            }
            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Game Start menu entry is selected.
        /// </summary>
        void GameStartMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                              new MazeGamePlayScreen((int)currentVariant, (int)currentMazeLevel));
        }

    }
}
