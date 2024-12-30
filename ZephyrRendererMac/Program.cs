using System;
using ZephyrRenderer.Mac;
using ZephyrRenderer.UI;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Starting application...");

        var window = new Window();

        // Initialize NSApplication
        window.InitializeApp();

        // Create a window
        window.CreateWindow(100, 100, 400, 300);

        // Create a panel and attach it to the window
        using (var panel = new Panel(window.GetWindowHandle(), 0, 0, 400, 300))
        {
            Console.WriteLine("Panel created and added to NSWindow.");

            // Create the first button
            var button1 = new Button();
            button1.CreateButton(50, 50, 200, 50, "Click Me");
            button1.Click += (sender, e) => Console.WriteLine("First button clicked!");
            Console.WriteLine("First NSButton created and titled.");

            // Add the first button to the panel
            panel.AddChild(button1.GetButtonHandle());
            Console.WriteLine("First NSButton added to Panel.");

            // Create the second button
            var button2 = new Button();
            button2.CreateButton(50, 150, 200, 50, "Another Button");
            button2.Click += (sender, e) => Console.WriteLine("Second button clicked!");
            Console.WriteLine("Second NSButton created and titled.");

            // Add the second button to the panel
            panel.AddChild(button2.GetButtonHandle());
            Console.WriteLine("Second NSButton added to Panel.");

            // Start the NSApplication run loop
            window.Run();
        }

        Console.WriteLine("Application started.");
    }
}