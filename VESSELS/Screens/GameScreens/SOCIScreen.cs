using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VESSELS.SOCI.DirectX_Overlay;
using VESSELS.SOCI.GDI_Overlay;
using System.Runtime.InteropServices;
using VESSELS.BCI_Logic.Device_Emulation;
using VESSELS.BCI_Logic.BCI2000_Control.SpecificApp;

namespace VESSELS
{
    class SOCIScreen : GameScreen
    {

        #region Fields

        //BCI components
        WowControl wowBCIcontrol;
        SSVEP_DirectX_Advanced_V2 ssvepDX;

        //Screen alpha
        float pauseAlpha;

        #endregion Fields

        /// <summary>
        /// Constructor.
        /// </summary>
        public SOCIScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.0);
        }

        /// <summary>
        /// Load and Initialize Game Components
        /// </summary>
        public override void LoadContent()
        {
            //Adjust DWM frame settings
            int[] margins = new int[] { 0, 0, ScreenManager.maxWidth, ScreenManager.maxHeight };
            User32.DwmExtendFrameIntoClientArea(ScreenManager.Game.Window.Handle, ref margins);

            DepthStencilState depthState = new DepthStencilState();
            depthState.DepthBufferEnable = false;
            depthState.DepthBufferWriteEnable = false;
            ScreenManager.GraphicsDevice.DepthStencilState = depthState;

            //Hook the interceptKeys module
            InterceptKeys.Hook();

            wowBCIcontrol = new WowControl(ScreenManager.Game, "World Of Warcraft");
            ScreenManager.Game.Components.Add(wowBCIcontrol);

            ssvepDX = new SSVEP_DirectX_Advanced_V2(ScreenManager.Game);
            ssvepDX.Initialize(ScreenManager.form, ScreenManager.SpriteBatch);
            ScreenManager.Game.Components.Add(ssvepDX);
        }

        // Release any resources taken up by the components
        public override void UnloadContent()
        {
            ScreenManager.Game.Components.Remove(wowBCIcontrol);
            wowBCIcontrol.Dispose();
            ScreenManager.Game.Components.Remove(ssvepDX);
            ssvepDX.Dispose();

            InterceptKeys.Unhook();
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
            if (!IsActive && previouslyActive)
            {
                if(ssvepDX.flashing == true)
                    ssvepDX.flashing = false;
                previouslyActive = false;
            }
            if(IsActive)
            {
                previouslyActive = true;
            }
        }

        bool previouslyActive = true;

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
            if(Globals.OS_VERSION.Minor == 2)
                ScreenManager.GraphicsDevice.Clear(Color.Gray);
            else
                ScreenManager.GraphicsDevice.Clear(Color.Transparent);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

    }
}
