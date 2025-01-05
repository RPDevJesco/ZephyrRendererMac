namespace ZephyrRenderer.Mac
{
    public class Color
    {
        public int Red { get; }
        public int Green { get; }
        public int Blue { get; }
        public int Alpha { get; }

        // Constructor accepting 0-255 values
        public Color(int red, int green, int blue, int alpha = 255)
        {
            Red = Clamp(red, 0, 255);
            Green = Clamp(green, 0, 255);
            Blue = Clamp(blue, 0, 255);
            Alpha = Clamp(alpha, 0, 255);
        }

        // Constructor accepting 0-1 values (macOS style)
        public Color(float red, float green, float blue, float alpha = 1.0f)
        {
            Red = (int)(Clamp(red, 0f, 1f) * 255);
            Green = (int)(Clamp(green, 0f, 1f) * 255);
            Blue = (int)(Clamp(blue, 0f, 1f) * 255);
            Alpha = (int)(Clamp(alpha, 0f, 1f) * 255);
        }

        // Convert to macOS (0-1 range)
        public (float Red, float Green, float Blue, float Alpha) ToMacOS()
        {
            return (Red / 255f, Green / 255f, Blue / 255f, Alpha / 255f);
        }

        // Convert to hex string
        public string ToHex()
        {
            return $"#{Red:X2}{Green:X2}{Blue:X2}{Alpha:X2}";
        }

        private static int Clamp(int value, int min, int max) => Math.Max(min, Math.Min(max, value));
        private static float Clamp(float value, float min, float max) => Math.Max(min, Math.Min(max, value));
    }
}