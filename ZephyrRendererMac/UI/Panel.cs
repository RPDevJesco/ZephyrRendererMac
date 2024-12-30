using ZephyrRenderer.Mac;

namespace ZephyrRenderer.UI
{
    public class Panel
    {
        private IntPtr _panelHandle;
        private IntPtr _windowHandle;
        private List<IntPtr> _children;

        public Panel(IntPtr windowHandle, double x, double y, double width, double height)
        {
            _windowHandle = windowHandle;
            _panelHandle = NSViewWrapper.Create(new CGRect(x, y, width, height));
            _children = new List<IntPtr>();

            // Add the panel to the window
            NSWindowWrapper.AddSubview(_windowHandle, _panelHandle);
        }

        public void AddChild(IntPtr childHandle)
        {
            _children.Add(childHandle);
            NSViewWrapper.AddSubview(_panelHandle, childHandle);
        }

        public IntPtr GetPanelHandle()
        {
            return _panelHandle;
        }
    }
}