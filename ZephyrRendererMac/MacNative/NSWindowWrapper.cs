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

            IntPtr window = NativeMethods.objc_msgSend(windowClass, allocSelector);
            window = NativeMethods.objc_msgSend(window, initSelector, rect, 15, 2, false);
            NativeMethods.objc_msgSend(window, makeKeyAndOrderFrontSelector, IntPtr.Zero);

            // Create a container view and set it as the content view
            IntPtr containerView = NSViewWrapper.Create(rect);
            SetContentView(window, containerView);

            return window;
        }

        public static void SetContentView(IntPtr window, IntPtr view)
        {
            IntPtr setContentViewSelector = NativeMethods.sel_registerName("setContentView:");
            NativeMethods.objc_msgSend(window, setContentViewSelector, view);
        }

        public static void AddSubview(IntPtr window, IntPtr subview)
        {
            IntPtr contentView = GetContentView(window);
            NSViewWrapper.AddSubview(contentView, subview);
        }

        private static IntPtr GetContentView(IntPtr window)
        {
            IntPtr contentViewSelector = NativeMethods.sel_registerName("contentView");
            return NativeMethods.objc_msgSend(window, contentViewSelector);
        }
    }
}