using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECHotkeyHelper
{
    
    public static class ECHotkeyHelper
    {        
        private static int nextKeyId = 0;        
        private static HelperForm form = new HelperForm();
        internal static Dictionary<Keys, ECHotkey> hotkeys = new Dictionary<Keys, ECHotkey>();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static string KeysToString(Keys _key)
        {
            string ret = "";
            List<string> modifiers = new List<string>();

            if (((int)_key & (int)Keys.Modifiers & (int)Keys.Shift) > 0)
                modifiers.Add("Shift");
            if (((int)_key & (int)Keys.Modifiers & (int)Keys.Control) > 0)
                modifiers.Add("Control");
            if (((int)_key & (int)Keys.Modifiers & (int)Keys.Alt) > 0)
                modifiers.Add("Alt");

            ret = String.Join(", ", modifiers.ToArray());

            int keyCode = (int)_key & (int)Keys.KeyCode;
            Keys key = (Keys)keyCode;

            if (keyCode != 0 && modifiers.Count != 0)
            {
                if (!key.ToString().Contains("Key"))
                    ret += " + " + ((Keys)keyCode).ToString();
            }
            else
            {
                if (!key.ToString().Contains("Key"))
                    ret = ((Keys)keyCode).ToString();
            }
            return ret;
        }

        public static ECHotkey[] GetRegisteredHotkeys()
        {
            return hotkeys.Values.ToArray();
        }

        public static ECHotkey RegisterHotkey(Keys _key, Keys _modifiers, ECHotKeyHelperDelegate _callback)
        {
            int keyAndModifiers = (int)_key | (int)_modifiers;
            return RegisterHotkey((Keys)keyAndModifiers, _callback);
        }

        public static ECHotkey RegisterHotkey(Keys _keyAndModifiers, ECHotKeyHelperDelegate _callback)
        {
            int key = (int)_keyAndModifiers & (int)Keys.KeyCode;
            int modifiers = (int)_keyAndModifiers & (int)Keys.Modifiers;

            ECHotkey hotkey = new ECHotkey(nextKeyId, (Keys)key, (Keys)modifiers, _callback);
            hotkeys.Add(_keyAndModifiers, hotkey);

            if (!RegisterHotKey(form.Handle, nextKeyId, WinFormKeyToWinProcKey((Keys)modifiers), key))
                throw new InvalidOperationException("Couldn’t register the hot key.");

            nextKeyId++;
            return hotkey;
        }

        public static void UnregisterHotkey(ECHotkey _hotkey)
        {
            UnregisterHotKey(form.Handle, _hotkey.id);
        }

        public static void UnregisterAllHotkeys()
        {
            foreach (ECHotkey hk in hotkeys.Values)
            {                
                UnregisterHotKey(form.Handle, hk.id);
            }
            hotkeys.Clear();
        }

        internal static int WinFormKeyToWinProcKey(Keys _keys)
        {
            int ret = 0;
            int key = (int)_keys & (int)Keys.KeyCode;
            int modifiers = (int)_keys & (int)Keys.Modifiers;

            ret = key;

            ret = (modifiers & (int)Keys.Alt) > 0 ? ret | (int)ECWinProcKeyModifier.Alt : ret;
            ret = (modifiers & (int)Keys.Shift) > 0 ? ret | (int)ECWinProcKeyModifier.Shift : ret;
            ret = (modifiers & (int)Keys.Control) > 0 ? ret | (int)ECWinProcKeyModifier.Control : ret;

            return ret;
        }

        internal static Keys WinProcKeyToWinFormKey(ECWinProcKeyModifier _modifiers)
        {
            int ret = (int)ECWinProcKeyModifier.None;            

            ret = (_modifiers & ECWinProcKeyModifier.Alt) > 0 ? ret | (int)Keys.Alt : ret;
            ret = (_modifiers & ECWinProcKeyModifier.Shift) > 0 ? ret | (int)Keys.Shift : ret;
            ret = (_modifiers & ECWinProcKeyModifier.Control) > 0 ? ret | (int)Keys.Control : ret;
            //ret = (modifier & ECWinProcKeyModifier.WinKey) > 0 ? ret | (int)Keys.Wi : ret;

            return (Keys)ret;
        }        

    }
}
