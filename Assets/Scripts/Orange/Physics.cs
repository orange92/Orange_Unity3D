using UnityEngine;

namespace Orange
{
    public static class Physics
    { 
        /// <summary>
        /// Oblicza siłę i wektor jaki zadziałał na uderzony obiekt. 
        /// </summary>
        /// <param name="otherCollision">Parametr Collision ze zdarzenia</param>
        /// <param name="otherMass">Masa obiektu ktury uderzył</param>
        /// <returns>Wektor z siłą uderzenia.</returns>
        public static Vector3 CalcCollisionForce(Collision otherCollision, float otherMass = 1000)
        {
            Vector3 force = otherCollision.relativeVelocity * otherMass;
            return force;
        }
    }
}
