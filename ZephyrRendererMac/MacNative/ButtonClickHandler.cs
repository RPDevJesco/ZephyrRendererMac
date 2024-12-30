using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ZephyrRenderer.UI;

namespace ZephyrRenderer.Mac
{
    internal static class ButtonClickHandler
    {
        private static readonly Dictionary<IntPtr, Button> _buttonInstances = new Dictionary<IntPtr, Button>();

        public static IntPtr Create(Button button)
        {
            IntPtr target = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            _buttonInstances[target] = button;
            return target;
        }

        private delegate void ButtonClickedDelegate(IntPtr self, IntPtr cmd);

        private static readonly ButtonClickedDelegate ButtonClickedDelegateInstance = ButtonClicked;

        private static void ButtonClicked(IntPtr target, IntPtr sender)
        {
            if (_buttonInstances.TryGetValue(target, out var button))
            {
                button.OnClick();
            }
        }

        public static IntPtr GetButtonClickedDelegate()
        {
            return Marshal.GetFunctionPointerForDelegate(ButtonClickedDelegateInstance);
        }
    }
}