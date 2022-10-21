using System;
using System.Collections.Generic;
using HappyTroll;
using UnityEngine;

public class BoardHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer borderRenderer;
    [SerializeField] private Transform objectPoolsParent;
    [SerializeField] private Transform spawnersParent;

    private GameManager _gameManager;
    private LevelParameters _levelData;
    private List<List<CellData>> _board;
    private List<List<Coordinate>> _groups;
    private List<List<bool>> _isGrouped;
    private ObjectPool[] _pools;
    private Spawner[] _spawners;
    private int _groupIndex;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _groupIndex = 0;
        Initialize();
    }

    private void Start()
    {
        FillBoard();
        GroupTiles();
        _gameManager.GameStart();
    }

    private void Initialize()
    {
        Debug.Log("Initializing Board");
        _levelData = _gameManager.currentLevel;

        var initialOffsetX = -_gameManager.parameters.tileOffsetX * (_levelData.columns - 1);
        var initialOffsetY = -_gameManager.parameters.tileOffsetY * (_levelData.rows - 1);
        var initialOffsetZ = 0;
        _board = new List<List<CellData>>(_levelData.columns);
        for (int i = 0; i < _levelData.columns; i++)
        {
            _board.Add(new List<CellData>(_levelData.rows));
            for (int j = 0; j < _levelData.rows; j++)
            {
                _board[i].Add(new CellData
                {
                    position = new Vector3(
                        initialOffsetX + (_gameManager.parameters.tileOffsetX * i * 2),
                        initialOffsetY + (_gameManager.parameters.tileOffsetY * j * 2),
                        initialOffsetZ + (_gameManager.parameters.tileOffsetZ * j)),
                    isGrouped = false,
                    tile = null
                });
            }
        }

        _groups = new List<List<Coordinate>>();
        _pools = objectPoolsParent.GetComponentsInChildren<ObjectPool>();
        for (int i = 0; i < _levelData.colors; i++)
        {
            _pools[i].Initialize((_levelData.rows * _levelData.columns) / _levelData.colors);
        }

        _spawners = new Spawner[_levelData.columns];
        for (int i = 0; i < _levelData.columns; i++)
        {
            var obj = new GameObject();
            _spawners[i] = obj.AddComponent<Spawner>();
            obj.transform.parent = spawnersParent;
        }

        borderRenderer.size = new Vector2(
            _gameManager.parameters.borderSizeX +
            (_gameManager.parameters.borderOffsetX * (_levelData.columns - 1)),
            _gameManager.parameters.borderSizeY +
            (_gameManager.parameters.borderOffsetY * (_levelData.rows - 1))
        );
    }

    private void FillBoard()
    {
        Debug.Log("Filling Board");
        for (int i = 0; i < _levelData.columns; i++)
        {
            //_spawners[i].Spawn(_board[i], _levelData.rows);

            var gamePieceIndex = i % _gameManager.parameters.gamePieces.Count;
            for (int j = 0; j < _levelData.rows; j++)
            {
                Instantiate(_gameManager.parameters.gamePieces[gamePieceIndex], _board[i][j].position,
                    Quaternion.identity, transform);
            }
        }
    }

    private void GroupTiles()
    {
        Debug.Log("Grouping Tiles");
        for (int i = 0; i < _levelData.columns; i++)
        {
            for (int j = 0; j < _levelData.rows; j++)
            {
                if (_board[i][j].isGrouped) continue;
                _groups.Add(new List<Coordinate>());
                RecursivePartition(i, j);
                _groupIndex++;
            }
        }
    }

    private void RecursivePartition(int column, int row)
    {
    }
}