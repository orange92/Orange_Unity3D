using System.Collections;

namespace Orange
{
    public static class Collection
    {
        /// <summary>
        /// Zamienia wartości elementów miejscami.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Informacja czy operacja się powiodła.</returns>
        public static bool Swap<T>(ref T x, ref T y)
        {
            try
            {
                T t = y;
                y = x;
                x = t;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}