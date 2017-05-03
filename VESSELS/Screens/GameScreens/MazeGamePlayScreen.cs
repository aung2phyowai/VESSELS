using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VESSELS.MazeGameLogic;

namespace VESSELS
{
    class MazeGamePlayScreen : GameScreen
    {
        #region Fields
        //game options
        int variant;
        int level;
        //Game component system
        GameComponentCollection components = new GameComponentCollection();

        //Screen alpha
        float pauseAlpha;
        #endregion Fields

        /// <summary>
        /// Constructor.
        /// </summary>
        public MazeGamePlayScreen(int variant, int level)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            this.variant = variant;
            this.level = level;
        }

        /// <summary>
        /// Load and Initialize Game Components
        /// </summary>
        public override void LoadContent()
        {
            //Start New Game
            GameCore gameCore = new GameCore(ScreenManager, level); 
            gameCore.Initialize();
            gameCore.DrawOrder = 1;
            components.Add(gameCore);

            ////start SSVEP stimuli
            if (variant == 0)
            {
                SSVEP ssvep = new SSVEP(ScreenManager);
                ssvep.Initialize();
                ssvep.DrawOrder = 2;
                components.Add(ssvep);
            }
            else if (variant == 1)
            {
                //start SSVEP stimuli
                tightSSVEP ssvep = new tightSSVEP(ScreenManager);
                ssvep.Initialize();
                ssvep.DrawOrder = 2;
                components.Add(ssvep);
            }
            else if (variant == 2)
            { 
            }
            else
            {
                SSVEPcb ssvep = new SSVEPcb(ScreenManager);
                ssvep.Initialize();
                ssvep.DrawOrder = 2;
                components.Add(ssvep);
            }

            //Establish BCI2000 comm
            LSLcomm bciComm = new LSLcomm(ScreenManager.Game);
            //BCI2000comm bciComm = new BCI2000comm(ScreenManager.Game);
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
            ScreenManager.GraphicsDevice.Clear(new Color(0, 0, 0, 1.0f));
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
