using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using VESSELS.BCI_Logic.Device_Emulation;

namespace VESSELS.BCI_Logic.BCI2000_Control.AnyApp
{
    class Advanced2Class : LSLcommSOCI
    {
        //Constructor
        public Advanced2Class(Game game)
            : base(game)
        {
        }

        //Initialize. 
        public override void Initialize()
        {

            base.Initialize();
        }

        //Override this method for the application specific control
        protected override void sendCommand(int output)
        {
            switch (output)
            {
                case 0:     //Do Nothing
                    VirtualKeyboard.ReleaseKey(Keys.Left);
                    VirtualKeyboard.ReleaseKey(Keys.Right);
                    break;
                case 1:    //Left  arrow (move left)
                    VirtualKeyboard.HoldKey(Keys.Left);
                    VirtualKeyboard.ReleaseKey(Keys.Right);
                    //Console.WriteLine("Left");
                    break;
                case 2:    //Right arrow (move right)
                    VirtualKeyboard.ReleaseKey(Keys.Left);
                    VirtualKeyboard.HoldKey(Keys.Right);
                    //Console.WriteLine("Right");
                    break;
                default:
                    VirtualKeyboard.ReleaseKey(Keys.Left);
                    VirtualKeyboard.ReleaseKey(Keys.Right);
                    break;

            }
            base.sendCommand(output);
        }

    }
}
