using System.Runtime.InteropServices;
using ZephyrRenderer.Mac;
using ZephyrRenderer.UI;

class Program
{
    static void Main()
    {
        try
        {
            var window = new Window();
            window.InitializeApp();
            
            // Create a window with room for animation
            window.CreateWindow(100, 100, 800, 600);
            var windowHandle = window.GetWindowHandle();

            // Create main panel with small margin
            using (var mainPanel = new Panel(windowHandle, 20, 20, 760, 560))
            {
                // Add the animated panel on the left side
                using (var animatedPanel = new AnimatedPanel(windowHandle, 40, 40, 400, 300))
                {
                    mainPanel.AddChild(animatedPanel.GetPanelHandle());

                    // Create button panel on the right side
                    using (var buttonPanel = new Panel(windowHandle, 460, 40, 280, 300))
                    {
                        mainPanel.AddChild(buttonPanel.GetPanelHandle());

                        // Add buttons with consistent spacing
                        using (var resetButton = new Button())
                        {
                            resetButton.CreateButton(10, 10, 260, 30, "Reset Animation");
                            resetButton.Click += (sender, e) => Console.WriteLine("Reset clicked");
                            buttonPanel.AddChild(resetButton.GetButtonHandle());
                        }

                        using (var colorButton = new Button())
                        {
                            colorButton.CreateButton(10, 50, 260, 30, "Change Colors");
                            colorButton.Click += (sender, e) => Console.WriteLine("Colors clicked");
                            buttonPanel.AddChild(colorButton.GetButtonHandle());
                        }

                        using (var speedButton = new Button())
                        {
                            speedButton.CreateButton(10, 90, 260, 30, "Toggle Speed");
                            speedButton.Click += (sender, e) => Console.WriteLine("Speed clicked");
                            buttonPanel.AddChild(speedButton.GetButtonHandle());
                        }

                        window.Run();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}

public delegate void DrawRectDelegate(IntPtr self, IntPtr cmd, CGRect dirtyRect);

public class AnimatedPanel : Panel 
{
    private float xPos = 0;
    private float yPos = 150;
    private float xDirection = 1;
    private int speed = 2;
    private bool isRunning = true;
    private Thread animationThread;
    private readonly DrawRectDelegate drawRectHandler;
    private readonly double panelX;
    private readonly double panelY;
    private readonly double panelWidth;
    private readonly double panelHeight;

    // Make sure to call the base constructor
    public AnimatedPanel(IntPtr windowHandle, double x, double y, double width, double height) 
        : base(windowHandle, x, y, width, height)  // This must come first
    {
        panelX = x;
        panelY = y;
        panelWidth = width;
        panelHeight = height;
        
        drawRectHandler = new DrawRectDelegate(DrawRect);
        SetupCustomDrawing();
        StartAnimation();
    }

    private void StartAnimation() 
    {
        Console.WriteLine("Starting animation thread...");
        animationThread = new Thread(() => {
            try {
                Console.WriteLine("Animation thread started");
                while (isRunning) {
                    xPos += speed * xDirection;
                    if (xPos >= panelWidth - 40 || xPos <= 0) {
                        xDirection *= -1;
                    }
                    Console.WriteLine($"Position updated: {xPos}");

                    // Request redraw on main thread
                    var nsObjectClass = NativeMethods.objc_getClass("NSObject");
                    var performSelector = NativeMethods.sel_registerName("performSelectorOnMainThread:withObject:waitUntilDone:");
                    var setNeedsDisplaySelector = NativeMethods.sel_registerName("setNeedsDisplay");
                    
                    var handle = GetPanelHandle();
                    if (handle != IntPtr.Zero)
                    {
                        NativeMethods.objc_msgSend(handle, performSelector, setNeedsDisplaySelector, IntPtr.Zero, true);
                        
                        // Force immediate update
                        var displaySelector = NativeMethods.sel_registerName("display");
                        NativeMethods.objc_msgSend(handle, displaySelector);
                    }

                    Thread.Sleep(1000 / 60); // 60 FPS
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Animation thread error: {ex}");
            }
        });
        animationThread.Start();
        Console.WriteLine("Animation thread launched");
    }

    private void DrawRect(IntPtr self, IntPtr cmd, CGRect dirtyRect) 
    {
        Console.WriteLine($"\nDrawRect called with rect: {dirtyRect.X}, {dirtyRect.Y}, {dirtyRect.Width}, {dirtyRect.Height}");

        try
        {
            // Get the current graphics context
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

            // Draw magenta background using hex color
            NSColorWrapper.CreateFromHex("#FF00FF"); // Magenta
            
            // Draw background using NSBezierPath
            var bezierClass = NativeMethods.objc_getClass("NSBezierPath");
            var backgroundPath = NativeMethods.objc_msgSend(bezierClass, NativeMethods.sel_registerName("bezierPath"));
            NativeMethods.objc_msgSend(backgroundPath, NativeMethods.sel_registerName("appendBezierPathWithRect:"), dirtyRect);
            NativeMethods.objc_msgSend(backgroundPath, NativeMethods.sel_registerName("fill"));
            Console.WriteLine("Drew magenta background");

            // Draw white circle for the ball
            NSColorWrapper.SetColor(new Color(1,1,1,1));

            var ballSize = 40.0;
            var ballRect = new CGRect(xPos, yPos - ballSize/2, ballSize, ballSize);
            
            var circlePath = NativeMethods.objc_msgSend(bezierClass, NativeMethods.sel_registerName("bezierPathWithOvalInRect:"));
            NativeMethods.objc_msgSend(circlePath, NativeMethods.sel_registerName("appendBezierPathWithOvalInRect:"), ballRect);
            NativeMethods.objc_msgSend(circlePath, NativeMethods.sel_registerName("fill"));
            Console.WriteLine($"Drew white ball at ({ballRect.X}, {ballRect.Y})");
            
            // Draw green border for debugging - using RGBA
            NSColorWrapper.SetColor(new Color(0, 255, 0,1)); // Bright green
            
            var borderPath = NativeMethods.objc_msgSend(bezierClass, NativeMethods.sel_registerName("bezierPath"));
            NativeMethods.objc_msgSend(borderPath, NativeMethods.sel_registerName("appendBezierPathWithRect:"), dirtyRect);
            
            // Set a thick border width
            NativeMethods.objc_msgSend_setLineWidth(borderPath, NativeMethods.sel_registerName("setLineWidth:"), 4.0);
            NativeMethods.objc_msgSend(borderPath, NativeMethods.sel_registerName("stroke"));
            Console.WriteLine("Drew green border");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DrawRect: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    private void SetupCustomDrawing()
    {
        Console.WriteLine($"Starting custom view setup at ({panelX}, {panelY}) with size {panelWidth}x{panelHeight}");
        
        var className = "CustomAnimatedView";
        var baseClass = NativeMethods.objc_getClass("NSView");

        var newClass = CreateCustomViewClass(baseClass, className);
        var viewHandle = CreateCustomView(newClass);
        ConfigureCustomView(viewHandle);
        ReplaceExistingView(viewHandle);
    }

    private IntPtr CreateCustomViewClass(IntPtr baseClass, string className)
    {
        var newClass = NativeMethods.objc_allocateClassPair(baseClass, className, 0);

        if (newClass == IntPtr.Zero)
        {
            newClass = NativeMethods.objc_getClass(className);
            if (newClass == IntPtr.Zero)
            {
                throw new Exception("Failed to create or get CustomView class");
            }
            return newClass;
        }

        // Add isFlipped method
        var isFlippedPtr = Marshal.GetFunctionPointerForDelegate(new IsFlippedDelegate(() => true));
        NativeMethods.class_addMethod(newClass, NativeMethods.sel_registerName("isFlipped"), 
            isFlippedPtr, "B@:");

        // Add drawRect: method
        var drawRectPtr = Marshal.GetFunctionPointerForDelegate(drawRectHandler);
        NativeMethods.class_addMethod(newClass, NativeMethods.sel_registerName("drawRect:"), 
            drawRectPtr, "v@:{CGRect=dddd}");

        // Register the class
        NativeMethods.objc_registerClassPair(newClass);
        return newClass;
    }

    private IntPtr CreateCustomView(IntPtr viewClass)
    {
        var viewHandle = NativeMethods.objc_msgSend(viewClass, NativeMethods.sel_registerName("alloc"));
        viewHandle = NativeMethods.objc_msgSend_Init(viewHandle, 
            NativeMethods.sel_registerName("initWithFrame:"), 
            new CGRect(panelX, panelY, panelWidth, panelHeight));

        if (viewHandle == IntPtr.Zero)
        {
            throw new Exception("Failed to create custom view");
        }

        return viewHandle;
    }

    private void ConfigureCustomView(IntPtr viewHandle)
    {
        // Enable layer backing and set opaque
        var setWantsLayerSelector = NativeMethods.sel_registerName("setWantsLayer:");
        NativeMethods.objc_msgSend_bool(viewHandle, setWantsLayerSelector, true);

        var setOpaqueSelector = NativeMethods.sel_registerName("setOpaque:");
        NativeMethods.objc_msgSend_bool(viewHandle, setOpaqueSelector, true);

        // Set background color using color wrapper
        SetBackgroundColor(viewHandle, new Color(51,51,51,1), false); // Dark gray background
        // Alternative: SetBackgroundColorHex(viewHandle, "#333333");

        // Set needs display
        var setNeedsDisplaySelector = NativeMethods.sel_registerName("setNeedsDisplay:");
        NativeMethods.objc_msgSend_bool(viewHandle, setNeedsDisplaySelector, true);
    }

    private void ReplaceExistingView(IntPtr newViewHandle)
    {
        var currentHandle = GetPanelHandle();
        var superview = GetParentHandle();

        if (superview != IntPtr.Zero)
        {
            // Remove old view
            var removeFromSuperviewSelector = NativeMethods.sel_registerName("removeFromSuperview");
            NativeMethods.objc_msgSend(currentHandle, removeFromSuperviewSelector);

            // Add new view
            var addSubviewSelector = NativeMethods.sel_registerName("addSubview:");
            NativeMethods.objc_msgSend(superview, addSubviewSelector, newViewHandle);
        }

        // Update the handle
        SetHandle(newViewHandle);
    }

    private delegate bool IsFlippedDelegate();

    public void ResetAnimation()
    {
        xPos = 0;
        yPos = 150;
        xDirection = 1;
        speed = 2;
    }

    public void RandomizeColors()
    {
        var setNeedsDisplaySelector = NativeMethods.sel_registerName("setNeedsDisplay:");
        NativeMethods.objc_msgSend_bool(GetPanelHandle(), setNeedsDisplaySelector, true);
    }

    public void ToggleSpeed()
    {
        speed = speed == 2 ? 4 : 2;
    }

    protected override void Dispose(bool disposing) {
        isRunning = false;
        animationThread?.Join();
        base.Dispose(disposing);
    }
}