namespace ZephyrRenderer.Mac
{
public class NSColorWrapper
    {
        private static readonly IntPtr colorClass = NativeMethods.objc_getClass("NSColor");

        // Create color from base Color object
        public static IntPtr CreateColor(float r, float g, float b, float a)
        {
            Color color = new Color(r, g, b, a);
            color.ToMacOS();
            var colorSelector = NativeMethods.sel_registerName("colorWithDeviceRed:green:blue:alpha:");
            return NativeMethods.objc_msgSend(colorClass, colorSelector, r, g, b, a );
        }

        // Create color from hex string
        public static IntPtr CreateFromHex(string hexColor, bool allowTransparency = false)
        {
            var color = ParseHexToColor(hexColor);
            // Convert the 0-255 values to 0-1 range for CreateColor
            float r = color.Red / 255f;
            float g = color.Green / 255f;
            float b = color.Blue / 255f;
            float a = color.Alpha / 255f;

            return CreateColor(r, g, b, a);
        }

        // Parse a hex string to a Color object
        private static Color ParseHexToColor(string hexColor)
        {
            if (string.IsNullOrEmpty(hexColor))
                throw new ArgumentException("Hex color string cannot be null or empty");

            hexColor = hexColor.TrimStart('#');
            byte r, g, b, a = 255;

            if (hexColor.Length == 6 || hexColor.Length == 8)
            {
                r = Convert.ToByte(hexColor.Substring(0, 2), 16);
                g = Convert.ToByte(hexColor.Substring(2, 2), 16);
                b = Convert.ToByte(hexColor.Substring(4, 2), 16);

                if (hexColor.Length == 8)
                    a = Convert.ToByte(hexColor.Substring(6, 2), 16);
            }
            else
            {
                throw new ArgumentException("Hex color must be in #RRGGBB or #RRGGBBAA format");
            }

            return new Color(r, g, b, a); // 0-255 range
        }


        // Set color for drawing
        public static void SetColor(Color color)
        {
            var nsColor = CreateColor(color.Red, color.Green, color.Blue, color.Alpha);
            NativeMethods.objc_msgSend(nsColor, NativeMethods.sel_registerName("set"));
        }

        // Predefined colors
        public static IntPtr Black => CreateColor(0, 0, 0,1);
        public static IntPtr White => CreateColor(255, 255, 255,1);
        public static IntPtr Red => CreateColor(255, 0, 0,1);
        public static IntPtr Green => CreateColor(0, 255, 0,1);
        public static IntPtr Blue => CreateColor(0, 0, 255,1);
        public static IntPtr Clear => CreateColor(0, 0, 0, 0);

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