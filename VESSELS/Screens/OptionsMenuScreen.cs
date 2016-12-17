#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
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
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry MazeLevelMenuEntry;
        MenuEntry NumStim;
        MenuEntry StimColor;
        MenuEntry StimShape;
        MenuEntry StimFrequencies;
        MenuEntry StimPattern;
        MenuEntry StimType;
        MenuEntry StimSize;
        

        enum Levels
        {
            Medium,
            Hard,
            Easy,
        }


       // static string[] levels = { "Medium", "Hard", "Easy" };

        Levels currentMazeLevel = Levels.Medium;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            type = 2;
            // Create our menu entries.
            MazeLevelMenuEntry = new MenuEntry(string.Empty);

            NumStim = new MenuEntry("Number of Stimuli: 4");
            StimColor = new MenuEntry("Stimulus Color: White");
            StimShape = new MenuEntry("Stimulus Shape: Rectangle");
            StimPattern = new MenuEntry("Stimulus Pattern: Solid");
            StimType = new MenuEntry("Stimulus Type: SSVEP");
            StimFrequencies = new MenuEntry("Stimulus Frequency: [6Hz 6.6Hz 7.5Hz 8.5Hz]");
            StimSize= new MenuEntry("Stimulus Size: Medium");

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            MazeLevelMenuEntry.Selected += MazeLevelMenuEntrySelected;
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            //MenuEntries.Add(MazeLevelMenuEntry);
            MenuEntries.Add(StimColor);
            MenuEntries.Add(StimShape);
            MenuEntries.Add(StimPattern);
            MenuEntries.Add(StimSize);
            MenuEntries.Add(StimType);
            MenuEntries.Add(StimFrequencies);
            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            MazeLevelMenuEntry.Text = "Maze Difficulty: " + currentMazeLevel;
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the maze level menu entry is selected.
        /// </summary>
        void MazeLevelMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentMazeLevel++;
            if (currentMazeLevel > Levels.Easy)
            {
                currentMazeLevel = 0;
            }

            
            //Update the new command
            ScreenManager.Game.Services.RemoveService(typeof(Levels));
            ScreenManager.Game.Services.AddService(typeof(Levels), currentMazeLevel);
            Console.WriteLine("service sent");
            SetMenuEntryText();

        }


        #endregion
    }
}
