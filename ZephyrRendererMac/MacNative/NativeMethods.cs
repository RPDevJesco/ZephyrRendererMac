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

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend_setBackground(IntPtr receiver, IntPtr selector, IntPtr color);
        
        // Class creation and method adding
        [DllImport("/usr/lib/libobjc.A.dylib")]
        public static extern IntPtr objc_allocateClassPair(IntPtr superclass, string name, int extraBytes);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend_layerBacked(IntPtr receiver, IntPtr selector, bool backed);

        // This one is specifically for needsDisplay and isFlipped boolean returns
        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern bool objc_msgSend_bool_ret(IntPtr receiver, IntPtr selector);
        
        [DllImport("/usr/lib/libobjc.A.dylib")]
        public static extern void objc_registerClassPair(IntPtr cls);

        [DllImport("/usr/lib/libobjc.A.dylib")]
        public static extern bool class_addMethod(IntPtr cls, IntPtr sel, IntPtr imp, string types);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, float r, float g, float b, float a);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend_ulong(IntPtr receiver, IntPtr selector, ulong value);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend(IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2, bool arg3);
        
        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern CGRect objc_msgSend_CGRect(IntPtr receiver, IntPtr selector);
        
        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern double objc_msgSend_Double(IntPtr receiver, IntPtr selector);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern IntPtr objc_msgSend_Font(IntPtr receiver, IntPtr selector, float size);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend_Positioned(IntPtr receiver, IntPtr selector, IntPtr subview, int position, IntPtr relativeTo);

        // Add this to the NativeMethods class:
        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern ulong objc_msgSend_GetMask(IntPtr receiver, IntPtr selector);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend_SetMask(IntPtr receiver, IntPtr selector, ulong value);
        
        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend_Size(IntPtr receiver, IntPtr selector, CGSize size);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend_Point(IntPtr receiver, IntPtr selector, CGPoint point);

        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend_Double(IntPtr receiver, IntPtr selector, double value);
        
        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern void objc_msgSend_setLineWidth(IntPtr receiver, IntPtr selector, double width);
        
        [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint = "objc_msgSend")]
        public static extern IntPtr objc_msgSend_HSBA(IntPtr receiver, IntPtr selector, float hue, float saturation, float brightness, float alpha);

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