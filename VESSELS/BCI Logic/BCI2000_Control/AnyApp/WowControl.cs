using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using VESSELS.BCI_Logic.Device_Emulation;

namespace VESSELS.BCI_Logic.BCI2000_Control.AnyApp
{
    class WowControl : LSLcommSOCI
    {
        //Keyboard control vars
        int keyDownCntr = 5;
        bool turnKeyIsDown = false;
        
        //Constructor
        public WowControl(Game game)
            :base(game)
        {
        }

        //Initialize. 
        public override void Initialize()
        {
            //create new targeted keyStroke with name of application to control
            
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            if (turnKeyIsDown)
            {
                keyDownCntr--;

                if (keyDownCntr == 0)
                {
                    VirtualKeyboard.ReleaseKey(Keys.Left);
                    VirtualKeyboard.ReleaseKey(Keys.Right);
                    keyDownCntr = 5;
                    turnKeyIsDown = false;
                }
            }
            
            base.Update(gameTime);
        }

        //Override this method for the application specific control
        protected override void sendCommand(int output)
        {
                switch (output)
                {
                    case 0:     //Do Nothing
                        break;
                    case 1:     //Up arrow (move forward/up)
                        VirtualKeyboard.HoldKey(Keys.Up);
                        break;
                    case 2:     //Down arrow (move backward/down)
                        //sendTargetkey.SendKeyUp((int)Keys.Up);
                        //sendTargetkey.SendKeyDown((int)Keys.D1);
                        //sendTargetkey.SendKeyUp((int)Keys.D1);
                        //sendTargetkey.SendKeyDown((int)Keys.D1);
                        //sendTargetkey.SendKeyUp((int)Keys.D1);

                        
                        //sendTargetkey.SendKeyDown((int)Keys.D2);
                        //sendTargetkey.SendKeyUp((int)Keys.D2);
                        //sendTargetkey.SendKeyDown((int)Keys.D2);
                        //sendTargetkey.SendKeyUp((int)Keys.D2);
                        break;
                    case 3:     //Right arrow (move right)
                        VirtualKeyboard.ReleaseKey(Keys.Up);
                        VirtualKeyboard.HoldKey(Keys.Right);
                        turnKeyIsDown = true;
                        break;
                    case 4:     //Left  arrow (move left)
                        VirtualKeyboard.ReleaseKey(Keys.Up);
                        VirtualKeyboard.HoldKey(Keys.Left);
                        turnKeyIsDown = true;
                        break;
                }
            base.sendCommand(output);
        }


    }
}
