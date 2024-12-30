namespace ZephyrRenderer.Mac
{
    internal static class NSViewWrapper
    {
        public static IntPtr Create(CGRect rect)
        {
            IntPtr viewClass = NativeMethods.objc_getClass("NSView");
            IntPtr allocSelector = NativeMethods.sel_registerName("alloc");
            IntPtr initWithFrameSelector = NativeMethods.sel_registerName("initWithFrame:");

            IntPtr view = NativeMethods.objc_msgSend(viewClass, allocSelector);
            view = NativeMethods.objc_msgSend(view, initWithFrameSelector, rect);

            return view;
        }

        public static void AddSubview(IntPtr view, IntPtr subview)
        {
            IntPtr addSubviewSelector = NativeMethods.sel_registerName("addSubview:");
            NativeMethods.objc_msgSend(view, addSubviewSelector, subview);
        }
    }
}