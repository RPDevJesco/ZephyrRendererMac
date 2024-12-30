using System;
using System.Runtime.InteropServices;

namespace ZephyrRenderer.Mac
{
    internal static class ButtonClickHandler
    {
        private delegate void ButtonClickedDelegate(IntPtr self, IntPtr cmd);

        public static event EventHandler ButtonClickedEvent;

        public static IntPtr Create()
        {
            IntPtr classHandle = NativeMethods.objc_getClass("NSObject");
            IntPtr instance = NativeMethods.objc_msgSend(classHandle, NativeMethods.sel_registerName("alloc"));
            instance = NativeMethods.objc_msgSend(instance, NativeMethods.sel_registerName("init"));

            ButtonClickedDelegate buttonClickedDelegate = ButtonClicked;
            IntPtr buttonClickedSelector = NativeMethods.sel_registerName("buttonClicked:");
            IntPtr imp = Marshal.GetFunctionPointerForDelegate(buttonClickedDelegate);
            NativeMethods.class_addMethod(classHandle, buttonClickedSelector, imp, "v@:@");

            return instance;
        }

        private static void ButtonClicked(IntPtr self, IntPtr cmd)
        {
            Console.WriteLine("Button clicked!");
            ButtonClickedEvent?.Invoke(null, EventArgs.Empty);
        }
    }
}