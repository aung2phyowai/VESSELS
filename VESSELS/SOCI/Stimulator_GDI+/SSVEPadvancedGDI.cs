using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace VESSELS.BCI_Logic.StimulatorGDI
{
    class SSVEPadvancedGDI : SSVEP_GDI
    {
        #region Fields
        StimuliShapes_GDI stimShapes;
        System.Drawing.Rectangle[] rect = new System.Drawing.Rectangle[4];
        System.Drawing.Bitmap[] shape = new System.Drawing.Bitmap[8];
        StimStruct[] stimStruct = new StimStruct[4];
        int totalShapes = Enum.GetNames(typeof(Shape)).Length;

        KeyboardState oldKs;
        bool mouseDown = false;
        System.Drawing.Point offset;
        System.Drawing.Point prevLoc;

        #endregion Fields
        Game game;
        //Constructor
        public SSVEPadvancedGDI(Game game)
            :base(game)
        {
            this.game = game;
        }

        //Initialize
        public override void Initialize(System.Windows.Forms.Form form)
        {
            //Load Bitmap Images
            stimShapes = new StimuliShapes_GDI(form);
            
            //Initialize Shapes (get bounds and images)
            for (int i = 0; i < 4; i++)
            {
                stimStruct[i].shape = Shape.Rectangle;
                stimStruct[i].type =  (Type)i;
                stimStruct[i].sizeOffset = 1;
                stimStruct[i] = stimShapes.GetBoundingRects(stimStruct[i]);
            }

            //set up the Initial stimuli locations
            stimStruct[0].stimRect.Location = new System.Drawing.Point((SCREENWIDTH - stimStruct[0].stimRect.Width) / 2, 0);                                             //Up
            stimStruct[1].stimRect.Location = new System.Drawing.Point((SCREENWIDTH - stimStruct[1].stimRect.Width) / 2, SCREENHEIGHT - stimStruct[1].stimRect.Height); //Down
            stimStruct[2].stimRect.Location = new System.Drawing.Point(SCREENWIDTH - stimStruct[2].stimRect.Width, (SCREENHEIGHT - stimStruct[2].stimRect.Height) / 2); //Right
            stimStruct[3].stimRect.Location = new System.Drawing.Point(0, (SCREENHEIGHT - stimStruct[3].stimRect.Height) / 2);                                          //Left
            
            //Event Handlers
            form.MouseDown += onMouseDown;
            form.MouseMove += onMouseMove;
            form.MouseUp += onMouseUp;
            form.MouseClick += onMouseClick;
            form.MouseDoubleClick += onMouseDoubleClick;
            form.MouseWheel += onMouseScroll;
            
            base.Initialize(form);
        }

        //Handle mouse wheel
        private void onMouseScroll(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (mouseDown)
            {
                if (stimStruct[0].stimRect.Contains(e.Location))
                {
                    if (e.Delta > 0)
                    {
                        stimStruct[0].sizeOffset += 0.05;
                        stimStruct[0] = stimShapes.GetBoundingRects(stimStruct[0]);
                    }
                    else if (e.Delta < 0)
                    {
                        stimStruct[0].sizeOffset -= 0.05;
                        stimStruct[0] = stimShapes.GetBoundingRects(stimStruct[0]);
                    }
                }
                else if (stimStruct[1].stimRect.Contains(e.Location))
                {
                    if (e.Delta > 0)
                    {
                        stimStruct[1].sizeOffset += 0.05;
                        stimStruct[1] = stimShapes.GetBoundingRects(stimStruct[1]);
                    }
                    else if (e.Delta < 0)
                    {
                        stimStruct[1].sizeOffset -= 0.05;
                        stimStruct[1] = stimShapes.GetBoundingRects(stimStruct[1]);
                    }
                }
                else if (stimStruct[2].stimRect.Contains(e.Location))
                {
                    if (e.Delta > 0)
                    {
                        stimStruct[2].sizeOffset += 0.05;
                        stimStruct[2] = stimShapes.GetBoundingRects(stimStruct[2]);
                    }
                    else if (e.Delta < 0)
                    {
                        stimStruct[2].sizeOffset -= 0.05;
                        stimStruct[2] = stimShapes.GetBoundingRects(stimStruct[2]);
                    }
                }
                else if (stimStruct[3].stimRect.Contains(e.Location))
                {
                    if (e.Delta > 0)
                    {
                        stimStruct[3].sizeOffset += 0.05;
                        stimStruct[3] = stimShapes.GetBoundingRects(stimStruct[3]);
                    }
                    else if (e.Delta < 0)
                    {
                        stimStruct[3].sizeOffset -= 0.05;
                        stimStruct[3] = stimShapes.GetBoundingRects(stimStruct[3]);
                    }
                } 
            }
            else
            {
                if (e.Delta > 0)
                {
                    stimStruct[0].sizeOffset += 0.05;
                    stimStruct[0] = stimShapes.GetBoundingRects(stimStruct[0]);
                    stimStruct[1].sizeOffset += 0.05;
                    stimStruct[1] = stimShapes.GetBoundingRects(stimStruct[1]);
                    stimStruct[2].sizeOffset += 0.05;
                    stimStruct[2] = stimShapes.GetBoundingRects(stimStruct[2]);
                    stimStruct[3].sizeOffset += 0.05;
                    stimStruct[3] = stimShapes.GetBoundingRects(stimStruct[3]);
                }
                else if (e.Delta < 0)
                {
                    stimStruct[0].sizeOffset -= 0.05;
                    stimStruct[0] = stimShapes.GetBoundingRects(stimStruct[0]);
                    stimStruct[1].sizeOffset -= 0.05;
                    stimStruct[1] = stimShapes.GetBoundingRects(stimStruct[1]);
                    stimStruct[2].sizeOffset -= 0.05;
                    stimStruct[2] = stimShapes.GetBoundingRects(stimStruct[2]);
                    stimStruct[3].sizeOffset -= 0.05;
                    stimStruct[3] = stimShapes.GetBoundingRects(stimStruct[3]);
                }
            }
        }

        //Handle mouse double clicks
        private void onMouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                stimStruct[0].shape = (Shape)(((int)stimStruct[0].shape + 1) % totalShapes);
                stimStruct[0] = stimShapes.GetBoundingRects(stimStruct[0]);
                stimStruct[1].shape = (Shape)(((int)stimStruct[1].shape + 1) % totalShapes);
                stimStruct[1] = stimShapes.GetBoundingRects(stimStruct[1]);
                stimStruct[2].shape = (Shape)(((int)stimStruct[2].shape + 1) % totalShapes);
                stimStruct[2] = stimShapes.GetBoundingRects(stimStruct[2]);
                stimStruct[3].shape = (Shape)(((int)stimStruct[3].shape + 1) % totalShapes);
                stimStruct[3] = stimShapes.GetBoundingRects(stimStruct[3]); 
            }
        }


        //Handle Mouse Clicks
        private void onMouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (stimStruct[0].stimRect.Contains(e.Location))
                {
                    stimStruct[0].shape = (Shape)(((int)stimStruct[0].shape + 1) % totalShapes);
                    stimStruct[0] = stimShapes.GetBoundingRects(stimStruct[0]);
                }
                else if (stimStruct[1].stimRect.Contains(e.Location))
                {
                    stimStruct[1].shape = (Shape)(((int)stimStruct[1].shape + 1) % totalShapes);
                    stimStruct[1] = stimShapes.GetBoundingRects(stimStruct[1]);
                }
                else if (stimStruct[2].stimRect.Contains(e.Location))
                {
                    stimStruct[2].shape = (Shape)(((int)stimStruct[2].shape + 1) % totalShapes);
                    stimStruct[2] = stimShapes.GetBoundingRects(stimStruct[2]);
                }
                else if (stimStruct[3].stimRect.Contains(e.Location))
                {
                    stimStruct[3].shape = (Shape)(((int)stimStruct[3].shape + 1) % totalShapes);
                    stimStruct[3] = stimShapes.GetBoundingRects(stimStruct[3]);
                } 
            }
        }

        //Handle mouse movement
        private void onMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (mouseDown && e.Button== System.Windows.Forms.MouseButtons.Left)
            {
                if (stimStruct[0].stimRect.Contains(e.Location))
                {
                    offset.X=e.X-prevLoc.X;
                    offset.Y=e.Y-prevLoc.Y;
                    prevLoc = e.Location;
                    stimStruct[0].stimRect.Offset(offset);
                }
                else if (stimStruct[1].stimRect.Contains(e.Location))
                {
                    offset.X = e.X - prevLoc.X;
                    offset.Y = e.Y - prevLoc.Y;
                    prevLoc = e.Location;
                    stimStruct[1].stimRect.Offset(offset);
                }
                else if (stimStruct[2].stimRect.Contains(e.Location))
                {
                    offset.X = e.X - prevLoc.X;
                    offset.Y = e.Y - prevLoc.Y;
                    prevLoc = e.Location;
                    stimStruct[2].stimRect.Offset(offset);
                }
                else if (stimStruct[3].stimRect.Contains(e.Location))
                {
                    offset.X = e.X - prevLoc.X;
                    offset.Y = e.Y - prevLoc.Y;
                    prevLoc = e.Location;
                    stimStruct[3].stimRect.Offset(offset);
                }
            }
        }

        //handle mouse click
        private void onMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseDown = true;
            prevLoc = e.Location;
        }
        //Handle mouse release
        private void onMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseDown = false;
        }

        //Update
        public override void Update(GameTime gameTime)
        {
            if (oldKs.IsKeyDown(Keys.Enter) && Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                flashing ^= true;

                if (flashing)
                {
                    form.MouseDown -= onMouseDown;
                    form.MouseMove -= onMouseMove;
                    form.MouseUp -= onMouseUp;
                    form.MouseClick -= onMouseClick;
                    form.MouseDoubleClick -= onMouseDoubleClick;
                }
                else
                {
                    form.MouseDown += onMouseDown;
                    form.MouseMove += onMouseMove;
                    form.MouseUp += onMouseUp;
                    form.MouseClick += onMouseClick;
                    form.MouseDoubleClick += onMouseDoubleClick;
                }
            }
            else if (oldKs.IsKeyDown(Keys.R) && Keyboard.GetState().IsKeyUp(Keys.R))
            {   //reset stimuli to default position, size and shape
                totalReset();
            }
            if (flashing)
            {
                //Update stimuli states
                UpdateStimulus();
                //Draw stimuli into buffer
                draw();
                //Render buffer
                backBuffer.Render();
            }
            else
            {
                reset();
            }
            oldKs = Keyboard.GetState();
            base.Update(gameTime);
        }

        //Render stimuli using buffered gpu memory
        private void draw()
        {
            if (StimType == 1)
            {
                backBuffer.Graphics.DrawImageUnscaled(stimStruct[0].bmpOn, stimStruct[0].stimRect);
            }
            else
            {
                backBuffer.Graphics.DrawImageUnscaled(stimStruct[0].bmpOff, stimStruct[0].stimRect);
            }
            if (StimType2 == 1)
            {
                backBuffer.Graphics.DrawImageUnscaled(stimStruct[1].bmpOn, stimStruct[1].stimRect);
            }
            else
            {
                backBuffer.Graphics.DrawImageUnscaled(stimStruct[1].bmpOff, stimStruct[1].stimRect);
            }
            if (StimType3 == 1)
            {
                backBuffer.Graphics.DrawImageUnscaled(stimStruct[2].bmpOn, stimStruct[2].stimRect);
            }
            else
            {
                backBuffer.Graphics.DrawImageUnscaled(stimStruct[2].bmpOff, stimStruct[2].stimRect);
            }
            if (StimType4 == 1)
            {
                backBuffer.Graphics.DrawImageUnscaled(stimStruct[3].bmpOn, stimStruct[3].stimRect);
            }
            else
            {
                backBuffer.Graphics.DrawImageUnscaled(stimStruct[3].bmpOff, stimStruct[3].stimRect);
            }
        }

        //Reset the image buffer
        private void reset()
        {
            backBuffer.Graphics.Clear(System.Drawing.Color.Black);
            backBuffer.Graphics.DrawImageUnscaled(stimStruct[0].bmpOn, stimStruct[0].stimRect);
            backBuffer.Graphics.DrawImageUnscaled(stimStruct[1].bmpOn, stimStruct[1].stimRect);
            backBuffer.Graphics.DrawImageUnscaled(stimStruct[2].bmpOn, stimStruct[2].stimRect);
            backBuffer.Graphics.DrawImageUnscaled(stimStruct[3].bmpOn, stimStruct[3].stimRect);
            backBuffer.Render();
        }

        //reset to stimuli defaults
        private void totalReset()
        {
            for (int i = 0; i < 4; i++)
            {
                stimStruct[i].shape = Shape.Rectangle;
                stimStruct[i].sizeOffset = 1.0;
                stimStruct[i] = stimShapes.GetBoundingRects(stimStruct[i]);
            }
            stimStruct[0].stimRect.Location = new System.Drawing.Point((SCREENWIDTH - stimStruct[0].stimRect.Width) / 2, 0);                                             //Up
            stimStruct[1].stimRect.Location = new System.Drawing.Point((SCREENWIDTH - stimStruct[1].stimRect.Width) / 2, SCREENHEIGHT - stimStruct[1].stimRect.Height); //Down
            stimStruct[2].stimRect.Location = new System.Drawing.Point(SCREENWIDTH - stimStruct[2].stimRect.Width, (SCREENHEIGHT - stimStruct[2].stimRect.Height) / 2); //Right
            stimStruct[3].stimRect.Location = new System.Drawing.Point(0, (SCREENHEIGHT - stimStruct[3].stimRect.Height) / 2);     
        }
    }
}
