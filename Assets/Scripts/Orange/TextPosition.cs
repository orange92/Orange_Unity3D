using UnityEngine;
using System.Collections;

public class TextPosition : MonoBehaviour {

    public int sortingOrder;

    void Start()
    {
        gameObject.renderer.sortingOrder = sortingOrder;
    }
}
