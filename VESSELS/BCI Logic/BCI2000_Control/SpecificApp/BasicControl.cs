using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using VESSELS.BCI_Logic.Device_Emulation;

namespace VESSELS.BCI_Logic.BCI2000_Control.SpecificApp
{
    class BasicControl : LSLcommSOCI
    {
        //Keyboard control vars
        SendTargetedKey sendTargetkey;
        bool keyIsDown = false;
        int keyDownCntr = 0;
        int keyToPullUp = 0;

        //Name of application to control
        string appName;

        //Constructor
        public BasicControl(Game game, string appName)
            :base(game)
        {
            this.appName = appName;
        }

        //Initialize. 
        public override void Initialize()
        {
            //create new targeted keyStroke with name of application to control
            sendTargetkey=new SendTargetedKey(appName);

            base.Initialize();
        }

        //Override this method for the application specific control
        protected override void sendCommand(int output)
        {

            ///Send Key down
            if (!keyIsDown) // if key is up
            {
                switch (output)
                {
                    case 0:     //Do Nothing
                        break;
                    case 1:     //Up arrow (move forward/up)
                        sendTargetkey.SendKeyDown((int)Keys.Up);
                        keyToPullUp = (int)Keys.Up;
                        keyIsDown = true;
                        break;
                    case 2:     //Down arrow (move backward/down)
                        sendTargetkey.SendKeyDown((int)Keys.Down);
                        keyToPullUp = (int)Keys.Down;
                        keyIsDown = true;
                        break;
                    case 3:     //Right arrow (move right)
                        sendTargetkey.SendKeyDown((int)Keys.Right);
                        keyToPullUp = (int)Keys.Right;
                        keyIsDown = true;
                        break;
                    case 4:     //Left  arrow (move left)
                        sendTargetkey.SendKeyDown((int)Keys.Left);
                        keyToPullUp = (int)Keys.Left;
                        keyIsDown = true;
                        break;
                }
            }
            else if (keyIsDown)
            {
                keyDownCntr++;
                if (keyDownCntr == 2)
                {
                    keyIsDown = false;
                    sendTargetkey.SendKeyUp(keyToPullUp);
                    keyDownCntr = 0;
                }
            }
            base.sendCommand(output);
        }


    }
}
