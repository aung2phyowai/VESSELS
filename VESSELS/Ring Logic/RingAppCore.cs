using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VESSELS.Ring_Logic
{
   
    class RingAppCore : DrawableGameComponent2
    {
        #region Fields
        //Copy of the screenmanager
        ScreenManager screenManager;
        //SpriteBatch
        SpriteBatch spriteBatch;
        //Screen width and height
        int SCREENWIDTH, SCREENHEIGHT;
        //Content manager
        ContentManager content;
        Texture2D Cross;
        //Cross Positions
        Vector2[] Radial = new Vector2[17];
        int FB = 0;
        #endregion Fields

        //Constructor
        public RingAppCore(ScreenManager screenManager)
        {
            this.screenManager = screenManager;

            this.SCREENHEIGHT = screenManager.GraphicsDevice.Viewport.Height;
            this.SCREENWIDTH = screenManager.GraphicsDevice.Viewport.Width;

        }
        
        //Load Content
        public override void Initialize()
        {
            //content manager
            if (content == null)
                content = new ContentManager(screenManager.Game.Services, "Content");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = screenManager.SpriteBatch;

            // Load Textures
            Cross = content.Load<Texture2D>(@"Textures/Ring Textures/cross");

            InitializePoints();

            base.Initialize();
        }

        // Initialize Cross positions
        private void InitializePoints()
        {
            int cnt = 0;
            // Draws a ring
            int cx = 0; int cy = 0; float r = (float)(SCREENHEIGHT / 2);
            float r2 = r - 120f;
            #region CrossPositions
            float max = r2 - 50;
            float interval = max / 5;
            float x = r; float y = 0;
            float x2 = r2; float y2 = 0;

            //Radial
            cnt = 0; float[] radius = { r2 + 60, r2 - 85 };
            for (int j = 0; j < 2; j++)
            {
                for (int i = 1; i <= 8; i++)
                {
                    x = ((SCREENWIDTH / 2) - ((Cross.Bounds.Width * 0.1f) / 2)) - ((float)((radius[j]) * Math.Cos(MathHelper.ToRadians((45f * i)))));
                    y = ((SCREENHEIGHT / 2) - ((Cross.Bounds.Height * 0.1f) / 2)) - ((float)((radius[j]) * Math.Sin(MathHelper.ToRadians((45f * i)))));
                    Radial[cnt] = new Vector2(x, y);
                    cnt++;
                }
            }
            x = ((SCREENWIDTH / 2) - ((Cross.Bounds.Width * 0.1f) / 2));
            y = ((SCREENHEIGHT / 2) - ((Cross.Bounds.Height * 0.1f) / 2));
            Radial[cnt] = new Vector2(x, y);
            #endregion CrossPositions
        }

        //Update 
        public override void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            //Check update from BCI2000
            try
            {
                //Check to see if a new command is available.
                if ((bool)screenManager.Game.Services.GetService(typeof(bool)) == true)
                {
                    FB = (int)screenManager.Game.Services.GetService(typeof(int));
                    
                    //set the new command status to false
                    screenManager.Game.Services.RemoveService(typeof(bool));
                    screenManager.Game.Services.AddService(typeof(bool), false);

                }
            }
            catch (Exception) { }
        }

        //Draw 
        public override void Draw(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            //begin spritebatch
            spriteBatch.Begin();

            //Draw All Fixation Crosses
            for (int i = 9; i < 16; i=i+2)
            {
                spriteBatch.Draw(Cross, Radial[i], null, Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 1);
            }
            //Draw Feedback
            if (FB != 0)
            {
                switch (FB)
                {
                    case 1:
                spriteBatch.Draw(Cross, Radial[9], null, Color.FromNonPremultiplied(new Vector4(0, 0.8f, 0, 1)), 0, Vector2.Zero, 0.1f, SpriteEffects.None, 1);
                break;
                    case 2:
                spriteBatch.Draw(Cross, Radial[13], null, Color.FromNonPremultiplied(new Vector4(0, 0.8f, 0, 1)), 0, Vector2.Zero, 0.1f, SpriteEffects.None, 1);
                break;
                    case 3:
                spriteBatch.Draw(Cross, Radial[11], null, Color.FromNonPremultiplied(new Vector4(0, 0.8f, 0, 1)), 0, Vector2.Zero, 0.1f, SpriteEffects.None, 1);
                break;
                    case 4:
                spriteBatch.Draw(Cross, Radial[15], null, Color.FromNonPremultiplied(new Vector4(0, 0.8f, 0, 1)), 0, Vector2.Zero, 0.1f, SpriteEffects.None, 1);
                break;
                }
            }

            //end spritebatch
            spriteBatch.End();
        }
    }
}
