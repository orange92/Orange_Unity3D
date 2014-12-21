using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orange
{
    public static class Math
    {
        public static float MaxLimit(float number, float maxLimit)
        {
            return number > maxLimit ? maxLimit : number;
        }
        public static float MinLimit(float number, float minLimit)
        {
            return number < minLimit ? minLimit : number;
        }
        public static float MinMaxLimit(float number, float minLimit, float maxLimit)
        {
            if (minLimit > maxLimit) throw new Exception("Min limit is greater then max limit.");
            if(number < minLimit) return minLimit;
            if(number > maxLimit) return maxLimit;
            return number;
        }
        public static int MaxLimit(int number, int maxLimit)
        {
            return number > maxLimit ? maxLimit : number;
        }
        public static int MinLimit(int number, int minLimit)
        {
            return number < minLimit ? minLimit : number;
        }
        public static int MinMaxLimit(int number, int minLimit, int maxLimit)
        {
            if (minLimit > maxLimit) throw new Exception("Min limit is greater then max limit.");
            if (number < minLimit) return minLimit;
            if (number > maxLimit) return maxLimit;
            return number;
        }


        /// <summary>
        /// Pobiera ilość obrotów na minutę i zwraca prędkość w km/h
        /// </summary>
        /// <param name="rpm">Ilość obrotów na minutę.</param>
        /// <param name="radius">Promień koła w metrach [m]</param>
        /// <returns>prędkość w [km/h]</returns>
        public static float RpmToKmH(float rpm, float radius)
        {
            float length = 2 * UnityEngine.Mathf.PI * radius;
            float distance = length * rpm;
            float speed = distance * (0.001f / (1f / 60f));
            return speed;
        }
        
        /// <summary>
        /// Oblicza kont wychylenia jaki należy uczynić by z konta aktualnego dojść do konta docelowego najmniejszym kosztem.
        /// Przykład current=350, direction=340, wynik=-10 (należy zmniejszyć aktualny kąt o 10 by uzyskać cel
        /// </summary>
        /// <param name="current">Aktualny kąt [0 - 360].</param>
        /// <param name="direction">Kąt oczekiwany [0 - 360].</param>
        /// <returns>Kąt [-180 - 180]</returns>
 
        public static float DeflectionAngle(float current, float direction)
        {
            if (current < direction) current += 360;
            float buf = current - direction;
            if (buf < 180) return -buf;
            else return (360 - buf);
        }
    }
}
