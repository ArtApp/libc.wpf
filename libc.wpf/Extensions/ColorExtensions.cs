using System.Windows.Media;

namespace libc.wpf.Extensions {
    public static class ColorExtensions {
        public static string ColorToHex(this Color c) {
            return $"#{c.A:X2}{c.R:X2}{c.G:X2}{c.B:X2}";
        }
        public static Color ColorFromHex(this string hex) {
            return (Color)ColorConverter.ConvertFromString(hex);
        }
    }
}
