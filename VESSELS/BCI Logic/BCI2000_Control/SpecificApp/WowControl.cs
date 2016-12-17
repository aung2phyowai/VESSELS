using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using VESSELS.BCI_Logic.Device_Emulation;

namespace VESSELS.BCI_Logic.BCI2000_Control.SpecificApp
{
    class WowControl : BCI2000commSOCI
    {
        //Keyboard control vars
        SendTargetedKey sendTargetkey;
        int keyDownCntr = 5;
        bool turnKeyIsDown = false;

        //Name of application to control
        string appName;

        //Constructor
        public WowControl(Game game, string appName)
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


        public override void Update(GameTime gameTime)
        {
            if (turnKeyIsDown)
            {
                keyDownCntr--;

                if (keyDownCntr == 0)
                {
                    sendTargetkey.SendKeyUp((int)Keys.Right);
                    sendTargetkey.SendKeyUp((int)Keys.Left);
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
                        sendTargetkey.SendKeyDown((int)Keys.Up);
                        break;
                    case 2:     //Down arrow (move backward/down)
                        sendTargetkey.SendKeyUp((int)Keys.Up);
                        sendTargetkey.SendKeyDown((int)Keys.D1);
                        sendTargetkey.SendKeyUp((int)Keys.D1);
                        sendTargetkey.SendKeyDown((int)Keys.D1);
                        sendTargetkey.SendKeyUp((int)Keys.D1);

                        
                        sendTargetkey.SendKeyDown((int)Keys.D2);
                        sendTargetkey.SendKeyUp((int)Keys.D2);
                        sendTargetkey.SendKeyDown((int)Keys.D2);
                        sendTargetkey.SendKeyUp((int)Keys.D2);
                        break;
                    case 3:     //Right arrow (move right)
                        sendTargetkey.SendKeyUp((int)Keys.Up);
                        sendTargetkey.SendKeyDown((int)Keys.Right);
                        turnKeyIsDown = true;
                        break;
                    case 4:     //Left  arrow (move left)
                        sendTargetkey.SendKeyUp((int)Keys.Up);
                        sendTargetkey.SendKeyDown((int)Keys.Left);
                        turnKeyIsDown = true;
                        break;
                }
            base.sendCommand(output);
        }


    }
}
