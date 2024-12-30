﻿using System;
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

        // Create the first button
        var button1 = new Button();
        button1.CreateButton(50, 50, 200, 50, "Click Me");
        Console.WriteLine("First NSButton created and titled.");

        // Add the first button to the window
        NSWindowWrapper.AddSubview(window.GetWindowHandle(), button1.GetButtonHandle());
        Console.WriteLine("First NSButton added to NSWindow.");

        // Create the second button
        var button2 = new Button();
        button2.CreateButton(50, 150, 200, 50, "Another Button");
        Console.WriteLine("Second NSButton created and titled.");

        // Add the second button to the window
        NSWindowWrapper.AddSubview(window.GetWindowHandle(), button2.GetButtonHandle());
        Console.WriteLine("Second NSButton added to NSWindow.");

        // Start the NSApplication run loop
        window.Run();

        Console.WriteLine("Application started.");
    }
}