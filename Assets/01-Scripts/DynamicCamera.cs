using System;
using UnityEngine;

namespace _01_Scripts
{
    public class DynamicCamera : MonoBehaviour
    {
        [SerializeField] private Camera camera;

        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

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
            camera.orthographicSize =
                _gameManager.parameters.cameraSizeBase +
                (_gameManager.parameters.cameraSizeOffset * (_gameManager.currentLevel.columns - 1));
        }
    }
}