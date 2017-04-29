using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using VESSELS.BCI_Logic.Device_Emulation;

namespace VESSELS.BCI_Logic.BCI2000_Control.SpecificApp
{
    class SpecificAppControl : LSLcommSOCI
    {
        //Keyboard control vars
        SendTargetedKey sendTargetkey;
        bool keyIsDown = false;
        int keyDownCntr = 0;
        int keyToPullUp = 0;

        //Name of application to control
        string appName;

        //Constructor
        public SpecificAppControl(Game game, string appName)
            : base(game)
        {
            this.appName = appName;
        }

        //Initialize. 
        public override void Initialize()
        {
            //create new targeted keyStroke with name of application to control
            sendTargetkey = new SendTargetedKey(appName);

            base.Initialize();
        }

        //Override this method for the application specific control
        protected override void sendCommand(int output)
        {
            switch (output)
            {
                case 0:     //Do Nothing
                    sendTargetkey.SendKeyUp((int)Keys.Down);
                    sendTargetkey.SendKeyUp((int)Keys.Right);
                    sendTargetkey.SendKeyUp((int)Keys.Left);
                    sendTargetkey.SendKeyUp((int)Keys.Up);
                    break;
                case 1:     //Up arrow (move forward/up)
                    sendTargetkey.SendKeyUp((int)Keys.Down);
                    sendTargetkey.SendKeyUp((int)Keys.Right);
                    sendTargetkey.SendKeyUp((int)Keys.Left);
                    sendTargetkey.SendKeyDown((int)Keys.Up);
                    //Console.WriteLine("Up");
                    break;
                case 2:     //Down arrow (move backward/down)
                    sendTargetkey.SendKeyUp((int)Keys.Up);
                    sendTargetkey.SendKeyUp((int)Keys.Right);
                    sendTargetkey.SendKeyUp((int)Keys.Left);
                    sendTargetkey.SendKeyDown((int)Keys.Down);
                    //Console.WriteLine("Down");
                    break;
                case 3:     //Right arrow (move right)
                    sendTargetkey.SendKeyUp((int)Keys.Down);
                    sendTargetkey.SendKeyUp((int)Keys.Up);
                    sendTargetkey.SendKeyUp((int)Keys.Left);
                    sendTargetkey.SendKeyDown((int)Keys.Right);
                    //Console.WriteLine("Right");
                    break;
                case 4:     //Left  arrow (move left)
                    sendTargetkey.SendKeyUp((int)Keys.Down);
                    sendTargetkey.SendKeyUp((int)Keys.Right);
                    sendTargetkey.SendKeyUp((int)Keys.Up);
                    sendTargetkey.SendKeyDown((int)Keys.Left);
                    //Console.WriteLine("Left");
                    break;
                default:
                    sendTargetkey.SendKeyUp((int)Keys.Down);
                    sendTargetkey.SendKeyUp((int)Keys.Right);
                    sendTargetkey.SendKeyUp((int)Keys.Left);
                    sendTargetkey.SendKeyUp((int)Keys.Up);
                    break;

            }
            base.sendCommand(output);
        }

    }
}
