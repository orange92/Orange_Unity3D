using UnityEngine;
using System.Collections;

namespace Orange
{

    public static class Input
    {
		/// <summary>
		/// Wirtualny TrackBall. Wirtualny dwuosiowy analog na ekranie sterowany palcem.
        /// Położenie trackballa jest definiowane zakresem i może on zmieniać swoje położenie w ramach zakresu.
        /// Rozmiar trackballa jest stały.
		/// </summary>
		public class TrackBall
        {
            /// <summary>
            /// Dodanie punktu wykrywanie trackballa. 
            /// Rozmiar ekranu jest przeskalowany do wartości float z zakresu [0 : 1]
            /// Punktem (0,0) jest lewy górny narożnik ekranu.
            /// </summary>
            /// <param name="leftLimit">Ogrniczenie z lewej strony [0 : 1]</param>
            /// <param name="rightLimit">Ograniczenie z prawej strony [0 : 1]</param>
            /// <param name="topLimit">Ograniczenie z góry [0 : 1]</param>
            /// <param name="bottomLimit">Ograniczenie z dołu [0 : 1]</param>
            /// <param name="radius">Promień maksymalnego wychylenia trackballa</param>
            public TrackBall(float leftLimit, float rightLimit, float topLimit, float bottomLimit, float radius)
            {
                this.leftLimit = leftLimit;
                this.rightLimit = rightLimit;
                this.bottomLimit = bottomLimit;
                this.topLimit = topLimit;
                this.radius = radius;
            }

            /// <summary>
            /// Informacja czy user totyka trackballa.
            /// </summary>
            private bool m_inTouch = false;

            /// <summary>
            /// Pozycja punktu [0;0] dla trackballa,
            /// </summary>
            private Vector2 m_position0;

            /// <summary>
            /// ID palca który dotyka trackballa.
            /// </summary>
            private int m_fingerID;
            
            /// <summary>
            /// lewa krawędź wykrywania.
            /// </summary>
            public float leftLimit;

            /// <summary>
            /// Prawa krawędź wykrywania.
            /// </summary>
            public float rightLimit;

            /// <summary>
            /// Górna krawędź wykrywania.
            /// </summary>
            public float topLimit;

            /// <summary>
            /// Dolna krawędź wykrywania.
            /// </summary>
            public float bottomLimit;

            /// <summary>
            /// Promień rozmiaru trackballa.
            /// </summary>
            public float radius;

            /// <summary>
            /// Pozycja wychylenia trackballa.
            /// </summary>
            /// <returns>Wspułczynnik wychylenia [-1,1].</returns>
            public Vector2 GetAxis()
            {
                // User dotyka trackballa.
                if (m_inTouch)
                {
                   for (int i = 0; i < UnityEngine.Input.touchCount; i++)
                   {
                       // Jeśli palec się zgadza
                       if (UnityEngine.Input.touches[i].fingerId == m_fingerID)
                       {
                            Vector2 position = UnityEngine.Input.touches[i].position;
                            position = new Vector2(position.x / Screen.width, position.y / Screen.height);
                            Vector2 vector = new Vector2(position.x - m_position0.x , position.y - m_position0.y);
                            vector = new Vector2(Math.MinMaxLimit(vector.x,-radius,radius), Math.MinMaxLimit(vector.y,-radius,radius));
                            vector = new Vector2((vector.x / radius) * 10f, (vector.y / radius) * 10f);
                            return vector;
                       }
                    }
                   m_inTouch = false;
                }
                // User nie dotyka trackballa
                else
                {
                    for (int i = 0; i < UnityEngine.Input.touchCount; i++)
                    {
                        Vector2 position = UnityEngine.Input.touches[i].position;
                        position = new Vector2(position.x / Screen.width, position.y / Screen.height);
                        if (position.x > leftLimit && position.x < rightLimit && position.y > topLimit && position.y < bottomLimit)
                        {
                            m_fingerID = UnityEngine.Input.touches[i].fingerId;
                            m_position0 = position;
                            m_inTouch = true;
                        }
                    }
                }
                return new Vector2(0, 0);
            }
        }
	
        /// <summary>
        /// Pobiera klawisz z klawiatury i dodaje znak do ciągu.
        /// </summary>
        /// <param name="text">Stary tekst.</param>
        /// <param name="preg">Typ znaków zdefiniowany przez wyrażenie regularne</param>
        /// <returns>Zwraca zaktualizowany tekst</returns>
        static public string KeyboardReadString(string text, string preg)
        {
            string buf = "";

            if (UnityEngine.Input.GetKeyDown(KeyCode.A)) buf = "a";
            if (UnityEngine.Input.GetKeyDown(KeyCode.B)) buf = "b";
            if (UnityEngine.Input.GetKeyDown(KeyCode.C)) buf = "c";
            if (UnityEngine.Input.GetKeyDown(KeyCode.D)) buf = "d";
            if (UnityEngine.Input.GetKeyDown(KeyCode.E)) buf = "e";
            if (UnityEngine.Input.GetKeyDown(KeyCode.F)) buf = "f";
            if (UnityEngine.Input.GetKeyDown(KeyCode.G)) buf = "g";
            if (UnityEngine.Input.GetKeyDown(KeyCode.H)) buf = "h";
            if (UnityEngine.Input.GetKeyDown(KeyCode.I)) buf = "i";
            if (UnityEngine.Input.GetKeyDown(KeyCode.J)) buf = "j";
            if (UnityEngine.Input.GetKeyDown(KeyCode.K)) buf = "k";
            if (UnityEngine.Input.GetKeyDown(KeyCode.L)) buf = "l";
            if (UnityEngine.Input.GetKeyDown(KeyCode.M)) buf = "m";
            if (UnityEngine.Input.GetKeyDown(KeyCode.N)) buf = "n";
            if (UnityEngine.Input.GetKeyDown(KeyCode.O)) buf = "o";
            if (UnityEngine.Input.GetKeyDown(KeyCode.P)) buf = "p";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Q)) buf = "q";
            if (UnityEngine.Input.GetKeyDown(KeyCode.R)) buf = "r";
            if (UnityEngine.Input.GetKeyDown(KeyCode.S)) buf = "s";
            if (UnityEngine.Input.GetKeyDown(KeyCode.T)) buf = "t";
            if (UnityEngine.Input.GetKeyDown(KeyCode.U)) buf = "u";
            if (UnityEngine.Input.GetKeyDown(KeyCode.V)) buf = "v";
            if (UnityEngine.Input.GetKeyDown(KeyCode.W)) buf = "w";
            if (UnityEngine.Input.GetKeyDown(KeyCode.X)) buf = "x";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Y)) buf = "y";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Z)) buf = "z";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha0)) buf = "0";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) buf = "1";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2)) buf = "2";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3)) buf = "3";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4)) buf = "4";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5)) buf = "5";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha6)) buf = "6";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha7)) buf = "7";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha8)) buf = "8";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha9)) buf = "9";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space)) buf = " ";
            if (UnityEngine.Input.GetKeyDown(KeyCode.Underscore)) buf = "_";

            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift) || UnityEngine.Input.GetKeyDown(KeyCode.RightShift)) buf = buf.ToUpper();
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.Backspace) && text.Length > 0) buf = text.Substring(0,text.Length - 1);
            else buf = text + buf;

            if(System.Text.RegularExpressions.Regex.IsMatch(buf, preg)) return buf;
            else return text;
        }


        /// <summary>
        /// Sprawdza czy kliknięto lub dotkięto ekranu. Zdarzenie jest wykrywane jednorazowo.
        /// </summary>
        /// <returns>bool</returns>
        static public bool IsClicked()
        {
            if (UnityEngine.Input.GetMouseButtonUp(0))
                return true;
            else if (UnityEngine.Input.touchCount > 0)
            {
                if (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Ended)
                    return true;
            }
            return false;
        }
        
        /// <summary>
        /// Sprawdza czy klawisz myszy jest wciśnięty lub ekran jest dotykany. Zwraca wartość przez cały okres zdarzenia. 
        /// </summary>
        /// <returns>bool</returns>
        static public bool IsDown()
        {
            if (UnityEngine.Input.GetMouseButton(0))
                return true;
            else if (UnityEngine.Input.touchCount > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Zwraca pozycję na mapie gdzie kliknięto lub dotknięto ekranu.
        /// W przeciwnym wypadku null.
        /// </summary>
        /// <returns>Wektor pozycji kliknięcia</returns>
        static public Vector3? InputClickPosition()
        {
            bool touchedScreen = false;
            Vector3 touchPosition3D = new Vector3();
            
            // mysz
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                touchPosition3D = UnityEngine.Input.mousePosition;
                touchedScreen = true;
            }
            
            // palec
            for(int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                if (UnityEngine.Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    touchPosition3D = UnityEngine.Input.GetTouch(i).position;
                    touchedScreen = true;
                    break;
                }
            }

            // wykrycie pozycji
            if (touchedScreen == true)
            {
                return Camera.main.ScreenToWorldPoint(touchPosition3D);
            }
            return null;
        }
    }
}