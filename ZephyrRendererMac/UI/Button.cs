using System;
using System.Runtime.InteropServices;
using ZephyrRenderer.Mac;

namespace ZephyrRenderer.UI
{
    public class Button : IDisposable
    {
        private IntPtr _buttonHandle;
        private IntPtr _targetHandle;

        public event EventHandler Click;

        public Button()
        {
            ButtonClickHandler.ButtonClickedEvent += OnClick;
        }

        public void CreateButton(double x, double y, double width, double height, string title)
        {
            Console.WriteLine($"Creating button at ({x}, {y}) with size {width}x{height}");
            
            IntPtr buttonClass = NativeMethods.objc_getClass("NSButton");
            IntPtr allocSelector = NativeMethods.sel_registerName("alloc");
            IntPtr initWithFrameSelector = NativeMethods.sel_registerName("initWithFrame:");
            
            _buttonHandle = NativeMethods.objc_msgSend(buttonClass, allocSelector);
            var frame = new CGRect(x, y, width, height);
            _buttonHandle = NativeMethods.objc_msgSend(_buttonHandle, initWithFrameSelector, frame);

            if (_buttonHandle == IntPtr.Zero)
            {
                throw new Exception("Failed to create NSButton");
            }

            // Set button properties
            SetButtonTitle(title);
            SetButtonStyle();
            
            // Set up target/action
            _targetHandle = ButtonClickHandler.Create();
            NSButtonWrapper.SetTarget(_buttonHandle, _targetHandle, "buttonClicked:");

            // Enable layer-backed view
            var setWantsLayerSelector = NativeMethods.sel_registerName("setWantsLayer:");
            NativeMethods.objc_msgSend_bool(_buttonHandle, setWantsLayerSelector, true);

            Console.WriteLine($"Button created successfully: {_buttonHandle}");
        }

        private void SetButtonTitle(string title)
        {
            var setTitleSelector = NativeMethods.sel_registerName("setTitle:");
            
            // Create NSString from title
            var nsStringClass = NativeMethods.objc_getClass("NSString");
            var stringWithUTF8StringSelector = NativeMethods.sel_registerName("stringWithUTF8String:");
            var titleString = NativeMethods.objc_msgSend(nsStringClass, stringWithUTF8StringSelector, title);
            
            NativeMethods.objc_msgSend(_buttonHandle, setTitleSelector, titleString);

            // Set font to system font size 13
            var fontClass = NativeMethods.objc_getClass("NSFont");
            var systemFontSelector = NativeMethods.sel_registerName("systemFontOfSize:");
            var font = NativeMethods.objc_msgSend_Font(fontClass, systemFontSelector, 13.0f);
            
            var setFontSelector = NativeMethods.sel_registerName("setFont:");
            NativeMethods.objc_msgSend(_buttonHandle, setFontSelector, font);
        }

        private void SetButtonStyle()
        {
            // Set bezel style to rounded
            var setBezelStyleSelector = NativeMethods.sel_registerName("setBezelStyle:");
            NativeMethods.objc_msgSend_ulong(_buttonHandle, setBezelStyleSelector, 1); // NSRoundedBezelStyle

            // Set button type to momentary push in
            var setButtonTypeSelector = NativeMethods.sel_registerName("setButtonType:");
            NativeMethods.objc_msgSend_ulong(_buttonHandle, setButtonTypeSelector, 1); // NSMomentaryPushInButton
        }

        public IntPtr GetButtonHandle()
        {
            if (_buttonHandle == IntPtr.Zero)
                throw new InvalidOperationException("Button has not been created yet.");
            return _buttonHandle;
        }

        private void OnClick(object sender, EventArgs e)
        {
            Console.WriteLine($"Button.OnClick invoked for button '{GetButtonHandle()}'.");
            Click?.Invoke(this, e);
        }

        public void Dispose()
        {
            ButtonClickHandler.ButtonClickedEvent -= OnClick;

            if (_targetHandle != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_targetHandle);
                _targetHandle = IntPtr.Zero;
            }

            if (_buttonHandle != IntPtr.Zero)
            {
                var releaseSelector = NativeMethods.sel_registerName("release");
                NativeMethods.objc_msgSend(_buttonHandle, releaseSelector);
                _buttonHandle = IntPtr.Zero;
            }
        }
    }
}