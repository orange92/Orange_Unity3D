using UnityEngine;
using System.Collections;

namespace Orange
{
    namespace Types
    {
        /// <summary>
        /// Wektor (x,y) reprezentowany przez liczby z zakresu integer.
        /// </summary>
        [System.Serializable]
        public struct VectorInt2
        {
            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public static bool operator ==(VectorInt2 lhs, VectorInt2 rhs)
            {
                return (lhs.x == rhs.x && lhs.y == rhs.y);
            }

            public static bool operator !=(VectorInt2 lhs, VectorInt2 rhs)
            {
                return (lhs.x != rhs.x || lhs.y != rhs.y);
            }

            public VectorInt2(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public override string ToString()
            {
                return "(" + x + "," + y + ")";
            }
            public int x;
            public int y;
        }
    }
}