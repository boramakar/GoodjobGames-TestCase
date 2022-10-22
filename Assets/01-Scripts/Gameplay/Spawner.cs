using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    private GameManager _gameManager;
    private Vector3 _initialOffset;
    private int _columnIndex;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _columnIndex = transform.GetSiblingIndex();
        var camera = FindObjectOfType<Camera>();
        _initialOffset = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth / 2f, camera.pixelHeight, -camera.transform.position.z));
        Debug.Log($"InitialOffset");
    }

    public void Spawn(int count = 1)
    {
        Debug.Log($"Spawning {count} tiles at column {_columnIndex}");
        for (int i = 0; i < count; i++)
        {
            var tileType = Random.Range(0, _gameManager.currentLevel.colors);
            var gamePiece = EventManager.GetGamePiece(tileType);
            gamePiece.transform.position = transform.position + _initialOffset +
                                           Vector3.up * (i * 2 * _gameManager.parameters.tileOffsetY);
            var rowIndex = EventManager.AddGamePieceToBoard(gamePiece, _columnIndex);
            if (rowIndex == -1)
            {
                throw new IndexOutOfRangeException($"rowIndex: -1 - Column: {_columnIndex} - Iteration: {i}");
            }
        }
    }
}