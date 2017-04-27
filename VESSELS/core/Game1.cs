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
            //screenManager.AddScreen(new BackgroundScreen(), null);
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
            //graphics.PreferredBackBufferHeight = screenManager.maxHeight;
            //graphics.PreferredBackBufferWidth = screenManager.maxWidth;
            GraphicsDevice.PresentationParameters.PresentationInterval = PresentInterval.One;
            graphics.PreferMultiSampling = false;
            graphics.ApplyChanges();

            //int[] margins = new int[] { 0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight };
            //int[] margins = new int[] { 0, 0, screenManager.smallWidth, screenManager.smallHeight};
            //int[] margins = new int[] { 0, 0, screenManager.maxWidth, screenManager.maxHeight };
            int[] margins = new int[] { -1, -1, -1, -1 };
            User32.DwmExtendFrameIntoClientArea(Window.Handle, ref margins);

            form = System.Windows.Forms.Control.FromHandle(Window.Handle).FindForm();
            screenManager.form = form;
            form.Visible = true;
            form.AllowTransparency = true;
            form.BackColor = System.Drawing.Color.Gray;
            form.TransparencyKey = System.Drawing.Color.Gray;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            form.WindowState = System.Windows.Forms.FormWindowState.Normal;
            form.TopMost = true;
            //form.DesktopLocation = new System.Drawing.Point(0, 0);
            //form.ClientSize = new System.Drawing.Size(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);
            form.ClientSize = new System.Drawing.Size(screenManager.smallWidth, screenManager.smallHeight);
            //form.ClientSize = new System.Drawing.Size(screenManager.maxWidth, screenManager.maxHeight);
           
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
            //GraphicsDevice.Clear(new Color(0,0,0,0.3f));
            GraphicsDevice.Clear(Color.Gray);
            base.Draw(gameTime);
        }
    }
}
