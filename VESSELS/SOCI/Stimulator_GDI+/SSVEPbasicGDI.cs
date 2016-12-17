using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace VESSELS.BCI_Logic.StimulatorGDI
{
    class SSVEPbasicGDI : SSVEP_GDI
    {
          #region Fields
        //Draw with forms
        System.Drawing.SolidBrush brushOn;
        System.Drawing.SolidBrush brushOff;
        System.Drawing.Rectangle stim1;
        System.Drawing.Rectangle stim2;
        System.Drawing.Rectangle stim3;
        System.Drawing.Rectangle stim4;

        //stim shape
        int vertWidth = 128;
        int vertHeight = 320;
        int horizHeight = 128;
        int horizWidth = 320;

        KeyboardState oldKs;
        bool mouseDown = false;
        System.Drawing.Point offset;
        System.Drawing.Point prevLoc;
        #endregion Fields
        Game game;
        //Constructor
        public SSVEPbasicGDI(Game game)
            :base(game)
        {
            this.game = game;
        }

        //Initialize
        public override void Initialize(System.Windows.Forms.Form form)
        {

            //Define drawing brushes
            brushOn = (System.Drawing.SolidBrush)System.Drawing.Brushes.White;
            brushOff = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(1, 1, 1));

            //set up the stim shapes with size: 320 x 128 and positions
            //Top
            stim1 = new System.Drawing.Rectangle((SCREENWIDTH - (horizWidth)) / 2, 0, horizWidth, horizHeight);
            //Botom
            stim2 = new System.Drawing.Rectangle((SCREENWIDTH - (horizWidth)) / 2, SCREENHEIGHT - horizHeight, horizWidth, horizHeight);
            //Right
            stim3 = new System.Drawing.Rectangle((SCREENWIDTH - (vertWidth)), (SCREENHEIGHT - vertHeight) / 2, vertWidth, vertHeight);
            //Left
            stim4 = new System.Drawing.Rectangle(0, (SCREENHEIGHT - vertHeight) / 2, vertWidth, vertHeight);

            //Event Handlers
            form.MouseDown += onMouseDown;
            form.MouseMove += onMouseMove;
            form.MouseUp += onMouseUp;

            base.Initialize(form);
        }

        //Handle mouse movement
        private void onMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (mouseDown && e.Button== System.Windows.Forms.MouseButtons.Left)
            {
                if (stim1.Contains(e.Location))
                {
                    
                    offset.X=e.X-prevLoc.X;
                    offset.Y=e.Y-prevLoc.Y;
                    prevLoc = e.Location;
                    stim1.Offset(offset);
                }
                else if (stim2.Contains(e.Location))
                {
                    offset.X = e.X - prevLoc.X;
                    offset.Y = e.Y - prevLoc.Y;
                    prevLoc = e.Location;
                    stim2.Offset(offset);
                }
                else if (stim3.Contains(e.Location))
                {
                    offset.X = e.X - prevLoc.X;
                    offset.Y = e.Y - prevLoc.Y;
                    prevLoc = e.Location;
                    stim3.Offset(offset);
                }
                else if (stim4.Contains(e.Location))
                {
                    offset.X = e.X - prevLoc.X;
                    offset.Y = e.Y - prevLoc.Y;
                    prevLoc = e.Location;
                    stim4.Offset(offset);
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
                }
                else
                {
                    form.MouseDown += onMouseDown;
                    form.MouseMove += onMouseMove;
                    form.MouseUp += onMouseUp;
                }
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
                backBuffer.Graphics.FillRectangle(brushOn, stim1);
            }
            else
            {
                backBuffer.Graphics.FillRectangle(brushOff, stim1);
            }
            if (StimType2 == 1)
            {
                backBuffer.Graphics.FillRectangle(brushOn, stim2);
            }
            else
            {
                backBuffer.Graphics.FillRectangle(brushOff, stim2);
            }
            if (StimType3 == 1)
            {
                backBuffer.Graphics.FillRectangle(brushOn, stim3);
            }
            else
            {
                backBuffer.Graphics.FillRectangle(brushOff, stim3);
            }
            if (StimType4 == 1)
            {
                backBuffer.Graphics.FillRectangle(brushOn, stim4);
            }
            else
            {
                backBuffer.Graphics.FillRectangle(brushOff, stim4);
            }
        }

        private void reset()
        {
            backBuffer.Graphics.Clear(System.Drawing.Color.Black);
            backBuffer.Graphics.FillRectangle(brushOff, stim1);
            backBuffer.Graphics.FillRectangle(brushOff, stim2);
            backBuffer.Graphics.FillRectangle(brushOff, stim3);
            backBuffer.Graphics.FillRectangle(brushOff, stim4);
            backBuffer.Render();
        }
    }
}
