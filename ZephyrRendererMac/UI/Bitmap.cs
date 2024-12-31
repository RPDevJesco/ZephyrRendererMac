using System;
using System.Runtime.InteropServices;
using ZephyrRenderer.Mac;

namespace ZephyrRenderer.UI
{
    public class Bitmap : IDisposable
    {
        private IntPtr _imageHandle;

        public void Load(string filePath)
        {
            Console.WriteLine($"Loading image from file: {filePath}");
            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine("File not found.");
                throw new System.IO.FileNotFoundException("The specified file was not found.", filePath);
            }

            IntPtr nsStringClass = NativeMethods.objc_getClass("NSString");
            IntPtr stringWithUTF8StringSelector = NativeMethods.sel_registerName("stringWithUTF8String:");
            IntPtr filePathHandle = NativeMethods.objc_msgSend(nsStringClass, stringWithUTF8StringSelector, filePath);

            if (filePathHandle == IntPtr.Zero)
            {
                Console.WriteLine("Failed to create NSString from file path.");
                throw new Exception("Failed to create NSString from file path.");
            }

            IntPtr nsImageClass = NativeMethods.objc_getClass("NSImage");
            IntPtr allocSelector = NativeMethods.sel_registerName("alloc");
            IntPtr initWithContentsOfFileSelector = NativeMethods.sel_registerName("initWithContentsOfFile:");

            IntPtr imageAllocHandle = NativeMethods.objc_msgSend(nsImageClass, allocSelector);
            if (imageAllocHandle == IntPtr.Zero)
            {
                Console.WriteLine("Failed to allocate NSImage.");
                throw new Exception("Failed to allocate NSImage.");
            }

            _imageHandle = NativeMethods.objc_msgSend(imageAllocHandle, initWithContentsOfFileSelector, filePathHandle);
            if (_imageHandle == IntPtr.Zero)
            {
                Console.WriteLine("Failed to initialize NSImage with file path.");
                throw new Exception("Failed to initialize NSImage with file path.");
            }

            Console.WriteLine($"Bitmap loaded from {filePath}");
        }

        public void Draw(IntPtr context, float x, float y)
        {
            Console.WriteLine("Starting draw process...");
            if (_imageHandle == IntPtr.Zero)
            {
                Console.WriteLine("Bitmap has not been loaded.");
                throw new InvalidOperationException("Bitmap has not been loaded.");
            }

            if (context == IntPtr.Zero)
            {
                Console.WriteLine("Invalid graphics context.");
                throw new ArgumentException("Invalid graphics context.");
            }

            CGSize size = NativeMethods.NSImage_size(_imageHandle);
            Console.WriteLine($"Bitmap size: {size.width}x{size.height}");

            var rect = new CGRect(x, y, size.width, size.height);
            IntPtr imageRef = NativeMethods.NSImage_CGImageForProposedRect(_imageHandle, ref rect, IntPtr.Zero, IntPtr.Zero);

            if (imageRef == IntPtr.Zero)
            {
                Console.WriteLine("Failed to get CGImage from NSImage.");
                throw new Exception("Failed to get CGImage from NSImage.");
            }

            Console.WriteLine($"Drawing image at ({x}, {y}) with size {size.width}x{size.height}");
            NativeMethods.CGContextDrawImage(context, rect, imageRef);
            Console.WriteLine($"Bitmap drawn at ({x}, {y})");
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing Bitmap...");
            if (_imageHandle != IntPtr.Zero)
            {
                Marshal.Release(_imageHandle);
                _imageHandle = IntPtr.Zero;
            }
            Console.WriteLine("Bitmap disposed.");
        }
    }
}