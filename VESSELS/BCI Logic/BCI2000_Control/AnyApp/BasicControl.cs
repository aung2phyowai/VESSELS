using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VESSELS.BCI_Logic.Device_Emulation;

namespace VESSELS.BCI_Logic.BCI2000_Control.AnyApp
{
    class BasicControl : LSLcommSOCI
    {
        //Constructor
        public BasicControl(Game game)
            :base(game)
        {
        }

        //Override this method for the application specific control
        protected override void sendCommand(int output)
        {
            switch (output)
            {
                case 0:     //Do Nothing
                    break;
                case 1:     //Up arrow (move forward/up)
                    VirtualKeyboard.PressKey(System.Windows.Forms.Keys.Up);
                    Console.WriteLine("Up");
                    break;
                case 2:     //Down arrow (move backward/down)
                    VirtualKeyboard.PressKey(System.Windows.Forms.Keys.Down);
                    Console.WriteLine("Down");
                    break;
                case 3:     //Right arrow (move right)
                    VirtualKeyboard.PressKey(System.Windows.Forms.Keys.Right);
                    Console.WriteLine("Right");
                    break;
                case 4:     //Left  arrow (move left)
                    VirtualKeyboard.PressKey(System.Windows.Forms.Keys.Left);
                    Console.WriteLine("Left");
                    break;
            }
            
            base.sendCommand(output);
        }


    }
}
