using System;
using System.Runtime.InteropServices;

namespace ZephyrRenderer.Mac
{
    internal static class NativeMethods
    {
        // Constants for window/view ordering
        public const int NSWindowAbove = 1;
        public const int NSWindowBelow = -1;
        public const int NSWindowOut = 0;

        // Basic AppKit functions
        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
        public static extern void NSApplicationLoad();

        // Basic Objective-C runtime functions
        [DllImport("/usr/lib/libobjc.A.dylib")]
        public static extern IntPtr objc_getClass(string name);

        [DllImport("/usr/lib/libobjc.A.dylib")]
        public static extern IntPtr sel_registerName(string name);

        // objc_msgSend variants - all with EntryPoint="objc_msgSend"
        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, CGRect rect);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, string arg1);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, IntPtr arg1);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, CGRect rect, ulong style, ulong bufferingType, bool defer);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend_void(IntPtr receiver, IntPtr selector, IntPtr arg1);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend_bool(IntPtr receiver, IntPtr selector, bool arg1);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern IntPtr objc_msgSend_Init(IntPtr receiver, IntPtr selector, CGRect rect);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern CGSize objc_msgSend_CGSize(IntPtr receiver, IntPtr selector);

        // Class creation and method adding
        [DllImport("/usr/lib/libobjc.A.dylib")]
        public static extern IntPtr objc_allocateClassPair(IntPtr superclass, string name, int extraBytes);

        [DllImport("/usr/lib/libobjc.A.dylib")]
        public static extern void objc_registerClassPair(IntPtr cls);

        [DllImport("/usr/lib/libobjc.A.dylib")]
        public static extern bool class_addMethod(IntPtr cls, IntPtr sel, IntPtr imp, string types);

        // Helper methods that use objc_msgSend internally
        public static IntPtr NSGraphicsContext_currentContext()
        {
            IntPtr nsGraphicsContextClass = objc_getClass("NSGraphicsContext");
            IntPtr currentContextSelector = sel_registerName("currentContext");
            return objc_msgSend(nsGraphicsContextClass, currentContextSelector);
        }

        public static IntPtr NSGraphicsContext_CGContext(IntPtr graphicsContext)
        {
            if (graphicsContext == IntPtr.Zero)
                return IntPtr.Zero;
                
            IntPtr cgContextSelector = sel_registerName("CGContext");
            return objc_msgSend(graphicsContext, cgContextSelector);
        }

        public static CGSize NSImage_size(IntPtr nsImage)
        {
            if (nsImage == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nsImage));

            IntPtr sizeSelector = sel_registerName("size");
            return objc_msgSend_CGSize(nsImage, sizeSelector);
        }

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern IntPtr objc_msgSend_CGImage(IntPtr receiver, IntPtr selector, ref CGRect rect, IntPtr context, IntPtr hints);

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        public static extern void CGContextDrawImage(IntPtr context, CGRect rect, IntPtr image);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend_AddSubview(IntPtr receiver, IntPtr selector, IntPtr view, int position, IntPtr relativeTo);
        
        public static IntPtr NSImage_CGImageForProposedRect(IntPtr nsImage, ref CGRect proposedRect, IntPtr context, IntPtr hints)
        {
            if (nsImage == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nsImage));

            IntPtr selector = sel_registerName("CGImageForProposedRect:context:hints:");
            return objc_msgSend_CGImage(nsImage, selector, ref proposedRect, context, hints);
        }
    }
}