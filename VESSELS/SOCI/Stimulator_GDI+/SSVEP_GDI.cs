using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace VESSELS.BCI_Logic.StimulatorGDI
{

    //Base class for SSVEP stimulation
    class SSVEP_GDI : GameComponent
    {
        #region Fields
        
        //Stimulus tracker variables
        int State = 1; int Cntr1 = 0;
        int State2 = 1; int Cntr2 = 0;
        int State3 = 1; int Cntr3 = 0;
        int State4 = 1; int Cntr4 = 0;
        protected int StimType = 2; protected int StimType2 = 2; protected int StimType3 = 2; protected int StimType4 = 2;

        //stimulus period, for variable frequencies
        protected int period1 = 1;
        protected int period2 = 1;
        protected int period3 = 1;
        protected int period4 = 1;

        // Frequency counters (set for frequency of stimuli)  FOR 30 HZ Variable
        protected int s1p1On = 1; protected int s1p1Off = 2; protected int s1p2On = 2; protected int s1p2Off = 2; //8.5Hz
        protected int s2p1On = 2; protected int s2p1Off = 3; protected int s2p2On = 2; protected int s2p2Off = 3; //6Hz
        protected int s3p1On = 2; protected int s3p1Off = 2; protected int s3p2On = 2; protected int s3p2Off = 2; //7.5Hz
        protected int s4p1On = 2; protected int s4p1Off = 2; protected int s4p2On = 2; protected int s4p2Off = 3; //6.667Hz

        //Screen properties
        protected int SCREENWIDTH, SCREENHEIGHT;

        //Forms Drawing
        protected System.Windows.Forms.Form form;

        //GPU graphics back buffers
        protected System.Drawing.BufferedGraphics backBuffer;
        protected System.Drawing.BufferedGraphicsContext currenContext;
        
        //Forms On/off control box
        System.Windows.Forms.CheckBox checkBox = new System.Windows.Forms.CheckBox();
        protected bool flashing = false;

        #endregion Fields

        //Constructor
        public SSVEP_GDI(Game game)
            : base(game)
        {
            this.SCREENHEIGHT = game.GraphicsDevice.DisplayMode.Height;
            this.SCREENWIDTH = game.GraphicsDevice.DisplayMode.Width;
        }

        //Initialize
        public virtual void Initialize(System.Windows.Forms.Form form)
        {   //Windows form
            this.form = form;
            // set up graphics device
            System.Drawing.Graphics graph = form.CreateGraphics();
            graph.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            graph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            graph.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            //Set up buffer 
            currenContext = System.Drawing.BufferedGraphicsManager.Current;
            backBuffer = currenContext.Allocate(graph, form.DisplayRectangle);
            backBuffer.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            backBuffer.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            backBuffer.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            backBuffer.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            backBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;            
                

            //Set frequencies based on refresh rate (60 or 30);
            if (Game.TargetElapsedTime.TotalMilliseconds > 30)
            {   //30Hz
                s1p1On = 1; s1p1Off = 2; s1p2On = 2; s1p2Off = 2; //8.5Hz
                s2p1On = 2; s2p1Off = 3; s2p2On = 2; s2p2Off = 3; //6Hz
                s3p1On = 2; s3p1Off = 2; s3p2On = 2; s3p2Off = 2; //7.5Hz
                s4p1On = 2; s4p1Off = 2; s4p2On = 2; s4p2Off = 3; //6.667Hz
            }
            else
            {   //60Hz
                s1p1On = 3; s1p1Off = 4; s1p2On = 3; s1p2Off = 4; //8.5Hz
                s2p1On = 5; s2p1Off = 5; s2p2On = 5; s2p2Off = 5; //6Hz
                s3p1On = 4; s3p1Off = 4; s3p2On = 4; s3p2Off = 4; //7.5Hz
                s4p1On = 4; s4p1Off = 5; s4p2On = 4; s4p2Off = 5; //6.667Hz
            }
        }

        /// <summary>
        /// Updates the states of the stimuli
        /// </summary>
        public void UpdateStimulus()
        {
            //update the counters
            Cntr1++; Cntr2++;
            Cntr3++; Cntr4++;
            //****************************
            // ------ STIMULI 1 -------
            //****************************
            if (period1 == 1)
            {
                //Update the 1st stimulus 
                if (Cntr1 == s1p1On && State == 1)
                {
                    StimType = 1;
                    Cntr1 = 0;
                    State = 0;
                }
                else if (Cntr1 == s1p1Off && State == 0)
                {
                    StimType = 2;
                    Cntr1 = 0;
                    State = 1;
                    period1 = 2;
                }
            }
            else
            {
                //Update the 1st stimulus 
                if (Cntr1 == s1p2On && State == 1)
                {
                    StimType = 1;
                    Cntr1 = 0;
                    State = 0;
                }
                else if (Cntr1 == s2p1Off && State == 0)
                {
                    StimType = 2;
                    Cntr1 = 0;
                    State = 1;
                    period1 = 1;
                }
            }
            //****************************
            // ------ STIMULI 2 -------
            //****************************
            if (period2 == 1)
            {
                //Update the 2nd stimulus 
                if (Cntr2 == s2p1On && State2 == 1)
                {
                    StimType2 = 1;
                    Cntr2 = 0;
                    State2 = 0;
                }
                else if (Cntr2 == s2p1Off && State2 == 0)
                {
                    StimType2 = 2;
                    Cntr2 = 0;
                    State2 = 1;
                    period2 = 2;
                }
            }
            else
            {
                //Update the 2nd stimulus 
                if (Cntr2 == s2p2On && State2 == 1)
                {
                    StimType2 = 1;
                    Cntr2 = 0;
                    State2 = 0;
                }
                else if (Cntr2 == s2p2Off && State2 == 0)
                {
                    StimType2 = 2;
                    Cntr2 = 0;
                    State2 = 1;
                    period2 = 1;
                }
            }
            //****************************
            // ------ STIMULI 3 -------
            //****************************
            if (period3 == 1)
            {
                //Update the third stimulus 
                if (Cntr3 == s3p1On && State3 == 1)
                {
                    StimType3 = 1;
                    Cntr3 = 0;
                    State3 = 0;
                }
                else if (Cntr3 == s3p1Off && State3 == 0)
                {
                    StimType3 = 2;
                    Cntr3 = 0;
                    State3 = 1;
                    period3 = 2;
                }
            }
            else
            {
                //Update the third stimulus 
                if (Cntr3 == s3p2On && State3 == 1)
                {
                    StimType3 = 1;
                    Cntr3 = 0;
                    State3 = 0;
                }
                else if (Cntr3 == s3p2Off && State3 == 0)
                {
                    StimType3 = 2;
                    Cntr3 = 0;
                    State3 = 1;
                    period3 = 1;
                }
            }
            //****************************
            // ------ STIMULI 4 -------
            //****************************
            if (period4 == 1)
            {
                //Update the fourth stimulus 
                if (Cntr4 == s4p1On && State4 == 1)
                {
                    StimType4 = 1;
                    Cntr4 = 0;
                    State4 = 0;
                }
                else if (Cntr4 == s4p1Off && State4 == 0)
                {
                    StimType4 = 2;
                    Cntr4 = 0;
                    State4 = 1;
                    period4 = 2;
                }
            }
            else
            {
                //Update the fourth stimulus 
                if (Cntr4 == s4p2On && State4 == 1)
                {
                    StimType4 = 1;
                    Cntr4 = 0;
                    State4 = 0;
                }
                else if (Cntr4 == s4p2Off && State4 == 0)
                {
                    StimType4 = 2;
                    Cntr4 = 0;
                    State4 = 1;
                    period4 = 1;
                }
            }
        }

        //Clear gpu buffer memory
        protected override void Dispose(bool disposing)
        {
            try
            {
                backBuffer.Dispose();
                currenContext.Dispose();
            }
            catch (Exception) { }

            base.Dispose(disposing);
        }

    }
}
