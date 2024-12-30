using ZephyrRenderer.Mac;

namespace ZephyrRenderer.UI
{
    public class Button
    {
        private IntPtr _buttonHandle;

        public void CreateButton(double x, double y, double width, double height, string title)
        {
            _buttonHandle = InitializeButton(x, y, width, height, title);
            IntPtr target = ButtonClickHandler.Create();
            NSButtonWrapper.SetTarget(_buttonHandle, target, "buttonClicked:");
            Console.WriteLine("NSButton target and action set.");
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
            Console.WriteLine("NSButton created and titled.");
            return button;
        }
    }
}