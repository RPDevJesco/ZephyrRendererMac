using System;
using ZephyrRenderer.Mac;
using ZephyrRenderer.UI;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Starting application...");

            var window = new Window();

            // Initialize NSApplication
            window.InitializeApp();

            // Create a larger window
            window.CreateWindow(100, 100, 300, 400);

            // Create a panel and attach it to the window
            using (var panel = new Panel(window.GetWindowHandle(), 0, 0, 1000, 800))
            {
                Console.WriteLine("Panel created and added to NSWindow.");

                // Create bitmap renderer and custom view for drawing first (will be below)
                using (var bitmapRenderer = new BitmapRenderer(window.GetWindowHandle()))
                {
                    string imagePath = "/Users/jesseglover/Desktop/sexyvelma.jpg";
                    Console.WriteLine($"Loading bitmap from {imagePath}");
                    bitmapRenderer.LoadBitmap(imagePath);

                    // Create a large custom view for the image
                    var customView = new CustomNSView(
                        new CGRect(0, 20, 700, 700),  // Larger view area
                        bitmapRenderer,
                        0, 0);  // Start drawing at top-left of the view
                    
                    // Add image view below
                    panel.AddChild(customView.Handle, false);

                    // Create buttons on the left side (will be above)
                    using (var button1 = new Button())
                    {
                        button1.CreateButton(20, 20, 200, 50, "Click Me");
                        button1.Click += (sender, e) => Console.WriteLine("First button clicked!");
                        panel.AddChild(button1.GetButtonHandle(), true);
                    }

                    using (var button2 = new Button())
                    {
                        button2.CreateButton(20, 90, 200, 50, "Another Button");
                        button2.Click += (sender, e) => Console.WriteLine("Second button clicked!");
                        panel.AddChild(button2.GetButtonHandle(), true);
                    }

                    // Start the NSApplication run loop
                    window.Run();
                }
            }

            Console.WriteLine("Application started.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}