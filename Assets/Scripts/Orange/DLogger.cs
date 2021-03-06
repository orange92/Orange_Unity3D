﻿using UnityEngine;
using System.Collections;

namespace Orange
{

    public class DLogger : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            Debug.Log("DLogger is Active...");
            enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            Debug.LogWarning("DLogger Update -- should not happen -- ");
        }

        void OnApplicationQuit()
        {
            D.Quit();
        }
    }
}