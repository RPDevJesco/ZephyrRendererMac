namespace ZephyrRenderer.Mac
{
    internal static class NSApplicationWrapper
    {
        public static void Load()
        {
            NativeMethods.NSApplicationLoad();
        }

        public static IntPtr Create()
        {
            IntPtr appClass = NativeMethods.objc_getClass("NSApplication");
            IntPtr sharedAppSelector = NativeMethods.sel_registerName("sharedApplication");
            return NativeMethods.objc_msgSend(appClass, sharedAppSelector);
        }

        public static void Activate(IntPtr app)
        {
            IntPtr activateIgnoringOtherAppsSelector = NativeMethods.sel_registerName("activateIgnoringOtherApps:");
            NativeMethods.objc_msgSend_bool(app, activateIgnoringOtherAppsSelector, true);
        }

        public static void Run(IntPtr app)
        {
            IntPtr runSelector = NativeMethods.sel_registerName("run");
            NativeMethods.objc_msgSend(app, runSelector);
        }
    }
}