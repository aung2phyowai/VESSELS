using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VESSELS.PracticeLogic;

namespace VESSELS
{
    class PracticeScreen : GameScreen
    {
        bool disposeOnce = true;

        #region Fields

        //Game Logic
        PracticeCore practiceCore;

        //BCI components
        SSVEP ssvep;
        BCI2000comm bciComm;

        //Game component system
        GameComponentCollection components = new GameComponentCollection();

        //Screen alpha
        float pauseAlpha;

        #endregion Fields

        /// <summary>
        /// Constructor.
        /// </summary>
        public PracticeScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Load and Initialize Game Components
        /// </summary>
        public override void LoadContent()
        {

            DepthStencilState depthState = new DepthStencilState();
            depthState.DepthBufferEnable = true;
            depthState.DepthBufferWriteEnable = true;
            ScreenManager.GraphicsDevice.DepthStencilState = depthState;


            //Start New Game
            practiceCore = new PracticeCore(ScreenManager);
            practiceCore.Initialize();
            practiceCore.DrawOrder = 1;
            components.Add(practiceCore);

            //start SSVEP stimuli
            ssvep = new SSVEP(ScreenManager);
            ssvep.Initialize();
            ssvep.DrawOrder = 2;
            components.Add(ssvep);

            //Establish BCI2000 comm
            LSLcomm bciComm = new LSLcomm(ScreenManager.Game);
            //bciComm = new BCI2000comm(ScreenManager.Game);
            bciComm.Initialize();
            components.Add(bciComm);

        }

        // Release any resources taken up by the components
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
