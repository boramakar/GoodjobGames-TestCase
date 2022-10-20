using System;
using UnityEngine;

namespace _01_Scripts
{
    public class DynamicCamera : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.GameStartEvent += AdjustCamera;
        }

        private void OnDisable()
        {
            EventManager.GameStartEvent -= AdjustCamera;
        }

        private void AdjustCamera()
        {
            Debug.Log("Adjusting Camera");
        }
    }
}