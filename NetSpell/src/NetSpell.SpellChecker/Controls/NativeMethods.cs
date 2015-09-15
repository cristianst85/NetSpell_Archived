using System;
using System.Runtime.InteropServices;

namespace NetSpell.SpellChecker.Controls {
    /// <summary>
    /// Summary description for NativeMethods.
    /// </summary>
    internal sealed class NativeMethods {

        private NativeMethods() {
        }

        // Windows Messages 
        internal const int WM_SETREDRAW = 0x000B;

        internal const int WM_PAINT = 0x000F;
        internal const int WM_ERASEBKGND = 0x0014;

        internal const int WM_NOTIFY = 0x004E;

        internal const int WM_HSCROLL = 0x0114;
        internal const int WM_VSCROLL = 0x0115;

        internal const int WM_CAPTURECHANGED = 0x0215;

        internal const int WM_USER = 0x0400;

        internal const int CFM_UNDERLINETYPE = 0x00800000;

        internal const int CFM_BACKCOLOR = 0x04000000;

        /* EM_SETCHARFORMAT wparam masks */
        internal const int SCF_SELECTION = 0x0001;
        internal const int SCF_WORD = 0x0002;
        internal const int SCF_DEFAULT = 0x0000;                        // set the default charformat or paraformat
        internal const int SCF_ALL = 0x0004;                            // not valid with SCF_SELECTION or SCF_WORD
        internal const int SCF_USEUIRULES = 0x0008;                     // modifier for SCF_SELECTION; says that
        // the format came from a toolbar, etc. and 
        // therefore UI formatting rules should be
        // used instead of strictly formatting the 
        // selection. 

        internal const int EM_SETCHARFORMAT = NativeMethods.WM_USER + 68;
        internal const int EM_GETCHARFORMAT = NativeMethods.WM_USER + 58;
        internal const int EM_SETEVENTMASK = NativeMethods.WM_USER + 69;

        // Win API declaration
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        internal static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        internal static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, ref CHARFORMAT2 cf) {
            IntPtr lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lParam, false);
            return NativeMethods.SendMessage(hWnd, msg, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
        public class CHARFORMAT2 {
            public int cbSize = Marshal.SizeOf(typeof(CHARFORMAT2));
            public int dwMask;
            public int dwEffects;
            public int yHeight;
            public int yOffset;
            public int crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szFaceName;
            public short wWeight;
            public short sSpacing;
            public int crBackColor;
            public int lcid;
            public int dwReserved;
            public short sStyle;
            public short wKerning;
            public byte bUnderlineType;
            public byte bAnimation;
            public byte bRevAuthor;
            public byte bReserved1;
        }
    }
}
