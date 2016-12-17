using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VESSELS.AndroidAppLogic;
using VESSELS.Ring_Logic;


namespace VESSELS
{
    class AndroidAppScreen : GameScreen
    {

        #region Fields
        //BCI components
        BCI2000comm bciComm;
        RingStimulator ringStim;
        AndroidAppCore androidApp;

        //Game component system
        GameComponentCollection components = new GameComponentCollection();

        //Screen alpha
        float pauseAlpha;

        #endregion Fields

        /// <summary>
        /// Constructor.
        /// </summary>
        public AndroidAppScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Load and Initialize Game Components
        /// </summary>
        public override void LoadContent()
        {

            // android app core
            androidApp = new AndroidAppCore(ScreenManager);
            androidApp.Initialize();
            androidApp.DrawOrder = 1;
            components.Add(androidApp);

            //start Ring stimuli
            ringStim = new RingStimulator(ScreenManager);
            ringStim.Initialize();
            ringStim.DrawOrder = 2;
            components.Add(ringStim);

            //Establish BCI2000 comm
            bciComm = new BCI2000comm(ScreenManager.Game);
            bciComm.Initialize();
            components.Add(bciComm);
        }

        public override void UnloadContent()
        {

            foreach (GameComponent2 gc in components)
            {
                gc.Dispose();
            }
            components.Clear();
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            //Perform game logic only when the gameplay screen is active
            if (IsActive)
            {
                //Update the game components local to the gameplay screen
                components.Update(gameTime.ElapsedGameTime, gameTime.TotalGameTime);
            }
        }

        #region HandleInput
        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }

        }
        #endregion HandleInput

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Draw the game components local to the gameplay screen
            components.Draw(gameTime.ElapsedGameTime, gameTime.TotalGameTime);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

    }
}
