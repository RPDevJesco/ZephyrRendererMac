using System;
using System.Collections.Generic;
using ZephyrRenderer.Mac;

namespace ZephyrRenderer.UI
{
    public class Panel : IDisposable
    {
        private IntPtr _panelHandle;
        private IntPtr _windowHandle;
        private List<IntPtr> _children;
        private bool _disposed = false;

        public Panel(IntPtr windowHandle, double x, double y, double width, double height)
        {
            if (windowHandle == IntPtr.Zero)
                throw new ArgumentException("Invalid window handle.");

            _windowHandle = windowHandle;
            _panelHandle = NSViewWrapper.Create(new CGRect(x, y, width, height));
            _children = new List<IntPtr>();

            // Add the panel to the window
            NSWindowWrapper.AddSubview(_windowHandle, _panelHandle);
        }

        public void AddChild(IntPtr childHandle)
        {
            if (childHandle == IntPtr.Zero)
                throw new ArgumentException("Invalid child handle.");

            _children.Add(childHandle);
            NSViewWrapper.AddSubview(_panelHandle, childHandle);
        }

        public IntPtr GetPanelHandle()
        {
            return _panelHandle;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources if any
                }

                // Dispose unmanaged resources
                if (_panelHandle != IntPtr.Zero)
                {
                    NSViewWrapper.RemoveSubview(_windowHandle, _panelHandle);
                    _panelHandle = IntPtr.Zero;
                }

                _disposed = true;
            }
        }

        ~Panel()
        {
            Dispose(false);
        }
    }
}