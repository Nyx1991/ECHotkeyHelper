using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECHotkeyHelper
{
    public class ECHotkey
    {
        internal int id { get; set; }
        public Keys Key { get; private set; }
        public Keys Modifiers { get; private set; }
        internal ECHotKeyHelperDelegate callback;

        private ECHotkey()
        {

        }

        internal ECHotkey(int _id, Keys _key, Keys _modifiers, ECHotKeyHelperDelegate _callback)
        {
            id = _id;
            Key = _key;
            Modifiers = _modifiers;
            callback = _callback;            
        }
    }
}
