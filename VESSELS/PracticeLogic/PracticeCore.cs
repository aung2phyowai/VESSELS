using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VESSELS.PracticeLogic
{
    class PracticeCore : DrawableGameComponent2
    {
        int delta = 35;
        int delta2 = 210;
        #region Fields
        //Copy of the screenmanager
        ScreenManager screenManager;

        //SpriteBatch
        SpriteBatch spriteBatch;

        //Feedback bars
        Texture2D Vert;

        //Source Rectangle
        Rectangle source;

        //Content manager
        ContentManager content;

      //  int feedBackHeight=0;
        int feedBackLeft = 0;
        int feedBackRight = 0;
        int feedBackUp = 0;
        int feedBackDown = 0;

        //Screen width and height
        int SCREENWIDTH, SCREENHEIGHT;

        #endregion Fields

        //Constructor
        public PracticeCore(ScreenManager screenManager)
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

            Vert = content.Load<Texture2D>(@"Textures/FeedbackBars/vert");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(screenManager.GraphicsDevice);

            base.Initialize();
        }

         //Update Game
        public override void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {

            //Check update from BCI2000
            try
            {
                //Check to see if a new command is available.
                if ((bool)screenManager.Game.Services.GetService(typeof(bool)) == true)
                {
                    int Val = (int)screenManager.Game.Services.GetService(typeof(int));

                    //update based on commnad
                    switch (Val)
                    {
                        case 0:     //Dont move

                            break;
                        case 1:     //move up
                            feedBackRight -= delta2;
                            feedBackLeft -= delta2;
                            feedBackDown -= delta2;

                            feedBackRight = 0;
                            feedBackLeft = 0;
                            feedBackDown = 0;

                            if (feedBackRight < 0)
                            {
                                feedBackRight = 0;
                            }

                            if (feedBackLeft < 0)
                            {
                                feedBackLeft = 0;
                            }

                            if (feedBackDown < 0)
                            {
                                feedBackDown = 0;
                            }

                            if (feedBackRight == 0 && feedBackLeft == 0 && feedBackDown == 0)
                            {
                                feedBackUp += delta;
                                if (feedBackUp >= 1000)
                                {
                                    feedBackUp = 1000;
                                }
                            }
                            break;
                        case 2:     //move down
                            feedBackRight -= delta2;
                            feedBackLeft -= delta2;
                            feedBackUp -= delta2;

                            feedBackRight = 0;
                            feedBackLeft = 0;
                            feedBackUp = 0;

                            if (feedBackRight < 0)
                            {
                                feedBackRight = 0;
                            }

                            if (feedBackLeft < 0)
                            {
                                feedBackLeft = 0;
                            }

                            if (feedBackUp < 0)
                            {
                                feedBackUp = 0;
                            }

                            if (feedBackRight == 0 && feedBackLeft == 0 && feedBackUp == 0)
                            {
                                feedBackDown += delta;
                                if (feedBackDown >= 1000)
                                {
                                    feedBackDown = 1000;
                                }
                            }

                            break;
                        case 3:     //move right
                            feedBackDown -= delta2;
                            feedBackLeft -= delta2;
                            feedBackUp -= delta2;
                            feedBackDown = 0;
                            feedBackLeft = 0;
                            feedBackUp = 0;

                            if (feedBackDown < 0)
                            {
                                feedBackDown = 0;
                            }

                            if (feedBackLeft < 0)
                            {
                                feedBackLeft = 0;
                            }

                            if (feedBackUp < 0)
                            {
                                feedBackUp = 0;
                            }

                            if (feedBackDown == 0 && feedBackLeft == 0 && feedBackUp == 0)
                            {
                                feedBackRight += delta;
                                if (feedBackRight >= 1000)
                                {
                                    feedBackRight = 1000;
                                }
                            }
                            break;

                        case 4:     //move left
                            feedBackDown -= delta2;
                            feedBackRight -= delta2;
                            feedBackUp -= delta2;

                            feedBackDown = 0;
                            feedBackRight = 0;
                            feedBackUp = 0;

                            if (feedBackDown < 0)
                            {
                                feedBackDown = 0;
                            }

                            if (feedBackRight < 0)
                            {
                                feedBackRight = 0;
                            }

                            if (feedBackUp < 0)
                            {
                                feedBackUp = 0;
                            }

                            if (feedBackDown == 0 && feedBackRight == 0 && feedBackUp == 0)
                            {
                                feedBackLeft += delta;
                                if (feedBackLeft >= 1000)
                                {
                                    feedBackLeft = 1000;
                                }
                            }


                            break;
                        case 99:

                            break;
                        default:

                            break;
                    }

                    //set the new command status to false
                    screenManager.Game.Services.RemoveService(typeof(bool));
                    screenManager.Game.Services.AddService(typeof(bool), false);

                }
            }
            catch (Exception e) { }
            base.Update(elapsedTime, totalTime);
        }

         //Draw Game
        public override void Draw(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            spriteBatch.Begin();

            if (feedBackLeft > 0)
            {
                source = new Rectangle(0, 0, 100, feedBackLeft);
                spriteBatch.Draw(Vert, new Vector2(150, (SCREENHEIGHT - source.Height) / 2), source, Color.White);
            }

            if (feedBackRight > 0)
            {
                source = new Rectangle(0, 0, 100, feedBackRight);
                spriteBatch.Draw(Vert, new Vector2(SCREENWIDTH-250, (SCREENHEIGHT - source.Height) / 2), source, Color.White);
            }

            if (feedBackUp > 0)
            {
                source = new Rectangle(0, 0, feedBackUp, 100);
                spriteBatch.Draw(Vert, new Vector2((SCREENWIDTH-source.Width)/2, 150), source, Color.White);
            }

            if (feedBackDown > 0)
            {
                source = new Rectangle(0, 0, feedBackDown, 100);
                spriteBatch.Draw(Vert, new Vector2((SCREENWIDTH - source.Width) / 2, SCREENHEIGHT-250), source, Color.White);
            }

            spriteBatch.End();

            base.Draw(elapsedTime, totalTime);
        }
        
    }
}
