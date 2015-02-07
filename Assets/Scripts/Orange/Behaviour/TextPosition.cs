using UnityEngine;
using System.Collections;

namespace Orange.Behaviour
{
    public class TextPosition : MonoBehaviour
    {
        public int sortingOrder;

        void Start()
        {
            gameObject.renderer.sortingOrder = sortingOrder;
        }
    }
}