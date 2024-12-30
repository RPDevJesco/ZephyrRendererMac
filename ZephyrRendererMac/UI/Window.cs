using ZephyrRenderer.Mac;

namespace ZephyrRenderer.UI
{
    public class Window
    {
        private IntPtr _appHandle;
        private IntPtr _windowHandle;

        public void InitializeApp()
        {
            InitializeNSApplication();
            _appHandle = CreateAndActivateNSApplication();
        }

        public void Run()
        {
            if (_appHandle == IntPtr.Zero)
                throw new InvalidOperationException("NSApplication has not been initialized.");

            NSApplicationWrapper.Run(_appHandle);
            Console.WriteLine("NSApplication run loop started.");
        }

        public void CreateWindow(double x, double y, double width, double height)
        {
            _windowHandle = CreateAndDisplayWindow(x, y, width, height);
        }

        public IntPtr GetWindowHandle()
        {
            if (_windowHandle == IntPtr.Zero)
                throw new InvalidOperationException("Window has not been created yet.");
            return _windowHandle;
        }

        private void InitializeNSApplication()
        {
            Console.WriteLine("Loading NSApplication...");
            NSApplicationWrapper.Load();
            Console.WriteLine("NSApplication loaded.");
        }

        private IntPtr CreateAndActivateNSApplication()
        {
            IntPtr app = NSApplicationWrapper.Create();
            Console.WriteLine("NSApplication instance created.");

            NSApplicationWrapper.Activate(app);
            Console.WriteLine("NSApplication activated.");

            return app;
        }

        private IntPtr CreateAndDisplayWindow(double x, double y, double width, double height)
        {
            var window = NSWindowWrapper.Create(new CGRect(x, y, width, height));
            Console.WriteLine("NSWindow created and displayed.");
            return window;
        }
    }
}