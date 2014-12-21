using UnityEngine;
using System.Collections;

namespace Orange
{
    public class Convert
    {
        /// <summary>
        /// Konwertuje element typu Vector3 to Vector2 (usuwa współrzędną Z).
        /// </summary>
        /// <param name="element">Element typu Vector3</param>
        /// <returns></returns>
        public static Vector2 Vector3ToVector2(Vector3 element)
        {
            return new Vector2(element.x, element.y);
        }

        /// <summary>
        /// Konwertuje kolor zapisany jako Color32 do formatu hex reprezentowanego przez string.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        /// <summary>
        /// Konwertuje kolor zapisany w systemi hex reprezentowany jako string "AABBCC" do Color
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color(r, g, b, 255);
        }
    }
}