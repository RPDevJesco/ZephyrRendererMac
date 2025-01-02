using System;
using System.Drawing;

namespace ZephyrRenderer.Mac
{
    public class NSColorWrapper
    {
        private static readonly IntPtr colorClass = NativeMethods.objc_getClass("NSColor");
        
        // Create color from RGBA values (0-1 range)
        public static IntPtr CreateColor(float r, float g, float b, float a = 1.0f, bool allowTransparency = false)
        {
            var colorSelector = NativeMethods.sel_registerName("colorWithDeviceRed:green:blue:alpha:");
            var color = NativeMethods.objc_msgSend(colorClass, colorSelector, r, g, b, allowTransparency ? a : 1.0f);
            return color;
        }

        // Create color from hex string (#RRGGBB or #RRGGBBAA)
        public static IntPtr CreateFromHex(string hexColor, bool allowTransparency = false)
        {
            try
            {
                if (string.IsNullOrEmpty(hexColor))
                    throw new ArgumentException("Hex color string cannot be null or empty");

                // Remove # if present
                hexColor = hexColor.TrimStart('#');

                byte r, g, b, a = 255;

                if (hexColor.Length == 6 || hexColor.Length == 8)
                {
                    r = Convert.ToByte(hexColor[..2], 16);
                    g = Convert.ToByte(hexColor.Substring(2, 2), 16);
                    b = Convert.ToByte(hexColor.Substring(4, 2), 16);

                    if (hexColor.Length == 8)
                    {
                        a = Convert.ToByte(hexColor.Substring(6, 2), 16);
                    }
                }
                else
                {
                    throw new ArgumentException("Hex color must be in #RRGGBB or #RRGGBBAA format");
                }

                // Convert to 0-1 range
                return CreateColor(r / 255f, g / 255f, b / 255f, a / 255f, allowTransparency);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating color from hex: {ex.Message}");
                return CreateColor(0, 0, 0); // Return black as fallback
            }
        }

        // Common predefined colors
        public static IntPtr Black => CreateColor(0, 0, 0);
        public static IntPtr White => CreateColor(1, 1, 1);
        public static IntPtr Red => CreateColor(1, 0, 0);
        public static IntPtr Green => CreateColor(0, 1, 0);
        public static IntPtr Blue => CreateColor(0, 0, 1);
        public static IntPtr Clear => CreateColor(0, 0, 0, 0, true);

        // Set the current color for drawing
        public static void SetColor(IntPtr color)
        {
            if (color != IntPtr.Zero)
            {
                NativeMethods.objc_msgSend(color, NativeMethods.sel_registerName("set"));
            }
        }

        // Helper method to directly set color from RGBA
        public static void SetColorRGBA(float r, float g, float b, float a = 1.0f, bool allowTransparency = false)
        {
            var color = CreateColor(r, g, b, a, allowTransparency);
            SetColor(color);
        }

        // Helper method to directly set color from hex
        public static void SetColorHex(string hexColor, bool allowTransparency = false)
        {
            var color = CreateFromHex(hexColor, allowTransparency);
            SetColor(color);
        }

        // Create a slightly darker version of a color
        public static IntPtr Darken(IntPtr color, float amount = 0.1f)
        {
            // Get the current RGB components
            var hueSelector = NativeMethods.sel_registerName("hueComponent");
            var saturationSelector = NativeMethods.sel_registerName("saturationComponent");
            var brightnessSelector = NativeMethods.sel_registerName("brightnessComponent");
    
            var h = (float)NativeMethods.objc_msgSend_Double(color, hueSelector);
            var s = (float)NativeMethods.objc_msgSend_Double(color, saturationSelector);
            var b = (float)NativeMethods.objc_msgSend_Double(color, brightnessSelector);

            // Create new color with reduced brightness
            var colorWithHSBSelector = NativeMethods.sel_registerName("colorWithHue:saturation:brightness:alpha:");
            return NativeMethods.objc_msgSend_HSBA(colorClass, colorWithHSBSelector, 
                h, s, Math.Max(0f, b - amount), 1.0f);
        }

        // Create a slightly lighter version of a color
        public static IntPtr Lighten(IntPtr color, float amount = 0.1f)
        {
            var hueSelector = NativeMethods.sel_registerName("hueComponent");
            var saturationSelector = NativeMethods.sel_registerName("saturationComponent");
            var brightnessSelector = NativeMethods.sel_registerName("brightnessComponent");
    
            var h = (float)NativeMethods.objc_msgSend_Double(color, hueSelector);
            var s = (float)NativeMethods.objc_msgSend_Double(color, saturationSelector);
            var b = (float)NativeMethods.objc_msgSend_Double(color, brightnessSelector);

            var colorWithHSBSelector = NativeMethods.sel_registerName("colorWithHue:saturation:brightness:alpha:");
            return NativeMethods.objc_msgSend_HSBA(colorClass, colorWithHSBSelector, 
                h, s, Math.Min(1f, b + amount), 1.0f);
        }
    }
}