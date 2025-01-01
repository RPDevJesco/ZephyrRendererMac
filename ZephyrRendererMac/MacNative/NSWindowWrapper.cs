namespace ZephyrRenderer.Mac
{
    internal static class NSWindowWrapper
    {
        public static IntPtr Create(CGRect rect)
        {
            IntPtr windowClass = NativeMethods.objc_getClass("NSWindow");
            IntPtr allocSelector = NativeMethods.sel_registerName("alloc");
            IntPtr initSelector = NativeMethods.sel_registerName("initWithContentRect:styleMask:backing:defer:");
            IntPtr makeKeyAndOrderFrontSelector = NativeMethods.sel_registerName("makeKeyAndOrderFront:");
            
            // Style mask: titled (1) | closable (2) | miniaturizable (4) | resizable (8) = 15
            const ulong styleMask = 15;
            // Backing store type: buffered = 2
            const ulong backingStoreType = 2;

            IntPtr window = NativeMethods.objc_msgSend(windowClass, allocSelector);
            window = NativeMethods.objc_msgSend(window, initSelector, rect, styleMask, backingStoreType, false);

            // After creating the window
            var setOpaqueSelector = NativeMethods.sel_registerName("setOpaque:");
            NativeMethods.objc_msgSend_bool(window, setOpaqueSelector, true);
            var colorClass = NativeMethods.objc_getClass("NSColor");
            var setBackgroundColorSelector = NativeMethods.sel_registerName("setBackgroundColor:");
            var blackColor = NativeMethods.objc_msgSend(colorClass, NativeMethods.sel_registerName("blackColor"));
            NativeMethods.objc_msgSend(window, setBackgroundColorSelector, blackColor);
            
            var colorSelector = NativeMethods.sel_registerName("colorWithDeviceRed:green:blue:alpha:");
            NativeMethods.objc_msgSend(window, setBackgroundColorSelector, blackColor);

            // Create the content view with black background
            IntPtr contentView = NSViewWrapper.Create(rect);
            var contentViewColorSelector = NativeMethods.sel_registerName("setBackgroundColor:");
            NativeMethods.objc_msgSend(contentView, contentViewColorSelector, blackColor);
            
            // Set layer-backed
            var setWantsLayerSelector = NativeMethods.sel_registerName("setWantsLayer:");
            NativeMethods.objc_msgSend_bool(contentView, setWantsLayerSelector, true);

            SetContentView(window, contentView);
            NativeMethods.objc_msgSend(window, makeKeyAndOrderFrontSelector, IntPtr.Zero);

            return window;
        }

        public static void SetContentView(IntPtr window, IntPtr view)
        {
            IntPtr setContentViewSelector = NativeMethods.sel_registerName("setContentView:");
            NativeMethods.objc_msgSend(window, setContentViewSelector, view);
        }

        public static IntPtr GetContentView(IntPtr window)
        {
            IntPtr contentViewSelector = NativeMethods.sel_registerName("contentView");
            return NativeMethods.objc_msgSend(window, contentViewSelector);
        }
    }
}