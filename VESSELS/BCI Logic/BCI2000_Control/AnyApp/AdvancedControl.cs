using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using VESSELS.BCI_Logic.Device_Emulation;

namespace VESSELS.BCI_Logic.BCI2000_Control.AnyApp
{
    class AdvancedControl : BCI2000commSOCI
    {
        //Constructor
        public AdvancedControl(Game game)
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
                    VirtualKeyboard.ReleaseKey(Keys.Up);
                    VirtualKeyboard.ReleaseKey(Keys.Down);
                    VirtualKeyboard.ReleaseKey(Keys.Left);
                    VirtualKeyboard.ReleaseKey(Keys.Right);
                    break;
                case 1:     //Up arrow (move forward/up)
                    VirtualKeyboard.HoldKey(Keys.Up);
                    VirtualKeyboard.ReleaseKey(Keys.Down);
                    VirtualKeyboard.ReleaseKey(Keys.Left);
                    VirtualKeyboard.ReleaseKey(Keys.Right);
                    //Console.WriteLine("Up");
                    break;
                case 2:     //Down arrow (move backward/down)
                    VirtualKeyboard.ReleaseKey(Keys.Up);
                    VirtualKeyboard.HoldKey(Keys.Down);
                    VirtualKeyboard.ReleaseKey(Keys.Left);
                    VirtualKeyboard.ReleaseKey(Keys.Right);
                    //Console.WriteLine("Down");
                    break;
                case 3:     //Right arrow (move right)
                    VirtualKeyboard.ReleaseKey(Keys.Up);
                    VirtualKeyboard.ReleaseKey(Keys.Down);
                    VirtualKeyboard.ReleaseKey(Keys.Left);
                    VirtualKeyboard.HoldKey(Keys.Right);
                    //Console.WriteLine("Right");
                    break;
                case 4:     //Left  arrow (move left)
                    VirtualKeyboard.ReleaseKey(Keys.Up);
                    VirtualKeyboard.ReleaseKey(Keys.Down);
                    VirtualKeyboard.HoldKey(Keys.Left);
                    VirtualKeyboard.ReleaseKey(Keys.Right);
                    //Console.WriteLine("Left");
                    break;
                default:
                    VirtualKeyboard.ReleaseKey(Keys.Up);
                    VirtualKeyboard.ReleaseKey(Keys.Down);
                    VirtualKeyboard.ReleaseKey(Keys.Left);
                    VirtualKeyboard.ReleaseKey(Keys.Right);
                    break;

            }
            base.sendCommand(output);
        }

    }
}
