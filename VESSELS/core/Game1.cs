using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Runtime.InteropServices;

namespace VESSELS
{
    //main class
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Fields
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        public System.Windows.Forms.Form form;
        public Color back_color = Color.Gray;


        //Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this) 
            { 
                SynchronizeWithVerticalRetrace = true,                
            };
            Content.RootDirectory = "Content"; 
            // Create the screen manager component.
            screenManager = new ScreenManager(this, graphics);
            Components.Add(screenManager);
            IsMouseVisible = true;
            // Activate the first screens.
            screenManager.AddScreen(new MainMenuScreen(), null);
            // keep a fixed time step of 60hz
            IsFixedTimeStep = true;
            TargetElapsedTime = new TimeSpan(10000000L / 60L);
            //extend the sleeper timout so it doesnt pause
            InactiveSleepTime = TimeSpan.Zero;
        }

        //Initialize
        protected override void Initialize()
        {
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;

            //Set the windows forms, and directX graphics buffer to match the screen of the host computer
            int maxHeight = 0; int maxWidth = 0; // vars to choose the max width and height
            GraphicsAdapter g = graphics.GraphicsDevice.Adapter;
            foreach (DisplayMode dm in g.SupportedDisplayModes)
            {
                if (maxHeight < dm.Height)
                {
                    maxHeight = dm.Height;
                }
                if (maxWidth < dm.Width)
                {
                    maxWidth = dm.Width;
                }
            }
            screenManager.maxHeight = maxHeight;
            screenManager.maxWidth = maxWidth;
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            graphics.PreferredBackBufferHeight = screenManager.smallHeight;
            graphics.PreferredBackBufferWidth = screenManager.smallWidth;
            GraphicsDevice.PresentationParameters.PresentationInterval = PresentInterval.One;
            graphics.PreferMultiSampling = false;
            graphics.ApplyChanges();

            
            int[] margins = new int[] { -1, -1, -1, -1 };
            User32.DwmExtendFrameIntoClientArea(Window.Handle, ref margins);
            form = System.Windows.Forms.Control.FromHandle(Window.Handle).FindForm();
            screenManager.form = form;
            form.Visible = true;
            form.AllowTransparency = true;

            // set parameters based on windows version
            if (Globals.OS_VERSION.Minor == 2) //Windows 10
            {
                back_color = Color.Gray;
                form.BackColor = System.Drawing.Color.Gray;
                form.TransparencyKey = System.Drawing.Color.Gray;
            }
            else                              //Windows 7
            {
                back_color = new Color(0, 0, 0, 0.3f);
            }

            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            form.WindowState = System.Windows.Forms.FormWindowState.Normal;
            form.TopMost = true;
            form.ClientSize = new System.Drawing.Size(screenManager.smallWidth, screenManager.smallHeight);
           
            base.Initialize();
        }

        //Load
        protected override void LoadContent()
        {
        }

        //Unload
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            foreach (GameComponent gc in Components)
            {
                gc.Dispose();
            }
        }

        //Update
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        //Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(back_color);
            base.Draw(gameTime);
        }
    }
}
