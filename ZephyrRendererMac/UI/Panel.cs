using System;
using System.Collections.Generic;
using ZephyrRenderer.Mac;

public class Panel : IDisposable
{
    private IntPtr _panelHandle;
    private IntPtr _windowHandle;
    private List<IntPtr> _children;
    private bool _disposed = false;

    public Panel(IntPtr windowHandle, double x, double y, double width, double height)
    {
        Console.WriteLine($"Creating panel at ({x}, {y}) with size {width}x{height}");
        if (windowHandle == IntPtr.Zero)
            throw new ArgumentException("Invalid window handle.");

        _windowHandle = windowHandle;
        _children = new List<IntPtr>();
        
        // Create the NSView
        var viewClass = NativeMethods.objc_getClass("NSView");
        var allocSelector = NativeMethods.sel_registerName("alloc");
        var initSelector = NativeMethods.sel_registerName("initWithFrame:");
        
        _panelHandle = NativeMethods.objc_msgSend(viewClass, allocSelector);
        
        // Create frame rect
        var frame = new CGRect(x, y, width, height);
        _panelHandle = NativeMethods.objc_msgSend_Init(_panelHandle, initSelector, frame);

        if (_panelHandle == IntPtr.Zero)
        {
            throw new Exception("Failed to create NSView");
        }

        // Set view to be layer-backed
        var setWantsLayerSelector = NativeMethods.sel_registerName("setWantsLayer:");
        NativeMethods.objc_msgSend_bool(_panelHandle, setWantsLayerSelector, true);

        // Create a light gray color
        var colorClass = NativeMethods.objc_getClass("NSColor");
        var colorSelector = NativeMethods.sel_registerName("colorWithDeviceRed:green:blue:alpha:");
        var color = NativeMethods.objc_msgSend(colorClass, colorSelector, 0.9f, 0.9f, 0.9f, 1.0f);
        
        // Set the background color
        var setBackgroundColorSelector = NativeMethods.sel_registerName("setBackgroundColor:");
        NativeMethods.objc_msgSend(_panelHandle, setBackgroundColorSelector, color);

        // Add to window's content view
        var contentView = GetParentHandle();
        if (contentView == IntPtr.Zero)
        {
            throw new Exception("Failed to get content view");
        }

        NSViewWrapper.AddSubview(contentView, _panelHandle);
        Console.WriteLine($"Panel {_panelHandle} added to content view {contentView}");
    }

    public void AddChild(IntPtr childHandle, bool above = true)
    {
        if (childHandle == IntPtr.Zero)
            throw new ArgumentException("Invalid child handle.");

        Console.WriteLine($"Adding child {childHandle} to panel {_panelHandle}");
        _children.Add(childHandle);

        // Enable layer-backed view for child
        var setWantsLayerSelector = NativeMethods.sel_registerName("setWantsLayer:");
        NativeMethods.objc_msgSend_bool(childHandle, setWantsLayerSelector, true);
        
        var addSubviewSelector = NativeMethods.sel_registerName("addSubview:");
        NativeMethods.objc_msgSend(_panelHandle, addSubviewSelector, childHandle);
        
        Console.WriteLine("Child added successfully");
    }

    public IntPtr GetPanelHandle() => _panelHandle;

    // Protected methods for derived classes
    protected IntPtr GetParentHandle()
    {
        var contentViewSelector = NativeMethods.sel_registerName("contentView");
        return NativeMethods.objc_msgSend(_windowHandle, contentViewSelector);
    }

    protected void SetHandle(IntPtr handle)
    {
        if (handle != IntPtr.Zero)
        {
            // Clean up old handle if needed
            if (_panelHandle != IntPtr.Zero)
            {
                var releaseSelector = NativeMethods.sel_registerName("release");
                NativeMethods.objc_msgSend(_panelHandle, releaseSelector);
            }
            _panelHandle = handle;
        }
    }
    
    protected void SetFrameSize(IntPtr viewHandle, double width, double height)
    {
        var sizeSelector = NativeMethods.sel_registerName("setFrameSize:");
        var size = new CGSize { width = width, height = height };
        NativeMethods.objc_msgSend_Size(viewHandle, sizeSelector, size);
    }

    protected void SetFrameOrigin(IntPtr viewHandle, double x, double y)
    {
        var originSelector = NativeMethods.sel_registerName("setFrameOrigin:");
        var point = new CGPoint(x, y);
        NativeMethods.objc_msgSend_Point(viewHandle, originSelector, point);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                foreach (var child in _children)
                {
                    NSViewWrapper.RemoveSubview(_panelHandle, child);
                }
                _children.Clear();
            }

            if (_panelHandle != IntPtr.Zero)
            {
                NSViewWrapper.RemoveSubview(GetParentHandle(), _panelHandle);
                var releaseSelector = NativeMethods.sel_registerName("release");
                NativeMethods.objc_msgSend(_panelHandle, releaseSelector);
                _panelHandle = IntPtr.Zero;
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Panel()
    {
        Dispose(false);
    }
}