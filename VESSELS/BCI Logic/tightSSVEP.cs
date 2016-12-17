using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VESSELS
{
    class tightSSVEP : DrawableGameComponent2
    {

        #region Fields
        //Stimulus textures
        Texture2D Horiz1, Horiz2, Vert1, Vert2;

        Color StimulusColor = Color.White;
        float stimScale = 0.20f;
        // Game and content
        public Game game;
        public ContentManager content;
        public SpriteBatch spriteBatch;

        //Avatar position for stimuli to follow
        Vector2 position;

        // stimulus tracker variables 
        int State = 1; int Cntr1 = 0;
        int State2 = 1; int Cntr2 = 0;
        int State3 = 1; int Cntr3 = 0;
        int State4 = 1; int Cntr4 = 0;
        int StimType = 2; int StimType2 = 2; int StimType3 = 2; int StimType4 = 2;

        int SCREENWIDTH, SCREENHEIGHT;

        #endregion Fields

        //Constructor
        public tightSSVEP(ScreenManager screenManager)
        {
            this.game = screenManager.Game;
            this.content = screenManager.Game.Content;
            this.spriteBatch = screenManager.SpriteBatch;
            this.SCREENHEIGHT = screenManager.GraphicsDevice.Viewport.Height;
            this.SCREENWIDTH = screenManager.GraphicsDevice.Viewport.Width;
        }

        //Initialize
        public override void Initialize()
        {            
            //Load Solid Stimuli
            Horiz1 = content.Load<Texture2D>(@"Textures/SSVEP_Textures/solid1");
            Horiz2 = content.Load<Texture2D>(@"Textures/SSVEP_Textures/solid2");
            Vert1 = content.Load<Texture2D>(@"Textures/SSVEP_Textures/solidVert1");
            Vert2 = content.Load<Texture2D>(@"Textures/SSVEP_Textures/solidVert2");

            base.Initialize();
        }

        /// <summary>
        /// Updates the stimuli states
        /// </summary>
        public override void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            // Update the stimuli
            UpdateStimulus();
        }

        /// <summary>
        /// Draws the stimuli States
        /// </summary>
        public override void Draw(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            //Flash the stimuli
            spriteBatch.Begin();

            //get Updated avatar position
            try
            {
              position=(Vector2)game.Services.GetService(typeof(Vector2));
            } catch (Exception) {}; 

             Draw4stim();
            spriteBatch.End();

        }

        //Draw four flashing stimuli
        void Draw4stim()
        {
            //TOP stimuli - 8.5 Stimulus Class 1
            if (StimType == 1)
            {
                spriteBatch.Draw(Horiz1, new Vector2(position.X - (Horiz1.Width / 2) * stimScale, position.Y - 80 - 2*(Horiz1.Height) * stimScale), null, Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
            }
            if (StimType == 2)
            {
                spriteBatch.Draw(Horiz2, new Vector2(position.X - (Horiz1.Width / 2) * stimScale, position.Y - 80 - 2*(Horiz1.Height) * stimScale), null, Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
            }

            //BOTTOM 6hz stimulus Class 2
            if (StimType2 == 1)
            {
                spriteBatch.Draw(Horiz1, new Vector2(position.X - (Horiz1.Width / 2) * stimScale, position.Y + 16+ (Horiz1.Height)*stimScale), null, Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
            }
            if (StimType2 == 2)
            {
                spriteBatch.Draw(Horiz2, new Vector2(position.X - (Horiz1.Width / 2) * stimScale, position.Y + 16+(Horiz1.Height)*stimScale), null, Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
            }

            //RIGHT 7.5hz stimulus Class 3
            if (StimType3 == 1)
            {
                
                spriteBatch.Draw(Vert1, new Vector2((position.X -32 + 2*(Vert1.Width * stimScale)), (position.Y -32- ((Vert1.Height/2) * stimScale))), null, Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
            }
            if (StimType3 == 2)
            {
                spriteBatch.Draw(Vert2, new Vector2((position.X -32 +2*(Vert2.Width * stimScale)), (position.Y -32- ((Vert2.Height/2) * stimScale))), null, Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
            }

            //LEFT 6.66hz stimulus Class4
            if (StimType4 == 1)
            {
                spriteBatch.Draw(Vert1, new Vector2((position.X - 48- 2 * (Vert1.Width * stimScale)), (position.Y - 32 - ((Vert1.Height / 2) * stimScale))), null, Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
            }
            if (StimType4 == 2)
            {
                spriteBatch.Draw(Vert2, new Vector2((position.X - 48- 2 * (Vert1.Width * stimScale)), (position.Y - 32 - ((Vert1.Height / 2) * stimScale))), null, Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
            }
        }


        /// <summary>
        /// Updates the states of the stimuli
        /// </summary>
        void UpdateStimulus()
        {
            //update the counters
            Cntr1++; Cntr2++;
            Cntr3++; Cntr4++;

            //Update the 8.5hz stimulus 
            if (Cntr1 == 3 && State == 1)
            {
                StimType = 1;
                Cntr1 = 0;
                State = 0;
            }
            else if (Cntr1 == 4 && State == 0)
            {
                StimType = 2;
                Cntr1 = 0;
                State = 1;
            }

            //Update the 6hz stimulus 
            if (Cntr2 == 5 && State2 == 1)
            {
                StimType2 = 1;
                Cntr2 = 0;
                State2 = 0;
            }
            else if (Cntr2 == 5 && State2 == 0)
            {
                StimType2 = 2;
                Cntr2 = 0;
                State2 = 1;
            }

            //Update the 7.5hz stimulus 
            if (Cntr3 == 4 && State3 == 1)
            {
                StimType3 = 1;
                Cntr3 = 0;
                State3 = 0;
            }
            else if (Cntr3 == 4 && State3 == 0)
            {
                StimType3 = 2;
                Cntr3 = 0;
                State3 = 1;
            }

            //Update the 6.66hz stimulus 
            if (Cntr4 == 4 && State4 == 1)
            {
                StimType4 = 1;
                Cntr4 = 0;
                State4 = 0;
            }
            else if (Cntr4 == 5 && State4 == 0)
            {
                StimType4 = 2;
                Cntr4 = 0;
                State4 = 1;
            }
        }


        //Draw three flashing stimuli
        //void Draw3stim()
        //{
        //    //TOP stimuli - 7.5
        //    if (StimType == 1)
        //    {
        //        spriteBatch.Draw(Check1, new Vector2((SCREENWIDTH - (Check1.Width * stimScale)) / 2, 0), new Rectangle(0, 0, Check1.Width, Check1.Height / 2), Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
        //    }
        //    if (StimType == 2)
        //    {
        //        spriteBatch.Draw(Check2, new Vector2((SCREENWIDTH - (Check2.Width * stimScale)) / 2, 0), new Rectangle(0, 0, Check1.Width, Check1.Height / 2), Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
        //    }

        //    //RIGHT 10hz stimuli
        //    if (StimType3 == 1)
        //    {
        //        spriteBatch.Draw(Check1, new Vector2((SCREENWIDTH - (Check1.Height * stimScale) / 2.0f), (SCREENHEIGHT - (Check1.Height * stimScale))), new Rectangle(0, 0, Check1.Width, Check1.Height / 2), Color.White, MathHelper.ToRadians(-90), Vector2.Zero, stimScale, SpriteEffects.None, 0);
        //    }
        //    if (StimType3 == 2)
        //    {
        //        spriteBatch.Draw(Check2, new Vector2((SCREENWIDTH - (Check2.Height * stimScale) / 2.0f), (SCREENHEIGHT - (Check2.Height * stimScale))), new Rectangle(0, 0, Check1.Width, Check1.Height / 2), Color.White, MathHelper.ToRadians(-90), Vector2.Zero, stimScale, SpriteEffects.None, 0);
        //    }

        //    //LEFT 6hz stimuli
        //    if (StimType4 == 1)
        //    {
        //        spriteBatch.Draw(Check1, new Vector2(0, (SCREENHEIGHT - ((Check1.Height) * stimScale))), new Rectangle(0, 0, Check1.Width, Check1.Height / 2), Color.White, MathHelper.ToRadians(-90), Vector2.Zero, stimScale, SpriteEffects.None, 0);
        //    }
        //    if (StimType4 == 2)
        //    {
        //        spriteBatch.Draw(Check2, new Vector2(0, (SCREENHEIGHT - (Check2.Height * stimScale))), new Rectangle(0, 0, Check1.Width, Check1.Height / 2), Color.White, MathHelper.ToRadians(-90), Vector2.Zero, stimScale, SpriteEffects.None, 0);
        //    }
        //}

        //void DrawStimSkyGame()
        //{
        //    //TOP stimuli - 8.5
        //    if (StimType == 1)
        //    {
        //        spriteBatch.Draw(Check1, new Vector2((SCREENWIDTH - (Check1.Width * stimScale)) / 2, 0), null, Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
        //    }
        //    if (StimType == 2)
        //    {
        //        spriteBatch.Draw(Check2, new Vector2((SCREENWIDTH - (Check2.Width * stimScale)) / 2, 0), null, Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
        //    }

        //    //RIGHT 7.5hz stimuli
        //    if (StimType3 == 1)
        //    {
        //        spriteBatch.Draw(Check1, new Vector2((SCREENWIDTH - (Check1.Width * stimScale)), (SCREENHEIGHT - ((Check1.Height) * stimScale))), null, Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
        //    }
        //    if (StimType3 == 2)
        //    {
        //        spriteBatch.Draw(Check2, new Vector2((SCREENWIDTH - (Check2.Width * stimScale)), (SCREENHEIGHT - ((Check1.Height) * stimScale))), null, Color.White, 0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
        //    }

        //    //LEFT 6.6hz stimuli
        //    if (StimType4 == 1)
        //    {
        //        spriteBatch.Draw(Check1, new Vector2(0, (SCREENHEIGHT - ((Check1.Height) * stimScale))), null, Color.White,0, Vector2.Zero, stimScale, SpriteEffects.None, 0);
        //    }
        //    if (StimType4 == 2)
        //    {
        //        spriteBatch.Draw(Check2, new Vector2(0, (SCREENHEIGHT - (Check2.Height * stimScale))), null, Color.White, 0, Vector2.Zero,stimScale, SpriteEffects.None, 0);
        //    }

        //}
    }
}
