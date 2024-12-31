using System;
using System.Runtime.InteropServices;
using ZephyrRenderer.UI;

namespace ZephyrRenderer.Mac
{
    public class CustomNSView
    {
        private IntPtr _viewHandle;
        private BitmapRenderer _bitmapRenderer;
        private readonly float _x;
        private readonly float _y;
        private static DrawRectDelegate _drawRectDelegate;

        public CustomNSView(CGRect frame, BitmapRenderer bitmapRenderer, float x, float y)
        {
            _bitmapRenderer = bitmapRenderer;
            _x = x;
            _y = y;

            // Create NSView subclass
            var className = "CustomView";
            var baseClass = NativeMethods.objc_getClass("NSView");
            var newClass = NativeMethods.objc_allocateClassPair(baseClass, className, 0);

            if (newClass == IntPtr.Zero)
            {
                // Class might already exist, try to get it
                newClass = NativeMethods.objc_getClass(className);
                if (newClass == IntPtr.Zero)
                {
                    throw new Exception("Failed to create or get CustomView class");
                }
            }
            else
            {
                // Add drawRect: method
                _drawRectDelegate = new DrawRectDelegate(DrawRect);
                var drawRectPtr = Marshal.GetFunctionPointerForDelegate(_drawRectDelegate);
                if (!NativeMethods.class_addMethod(newClass, NativeMethods.sel_registerName("drawRect:"), drawRectPtr, "v@:{CGRect=dddd}"))
                {
                    throw new Exception("Failed to add drawRect: method");
                }

                // Register the class
                NativeMethods.objc_registerClassPair(newClass);
            }

            // Create an instance
            _viewHandle = NativeMethods.objc_msgSend(newClass, NativeMethods.sel_registerName("alloc"));
            if (_viewHandle == IntPtr.Zero)
            {
                throw new Exception("Failed to allocate CustomView instance");
            }

            _viewHandle = NativeMethods.objc_msgSend_Init(_viewHandle, NativeMethods.sel_registerName("initWithFrame:"), frame);
            if (_viewHandle == IntPtr.Zero)
            {
                throw new Exception("Failed to initialize CustomView instance");
            }

            // Set needs display
            var setNeedsDisplaySelector = NativeMethods.sel_registerName("setNeedsDisplay:");
            NativeMethods.objc_msgSend_bool(_viewHandle, setNeedsDisplaySelector, true);
        }

        public IntPtr Handle => _viewHandle;

        private void DrawRect(IntPtr self, IntPtr cmd, CGRect dirtyRect)
        {
            try
            {
                var currentContext = NativeMethods.NSGraphicsContext_currentContext();
                if (currentContext == IntPtr.Zero)
                {
                    Console.WriteLine("Failed to get current graphics context");
                    return;
                }

                var cgContext = NativeMethods.NSGraphicsContext_CGContext(currentContext);
                if (cgContext == IntPtr.Zero)
                {
                    Console.WriteLine("Failed to get CGContext");
                    return;
                }

                Console.WriteLine($"Drawing at coordinates: ({_x}, {_y})");
                _bitmapRenderer.Render(cgContext, _x, _y);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DrawRect: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        private delegate void DrawRectDelegate(IntPtr self, IntPtr cmd, CGRect dirtyRect);
    }
}