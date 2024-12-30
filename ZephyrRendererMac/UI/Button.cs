using System;
using System.Runtime.InteropServices;
using ZephyrRenderer.Mac;

namespace ZephyrRenderer.UI
{
    public class Button : IDisposable
    {
        private IntPtr _buttonHandle;
        private IntPtr _targetHandle;

        // Define the event for button clicks
        public event EventHandler Click;

        public Button()
        {
            ButtonClickHandler.ButtonClickedEvent += OnClick;
        }

        public void CreateButton(double x, double y, double width, double height, string title)
        {
            _buttonHandle = InitializeButton(x, y, width, height, title);
            _targetHandle = ButtonClickHandler.Create();
            NSButtonWrapper.SetTarget(_buttonHandle, _targetHandle, "buttonClicked:");
            Console.WriteLine($"NSButton target and action set for button '{title}' at ({x}, {y}, {width}, {height}).");
        }

        public IntPtr GetButtonHandle()
        {
            if (_buttonHandle == IntPtr.Zero)
                throw new InvalidOperationException("Button has not been created yet.");
            return _buttonHandle;
        }

        private IntPtr InitializeButton(double x, double y, double width, double height, string title)
        {
            var button = NSButtonWrapper.Create(new CGRect(x, y, width, height), title);
            Console.WriteLine($"NSButton created and titled '{title}' at ({x}, {y}, {width}, {height}).");
            return button;
        }

        // Method to be called by the ButtonClickHandler
        private void OnClick(object sender, EventArgs e)
        {
            Console.WriteLine($"Button.OnClick invoked for button '{GetButtonHandle()}'.");
            Click?.Invoke(this, e);
        }

        public void Dispose()
        {
            if (_targetHandle != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_targetHandle);
                _targetHandle = IntPtr.Zero;
            }
            ButtonClickHandler.ButtonClickedEvent -= OnClick;
        }
    }
}