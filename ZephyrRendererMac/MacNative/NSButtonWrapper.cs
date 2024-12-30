using System;
using System.Runtime.InteropServices;

namespace ZephyrRenderer.Mac
{
    internal static class NSButtonWrapper
    {
        public static IntPtr Create(CGRect rect, string title)
        {
            IntPtr buttonClass = NativeMethods.objc_getClass("NSButton");
            IntPtr allocSelector = NativeMethods.sel_registerName("alloc");
            IntPtr initWithFrameSelector = NativeMethods.sel_registerName("initWithFrame:");
            IntPtr setTitleSelector = NativeMethods.sel_registerName("setTitle:");

            IntPtr button = NativeMethods.objc_msgSend(buttonClass, allocSelector);
            button = NativeMethods.objc_msgSend(button, initWithFrameSelector, rect);

            IntPtr nsStringClass = NativeMethods.objc_getClass("NSString");
            IntPtr stringWithUTF8StringSelector = NativeMethods.sel_registerName("stringWithUTF8String:");
            IntPtr buttonTitle = NativeMethods.objc_msgSend(nsStringClass, stringWithUTF8StringSelector, title);
            NativeMethods.objc_msgSend(button, setTitleSelector, buttonTitle);

            return button;
        }

        public static void SetTarget(IntPtr button, IntPtr target, string action)
        {
            IntPtr setTargetSelector = NativeMethods.sel_registerName("setTarget:");
            IntPtr setActionSelector = NativeMethods.sel_registerName("setAction:");
            IntPtr actionSelector = NativeMethods.sel_registerName(action);

            NativeMethods.objc_msgSend(button, setTargetSelector, target);
            NativeMethods.objc_msgSend(button, setActionSelector, actionSelector);
        }
    }
}