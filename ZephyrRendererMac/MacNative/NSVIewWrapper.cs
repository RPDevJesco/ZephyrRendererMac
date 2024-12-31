using System;
using System.Runtime.InteropServices;

namespace ZephyrRenderer.Mac
{
    public static class NSViewWrapper
    {
        private static readonly IntPtr nsViewClass = NativeMethods.objc_getClass("NSView");
        private static readonly IntPtr addSubviewSelector = NativeMethods.sel_registerName("addSubview:");
        private static readonly IntPtr removeFromSuperviewSelector = NativeMethods.sel_registerName("removeFromSuperview");
        private static readonly IntPtr setWantsBestResolutionOpenGLSurfaceSelector = NativeMethods.sel_registerName("setWantsBestResolutionOpenGLSurface:");

        public static IntPtr Create(CGRect frame)
        {
            IntPtr view = NativeMethods.objc_msgSend(nsViewClass, NativeMethods.sel_registerName("alloc"));
            view = NativeMethods.objc_msgSend(view, NativeMethods.sel_registerName("initWithFrame:"), frame);
            return view;
        }

        public static void AddSubview(IntPtr parentView, IntPtr childView)
        {
            NativeMethods.objc_msgSend(parentView, addSubviewSelector, childView);
        }

        public static void AddSubviewPositioned(IntPtr parentView, IntPtr childView, bool above)
        {
            var selector = NativeMethods.sel_registerName("addSubview:positioned:relativeTo:");
            var position = above ? NativeMethods.NSWindowAbove : NativeMethods.NSWindowBelow;
            NativeMethods.objc_msgSend_AddSubview(parentView, selector, childView, position, IntPtr.Zero);
        }

        public static void RemoveSubview(IntPtr parentView, IntPtr childView)
        {
            NativeMethods.objc_msgSend(childView, removeFromSuperviewSelector);
        }
    }
}