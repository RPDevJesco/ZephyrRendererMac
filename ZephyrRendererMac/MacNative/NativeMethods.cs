using System;
using System.Runtime.InteropServices;

namespace ZephyrRenderer.Mac
{
    internal static class NativeMethods
    {
        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
        public static extern void NSApplicationLoad();

        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
        public static extern IntPtr objc_getClass(string name);

        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
        public static extern IntPtr sel_registerName(string name);

        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector);

        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, CGRect rect, ulong style, ulong bufferingType, bool defer);

        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, CGRect rect);

        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
        public static extern void objc_msgSend(IntPtr receiver, IntPtr selector, IntPtr arg1);

        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
        public static extern void objc_msgSend(IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, string arg1);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend_bool(IntPtr receiver, IntPtr selector, bool arg1);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "class_addMethod")]
        public static extern bool class_addMethod(IntPtr cls, IntPtr sel, IntPtr imp, string types);
    }
}