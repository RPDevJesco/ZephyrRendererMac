namespace ZephyrRenderer.Mac
{
    internal static class NSWindowWrapper
{
    // Window style masks
    private const ulong NSWindowStyleMaskTitled = 1;
    private const ulong NSWindowStyleMaskClosable = 2;
    private const ulong NSWindowStyleMaskMiniaturizable = 4;
    private const ulong NSWindowStyleMaskResizable = 8;
    private const ulong NSWindowStyleMaskDefault = 
        NSWindowStyleMaskTitled | NSWindowStyleMaskClosable | 
        NSWindowStyleMaskMiniaturizable | NSWindowStyleMaskResizable;

    // Backing store types
    private const ulong NSBackingStoreBuffered = 2;

    public static IntPtr Create(CGRect rect)
    {
        IntPtr windowClass = NativeMethods.objc_getClass("NSWindow");
        IntPtr allocSelector = NativeMethods.sel_registerName("alloc");
        IntPtr initSelector = NativeMethods.sel_registerName("initWithContentRect:styleMask:backing:defer:");
        IntPtr makeKeyAndOrderFrontSelector = NativeMethods.sel_registerName("makeKeyAndOrderFront:");

        // Create the window
        IntPtr window = NativeMethods.objc_msgSend(windowClass, allocSelector);
        window = NativeMethods.objc_msgSend(window, initSelector, rect, 
            NSWindowStyleMaskDefault, NSBackingStoreBuffered, false);

        if (window == IntPtr.Zero)
        {
            throw new Exception("Failed to create NSWindow");
        }

        // Set window properties
        ConfigureWindow(window);

        // Create and configure content view
        IntPtr contentView = CreateContentView(rect);
        SetContentView(window, contentView);

        // Make window visible
        NativeMethods.objc_msgSend(window, makeKeyAndOrderFrontSelector, IntPtr.Zero);

        return window;
    }

    private static void ConfigureWindow(IntPtr window)
    {
        // Set window to be opaque
        var setOpaqueSelector = NativeMethods.sel_registerName("setOpaque:");
        NativeMethods.objc_msgSend_bool(window, setOpaqueSelector, true);

        // Set background color using our color wrapper
        var setBackgroundColorSelector = NativeMethods.sel_registerName("setBackgroundColor:");
        var backgroundColor = NSColorWrapper.CreateColor(0.2f, 0.2f, 0.2f); // Dark gray
        NativeMethods.objc_msgSend(window, setBackgroundColorSelector, backgroundColor);
    }

    private static IntPtr CreateContentView(CGRect rect)
    {
        IntPtr contentView = NSViewWrapper.Create(rect);
        
        // Set content view background color
        var setBackgroundColorSelector = NativeMethods.sel_registerName("setBackgroundColor:");
        var backgroundColor = NSColorWrapper.CreateColor(0.2f, 0.2f, 0.2f); // Match window color
        NativeMethods.objc_msgSend(contentView, setBackgroundColorSelector, backgroundColor);

        // Enable layer backing
        var setWantsLayerSelector = NativeMethods.sel_registerName("setWantsLayer:");
        NativeMethods.objc_msgSend_bool(contentView, setWantsLayerSelector, true);

        return contentView;
    }

    public static void SetContentView(IntPtr window, IntPtr view)
    {
        if (window == IntPtr.Zero)
            throw new ArgumentNullException(nameof(window));
        if (view == IntPtr.Zero)
            throw new ArgumentNullException(nameof(view));

        IntPtr setContentViewSelector = NativeMethods.sel_registerName("setContentView:");
        NativeMethods.objc_msgSend(window, setContentViewSelector, view);
    }

    public static IntPtr GetContentView(IntPtr window)
    {
        if (window == IntPtr.Zero)
            throw new ArgumentNullException(nameof(window));

        IntPtr contentViewSelector = NativeMethods.sel_registerName("contentView");
        return NativeMethods.objc_msgSend(window, contentViewSelector);
    }
}
}