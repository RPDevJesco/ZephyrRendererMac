# ZephyrRenderer

ZephyrRenderer is a lightweight .NET application that demonstrates native macOS UI integration using direct Objective-C runtime calls. It provides a bridge between .NET and macOS's native AppKit framework, allowing for the creation of native UI elements and image rendering.

## Features

- Native macOS window and view management
- Button creation with event handling
- Image loading and rendering support (PNG, JPG)
- Custom view implementation for drawing
- Direct Objective-C runtime integration

## Architecture

### Core Components

#### Native Method Wrappers
- `NativeMethods.cs`: P/Invoke declarations for Objective-C runtime and AppKit functions
- `NSApplicationWrapper.cs`: Wrapper for NSApplication functionality
- `NSWindowWrapper.cs`: Wrapper for NSWindow management
- `NSViewWrapper.cs`: Wrapper for NSView operations
- `NSButtonWrapper.cs`: Wrapper for NSButton creation and management

#### UI Elements
- `Window.cs`: High-level window management
- `Panel.cs`: Container for UI elements
- `Button.cs`: Button implementation with event handling
- `Bitmap.cs`: Image loading and rendering
- `BitmapRenderer.cs`: Image rendering management

#### Data Structures
- `CGRect.cs`: Structure definitions for Core Graphics rectangles and sizes

### Event Handling
- `ButtonClickHandler.cs`: Manages button click events and delegates

## Implementation Details

### Window Creation
The application creates an NSWindow with a content view that serves as the main container. UI elements are added to this container through a Panel class that manages the view hierarchy.

### Button Implementation
Buttons are created using NSButton and implement a custom event handling system that bridges Objective-C selectors with .NET events.

### Image Rendering
Images are loaded using NSImage and rendered using Core Graphics contexts within custom NSView subclasses.

## Usage Example

```csharp
// Initialize the application window
var window = new Window();
window.InitializeApp();
window.CreateWindow(100, 100, 1000, 800);

// Create a panel for UI elements
using (var panel = new Panel(window.GetWindowHandle(), 0, 0, 1000, 800))
{
    // Add a button
    using (var button = new Button())
    {
        button.CreateButton(20, 20, 200, 50, "Click Me");
        button.Click += (sender, e) => Console.WriteLine("Button clicked!");
        panel.AddChild(button.GetButtonHandle());
    }

    // Add an image
    using (var bitmapRenderer = new BitmapRenderer(window.GetWindowHandle()))
    {
        bitmapRenderer.LoadBitmap("path/to/image.png");
        var customView = new CustomNSView(
            new CGRect(250, 20, 700, 700),
            bitmapRenderer,
            0, 0);
        panel.AddChild(customView.Handle);
    }

    // Start the application
    window.Run();
}
```

## Technical Notes

### Objective-C Integration
The project uses P/Invoke to interact with the Objective-C runtime directly. This includes:
- Method dispatch using `objc_msgSend`
- Class creation and method addition
- Memory management
- Event handling through selectors

### Memory Management
All native resources are properly managed through:
- IDisposable implementation
- Proper cleanup of native handles
- Reference tracking for Objective-C objects

### Z-Order Management
UI elements can be positioned above or below other elements using NSView's positioning methods, ensuring proper layering of interface elements.

## Development

### Requirements
- .NET 9.0 or later
- macOS 11.0 or later
- Xcode Command Line Tools (for development)

### Building
```bash
dotnet build
```

### Running
```bash
dotnet run
```

## Future Enhancements
- Additional UI control support
- Enhanced image manipulation capabilities
- Support for more AppKit features
- Improved error handling and recovery
- Documentation improvements

## Contributing
Contributions are welcome! Please feel free to submit pull requests.
