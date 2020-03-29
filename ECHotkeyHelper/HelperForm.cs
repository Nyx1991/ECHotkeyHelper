using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECHotkeyHelper
{
    internal partial class HelperForm : Form
    {
        private static int WM_HOTKEY = 0x0312;

        internal HelperForm()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            
            if (m.Msg == WM_HOTKEY)
            {
                // get the keys.
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                ECWinProcKeyModifier modifiers = (ECWinProcKeyModifier)((int)m.LParam & 0xFFFF);

                int winFormsModifier = (int)ECHotkeyHelper.WinProcKeyToWinFormKey(modifiers);
                Keys keyAndModifier = (Keys)(winFormsModifier | (int)key);

                if (ECHotkeyHelper.hotkeys.ContainsKey(keyAndModifier))
                {                    
                    ECHotkeyHelper.hotkeys[keyAndModifier].callback();
                }
            }
        }

    }
}
