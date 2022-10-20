using UnityEngine;

    public class Spawner : MonoBehaviour
    {
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        public void Spawn(int count = 1)
        {
            Debug.Log($"Spawning {count} tiles");
        }

        public void GetRandomTile()
        {
            Debug.Log("Getting Random Tile");
        }
    }