using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public struct ColorHex
    {
        public string HexCode { get; }

        private ColorHex(string hexCode)
        {
            HexCode = hexCode.ToUpperInvariant();
        }

        public static ColorHex FromHex(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                throw new ArgumentException("Color hex code cannot be empty.", nameof(hex));

            if (!System.Text.RegularExpressions.Regex
                    .IsMatch(hex, "^#?[0-9A-Fa-f]{6}$"))
            {
                throw new ArgumentException("Invalid hex color format (expected #RRGGBB or RRGGBB).", nameof(hex));
            }

            hex = hex.StartsWith("#") ? hex : $"#{hex}";
            return new ColorHex(hex);
        }

        public static ColorHex FromRgb(byte r, byte g, byte b)
            => new ColorHex($"#{r:X2}{g:X2}{b:X2}");

        public override string ToString() => HexCode;


        public bool Equals(ColorHex other) => HexCode == other.HexCode;
        public override bool Equals(object? obj) => obj is ColorHex other && Equals(other);
        public override int GetHashCode() => HexCode.GetHashCode();

        public static bool operator ==(ColorHex left, ColorHex right) => left.Equals(right);
        public static bool operator !=(ColorHex left, ColorHex right) => !left.Equals(right);

        public static implicit operator string(ColorHex color) => color.HexCode;
        public static explicit operator ColorHex(string hex) => FromHex(hex);

        public static ColorHex Red => FromHex("#FF0000");
        public static ColorHex Green => FromHex("#00FF00");
        public static ColorHex Blue => FromHex("#0000FF");
    }
}
