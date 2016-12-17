using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VESSELS.BCI_Logic.Device_Emulation;

namespace VESSELS.SOCI.DirectX_Overlay
{
    class SSVEP_DirectX_Advanced_V2b : SSVEP_DirectX2
    {

        #region Fields
        //Shape helper
        DirectX_Shape_Helper shapeHelper;

        //DX to GDI interface
        DXGDI_Interface dxInterface;

        //Stimulus Struct
        StimStruct[] stimStruct = new StimStruct[4];

        //xna form
        System.Windows.Forms.Form form;

        //Stimuli Customization Variables
        bool mouseDown = false;
        bool alreadyPressed = false;
        bool keyPressed = false;
        Point prevLoc, offset;
        int totalShapes = Enum.GetNames(typeof(Shape)).Length;
  
        #endregion Fields

        //Constructor
        public SSVEP_DirectX_Advanced_V2b(Game game)
            :base(game)
        {   
        }

        //Initialize
        public override void Initialize(System.Windows.Forms.Form form,SpriteBatch spriteBatch)
        {
            //initialize DX to GDI interface
            dxInterface = new DXGDI_Interface(form);

            //Load Images
            shapeHelper = new DirectX_Shape_Helper(Game.Content);

            //Initialize Shapes (get bounds and images)
            for (int i = 0; i < 4; i++)
            {
                stimStruct[i].shape = Shape.Rectangle;
                stimStruct[i].type = (Type)i;
                stimStruct[i].stimScale = 1;
                stimStruct[i] = shapeHelper.GetShape(stimStruct[i]);
            }

            ////set up the Initial stimuli locations
            stimStruct[0].stimRect.Location = new Point((SCREENWIDTH - stimStruct[0].stimRect.Width) / 2, 0);                                             //Up
            stimStruct[1].stimRect.Location = new Point((SCREENWIDTH - stimStruct[1].stimRect.Width) / 2, SCREENHEIGHT - stimStruct[1].stimRect.Height); //Down
            stimStruct[2].stimRect.Location = new Point((SCREENWIDTH - stimStruct[2].stimRect.Width), (SCREENHEIGHT - stimStruct[2].stimRect.Height) / 2); //Right
            stimStruct[3].stimRect.Location = new Point(0, (SCREENHEIGHT - stimStruct[3].stimRect.Height) / 2);                                          //Left

            //Initialize form background rectangles
            dxInterface.UpdateFormImages(stimStruct);   

            ////Event Handlers
            this.form = form;
            form.MouseDown += onMouseDown;
            form.MouseMove += onMouseMove;
            form.MouseUp += onMouseUp;
            form.MouseClick += onMouseClick;
            form.MouseDoubleClick += onMouseDoubleClick;
            form.MouseWheel += onMouseScroll;
            
            //module to intercept any keypresses
            InterceptKeys.KeyDown += handleKeyDown;
            InterceptKeys.KeyUp += handleKeyUp;

            base.Initialize(form,spriteBatch);
        }

        //Handle mouse wheel
        private void onMouseScroll(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (mouseDown)
            {
                if (stimStruct[0].stimRect.Contains(new Point(e.X,e.Y)))
                {
                    if (e.Delta > 0)
                    {
                        stimStruct[0].stimScale += 0.05;
                        stimStruct[0] = shapeHelper.GetShape(stimStruct[0]);
                    }
                    else if (e.Delta < 0)
                    {
                        stimStruct[0].stimScale -= 0.05;
                        stimStruct[0] = shapeHelper.GetShape(stimStruct[0]);
                    }
                }
                else if (stimStruct[1].stimRect.Contains(new Point(e.X,e.Y)))
                {
                    if (e.Delta > 0)
                    {
                        stimStruct[1].stimScale += 0.05;
                        stimStruct[1] = shapeHelper.GetShape(stimStruct[1]);
                    }
                    else if (e.Delta < 0)
                    {
                        stimStruct[1].stimScale -= 0.05;
                        stimStruct[1] = shapeHelper.GetShape(stimStruct[1]);
                    }
                }
                else if (stimStruct[2].stimRect.Contains(new Point(e.X,e.Y)))
                {
                    if (e.Delta > 0)
                    {
                        stimStruct[2].stimScale += 0.05;
                        stimStruct[2] = shapeHelper.GetShape(stimStruct[2]);
                    }
                    else if (e.Delta < 0)
                    {
                        stimStruct[2].stimScale -= 0.05;
                        stimStruct[2] = shapeHelper.GetShape(stimStruct[2]);
                    }
                }
                else if (stimStruct[3].stimRect.Contains(new Point(e.X,e.Y)))
                {
                    if (e.Delta > 0)
                    {
                        stimStruct[3].stimScale += 0.05;
                        stimStruct[3] = shapeHelper.GetShape(stimStruct[3]);
                    }
                    else if (e.Delta < 0)
                    {
                        stimStruct[3].stimScale-= 0.05;
                        stimStruct[3] = shapeHelper.GetShape(stimStruct[3]);
                    }
                }
            }
            else
            {
                if (e.Delta > 0)
                {
                    stimStruct[0].stimScale += 0.05;
                    stimStruct[0] = shapeHelper.GetShape(stimStruct[0]);
                    stimStruct[1].stimScale+= 0.05;
                    stimStruct[1] = shapeHelper.GetShape(stimStruct[1]);
                    stimStruct[2].stimScale += 0.05;
                    stimStruct[2] = shapeHelper.GetShape(stimStruct[2]);
                    stimStruct[3].stimScale += 0.05;
                    stimStruct[3] = shapeHelper.GetShape(stimStruct[3]);
                }
                else if (e.Delta < 0)
                {
                    stimStruct[0].stimScale -= 0.05;
                    stimStruct[0] = shapeHelper.GetShape(stimStruct[0]);
                    stimStruct[1].stimScale -= 0.05;
                    stimStruct[1] = shapeHelper.GetShape(stimStruct[1]);
                    stimStruct[2].stimScale -= 0.05;
                    stimStruct[2] = shapeHelper.GetShape(stimStruct[2]);
                    stimStruct[3].stimScale -= 0.05;
                    stimStruct[3] = shapeHelper.GetShape(stimStruct[3]);
                }
            }

            //Update background rectangles
            dxInterface.UpdateFormImages(stimStruct);
        }

        //Handle mouse double clicks
        private void onMouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                stimStruct[0].shape = (Shape)(((int)stimStruct[0].shape + 1) % totalShapes);
                stimStruct[0] = shapeHelper.GetShape(stimStruct[0]);
                stimStruct[1].shape = (Shape)(((int)stimStruct[1].shape + 1) % totalShapes);
                stimStruct[1] = shapeHelper.GetShape(stimStruct[1]);
                stimStruct[2].shape = (Shape)(((int)stimStruct[2].shape + 1) % totalShapes);
                stimStruct[2] = shapeHelper.GetShape(stimStruct[2]);
                stimStruct[3].shape = (Shape)(((int)stimStruct[3].shape + 1) % totalShapes);
                stimStruct[3] = shapeHelper.GetShape(stimStruct[3]);

                //Update background rectangles
                dxInterface.UpdateFormImages(stimStruct);
            }
        }

        //Handle Mouse Clicks
        private void onMouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (stimStruct[0].stimRect.Contains(new Point(e.X,e.Y)))
                {
                    stimStruct[0].shape = (Shape)(((int)stimStruct[0].shape + 1) % totalShapes);
                    stimStruct[0] = shapeHelper.GetShape(stimStruct[0]);
                }
                else if (stimStruct[1].stimRect.Contains(new Point(e.X, e.Y)))
                {
                    stimStruct[1].shape = (Shape)(((int)stimStruct[1].shape + 1) % totalShapes);
                    stimStruct[1] = shapeHelper.GetShape(stimStruct[1]);
                }
                else if (stimStruct[2].stimRect.Contains(new Point(e.X, e.Y)))
                {
                    stimStruct[2].shape = (Shape)(((int)stimStruct[2].shape + 1) % totalShapes);
                    stimStruct[2] = shapeHelper.GetShape(stimStruct[2]);
                }
                else if (stimStruct[3].stimRect.Contains(new Point(e.X, e.Y)))
                {
                    stimStruct[3].shape = (Shape)(((int)stimStruct[3].shape + 1) % totalShapes);
                    stimStruct[3] = shapeHelper.GetShape(stimStruct[3]);
                }

                //Update background rectangles
                dxInterface.UpdateFormImages(stimStruct);
           }
       }

        //Handle mouse movement
        private void onMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (mouseDown && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (stimStruct[0].stimRect.Contains(new Point(e.X, e.Y)))
                {
                    offset.X = e.X - prevLoc.X;
                    offset.Y = e.Y - prevLoc.Y;
                    prevLoc.X = e.X; prevLoc.Y = e.Y;
                    stimStruct[0].stimRect.Offset(offset);
                }
                else if (stimStruct[1].stimRect.Contains(new Point(e.X, e.Y)))
                {
                    offset.X = e.X - prevLoc.X;
                    offset.Y = e.Y - prevLoc.Y;
                    prevLoc.X = e.X; prevLoc.Y = e.Y;
                    stimStruct[1].stimRect.Offset(offset);
                }
                else if (stimStruct[2].stimRect.Contains(new Point(e.X, e.Y)))
                {
                    offset.X = e.X - prevLoc.X;
                    offset.Y = e.Y - prevLoc.Y;
                    prevLoc.X = e.X; prevLoc.Y = e.Y;
                    stimStruct[2].stimRect.Offset(offset);
                }
                else if (stimStruct[3].stimRect.Contains(new Point(e.X, e.Y)))
                {
                    offset.X = e.X - prevLoc.X;
                    offset.Y = e.Y - prevLoc.Y;
                    prevLoc.X = e.X; prevLoc.Y = e.Y;
                    stimStruct[3].stimRect.Offset(offset);
                }

                //Update back rectangle positionss
                dxInterface.UpdateFormImages(stimStruct);
           }
       }

        //handle mouse press
        private void onMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseDown = true;
            prevLoc.X = e.X; prevLoc.Y = e.Y;
        }

        //Handle mouse release
        private void onMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseDown = false;
        }


        //Handle key presses
        private void handleKeyUp(object sender, EventArgs e)
        {
            alreadyPressed = false;
        }

        //Handle key presses 
        private void handleKeyDown(object sender, EventArgs e)
        {
            System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)sender;

                if (key == System.Windows.Forms.Keys.F4 && alreadyPressed==false)
                {
                    keyPressed = true;
                    alreadyPressed = true;
                }
                else if (key == System.Windows.Forms.Keys.Escape)
                {
                    Game.Exit();
                }
        }

        //Update
        public override void Update(GameTime gameTime)
        {
            if (keyPressed)
            {
                flashing ^= true;

                if (flashing)
                {
                    form.MouseDown -= onMouseDown;
                    form.MouseMove -= onMouseMove;
                    form.MouseUp -= onMouseUp;
                    form.MouseClick -= onMouseClick;
                    form.MouseDoubleClick -= onMouseDoubleClick;
                    form.MouseWheel -= onMouseScroll;
                }
                else
                {
                    form.MouseDown += onMouseDown;
                    form.MouseMove += onMouseMove;
                    form.MouseUp += onMouseUp;
                    form.MouseClick += onMouseClick;
                    form.MouseDoubleClick += onMouseDoubleClick;
                    form.MouseWheel += onMouseScroll;
                }

                keyPressed = false;
            }
            //else if (oldKs.IsKeyDown(Keys.R) && Keyboard.GetState().IsKeyUp(Keys.R))
            //{   //reset stimuli to default position, size and shape
            //    totalReset();
            //}
            if (flashing)
            {
                //Update stimuli states
                UpdateStimulus();
            }
            else
            {
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (flashing)
            {
                DrawStimuli();
            }
            else
            {
                offState();
            }
            base.Draw(gameTime);
        }

        //Draw FlashingStimli
        private void DrawStimuli()
        {
            //Begin draw
            spriteBatch.Begin();

           
            if (StimType2 == 1)
            {
                spriteBatch.Draw(stimStruct[1].texture, stimStruct[1].stimRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(stimStruct[1].texture, stimStruct[1].stimRect, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
           

            //End draw
            spriteBatch.End();
        }


        //Reset stimuli to black flash state
        private void offState()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(stimStruct[0].texture, stimStruct[0].stimRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(stimStruct[1].texture, stimStruct[1].stimRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(stimStruct[2].texture, stimStruct[2].stimRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(stimStruct[3].texture, stimStruct[3].stimRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        //Reset stimuli to stim defaults
        private void totalReset()
        {
        }


        protected override void Dispose(bool disposing)
        {
            InterceptKeys.KeyDown -= handleKeyDown;
            InterceptKeys.KeyUp -= handleKeyUp;
            base.Dispose(disposing);
        }
    }
}
