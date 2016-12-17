using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace VESSELS.BCI_Logic.Device_Emulation
{
    //This class is a targeted key press generator. It requires the name of the application, but it will
    //send the keypress to the application regardless if the application has focus or not. 
    // i.e. to send to the World of Warcraft game window set appName = "World of Warcraft"; 
    class SendTargetedKey
    {
        string appName;

        public SendTargetedKey(string appName)
        {
            this.appName = appName;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private const Int32 WM_KEYDOWN = 0x0100;
        private const Int32 WM_KEYUP = 0x0101;

        public void SendKeyDown(int Key)
        {
            IntPtr Handle = FindWindow(null, appName);
            PostMessage(Handle, WM_KEYDOWN, Key, 0);
        }
        public void SendKeyUp(int Key)
        {
            IntPtr Handle = FindWindow(null, appName);
            PostMessage(Handle, WM_KEYUP, Key, 0);
        }
    }
}
