using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using VESSELS.BCI_Logic.Device_Emulation;

namespace VESSELS.BCI_Logic.BCI2000_Control.SpecificApp
{
    class CursorControl : BCI2000commSOCI
    {
        //Current mouse position
        MOUSEPOINT mousePos;

        //Constructor
        public CursorControl(Game game)
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
            //Get the current cursor pos
            User32.GetCursorPos(ref mousePos);

                switch (output)
                {
                    case 0:     //Do Nothing
                        break;
                    case 1:     //Up arrow (move forward/up)
                        Microsoft.Xna.Framework.Input.Mouse.SetPosition(mousePos.X, mousePos.Y - 15);
                        break;
                    case 2:     //Down arrow (move backward/down)
                        Microsoft.Xna.Framework.Input.Mouse.SetPosition(mousePos.X, mousePos.Y + 15);
                        break;
                    case 3:     //Right arrow (move right)
                        Microsoft.Xna.Framework.Input.Mouse.SetPosition(mousePos.X + 15, mousePos.Y);
                        break;
                    case 4:     //Left  arrow (move left)
                        Microsoft.Xna.Framework.Input.Mouse.SetPosition(mousePos.X - 15, mousePos.Y);
                        break;
                }
           
            base.sendCommand(output);
        }


    }
}