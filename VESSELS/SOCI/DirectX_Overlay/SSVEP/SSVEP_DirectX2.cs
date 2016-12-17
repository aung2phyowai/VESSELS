using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;


namespace VESSELS.SOCI.DirectX_Overlay
{
    class SSVEP_DirectX2 : DrawableGameComponent
    {
        #region Fields

        //DirectX spritebatch
        protected SpriteBatch spriteBatch;

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

        //flashing On/off control
        protected bool flashing = false;

        //Screen Dimensions
        protected int SCREENHEIGHT;
        protected int SCREENWIDTH;

        #endregion Fields

        //Constructor
        public SSVEP_DirectX2(Game game)
            : base(game)
        {
            SCREENHEIGHT = game.GraphicsDevice.DisplayMode.Height;
            SCREENWIDTH = game.GraphicsDevice.DisplayMode.Width;
        }

        //Initialize
        public virtual void Initialize(System.Windows.Forms.Form form,SpriteBatch spriteBatch)
        {   
                //8 hz frequency with 60hz refresh rate
                s2p1On = 3; s2p1Off = 4; s2p2On = 4; s2p2Off = 4; //8Hz

            this.spriteBatch = spriteBatch;   
        }

        /// <summary>
        /// Updates the states of the stimuli
        /// </summary>
        public void UpdateStimulus()
        {
            //update the counters
            Cntr2++; 
           
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
        }

    }
}
