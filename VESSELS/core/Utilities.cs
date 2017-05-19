using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace VESSELS
{

    public struct VESSELS_Configuration
    {
        public int screenwidth;
        public int screenheight;
        public vessel_comm_component comm_component;
        public vessel_app_component app_component;
        public vessel_stim_component stim_component;

        public string stim_shape;
        public string stim_frequency;
        public string stim_color;
        public string stim_pattern;
        public string stim_size;
    }

    public enum vessel_comm_component
    {
        BCI2000comm,
        BCI2000commSOCI,
        LSLcomm,
        LSLcommSOCI,
        AnyAppAdvanced2Class,
        AnyAppAdvancedControl,
        AnyAppBasicControl,
        AnyAppWowControl,
        SpecificAppAdvancedControl,
        SpecificAppBasicControl,
        SpecificAppCursorControl,
        SpecificAppWowControl,

    }

    public enum vessel_app_component
    {
        MazeGame,
        GoogleMaps,
        Navigation,
        Practice,
        SOCS
    }

    public enum vessel_stim_component
    {
        SSVEP,
        SSVEPcb,
        tightSSVEP,
        checkerSSSVEP,
        RingStimulator,
        SSVEP_DirectX,
        SSVEP_DirectX_Advanced,
        SSVEP_DirectX_Advanced_V2,
        SSVEP_DirectX_Advanced_V2b,
        SSVEP_DirectX2,
        SSVEP_GDI,
        SSVEPadvancedGDI,
        SSVEPbasicGDI
    }

    public struct MOUSEPOINT
    {
        public int X;
        public int Y;
    }

    public class User32
    {
        [DllImport("user32.dll")]
        public static extern void SetWindowPos(uint Hwnd, int Level, int X, int Y, int W, int H, uint Flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("dwmapi.dll")]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref int[] pMargins);

        [DllImport("user32.dll")]
        public static extern int GetCursorPos(ref MOUSEPOINT mousePoint);

    }
}
