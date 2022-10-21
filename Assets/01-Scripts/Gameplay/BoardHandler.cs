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
    private Dictionary<int, List<GameObject>> _groups;
    private List<List<bool>> _isGrouped;
    private ObjectPool[] _pools;
    private Spawner[] _spawners;
    private int _groupIndex;
    private TileType _tileType;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _groupIndex = 0;
        Initialize();
    }

    private void OnEnable()
    {
        EventManager.GetBoardPositionEvent += GetBoardPosition;
        EventManager.GetGroupSizeEvent += GetGroupSize;
        EventManager.PopTilesEvent += ReleaseGamePieces;
    }

    private void OnDisable()
    {
        EventManager.GetBoardPositionEvent -= GetBoardPosition;
        EventManager.GetGroupSizeEvent -= GetGroupSize;
        EventManager.PopTilesEvent += ReleaseGamePieces;
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
                    groupIndex = -1,
                    tile = null
                });
            }
        }

        _groups = new Dictionary<int, List<GameObject>>();
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

            var gamePieceIndex = i % _levelData.colors;
            for (int j = 0; j < _levelData.rows; j++)
            {
                var obj = Instantiate(_gameManager.parameters.gamePieces[gamePieceIndex], _board[i][j].position,
                    Quaternion.identity, transform);
                _board[i][j].tile = obj;
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
                if (_board[i][j].groupIndex != -1) continue;
                var newGroup = new List<GameObject>();
                _groups.Add(_groupIndex, newGroup);
                _tileType = _board[i][j].tile.GetComponent<IGamePiece>().GetTileType();
                RecursivePartition(i, j);
                Debug.Log($"Setting Tile Data: {_groupIndex}");
                SetTileData(newGroup);
                _groupIndex++;
            }
        }
    }

    private void RecursivePartition(int column, int row)
    {
        if (column < 0 || column >= _levelData.columns || row < 0 || row >= _levelData.rows) return; // Out of bounds
        
        var cellData = _board[column][row];
        if (cellData.groupIndex == _groupIndex) return; // Visited cell

        var tileType = cellData.tile.GetComponent<IGamePiece>().GetTileType();
        if (tileType != _tileType) return; // Different type

        var oldGroupIndex = cellData.groupIndex;
        cellData.groupIndex = _groupIndex;
        _groups[_groupIndex].Add(cellData.tile);
        
        if (oldGroupIndex != -1)
        {
            _groups[oldGroupIndex].Remove(cellData.tile);
            if (_groups[oldGroupIndex].Count == 0)
                _groups.Remove(oldGroupIndex);
        }
        
        //Check adjacent tiles
        RecursivePartition(column + 1, row);
        RecursivePartition(column - 1, row);
        RecursivePartition(column, row + 1);
        RecursivePartition(column, row - 1);
    }

    private void SetTileData(List<GameObject> group)
    {
        var iconIndex = 0;
        var groupSize = group.Count;
        for (int i = _levelData.iconLimits.Count - 1; i >= 0; i--)
        {
            if (groupSize < _levelData.iconLimits[i]) continue;
            iconIndex = i;
            break;
        }

        foreach (var obj in group)
        {
            var gamePiece = obj.GetComponent<IGamePiece>();
            gamePiece.SetIcon(iconIndex);
            gamePiece.SetGroupIndex(_groupIndex);
        }
    }

    private Vector3 GetBoardPosition(int column, int row)
    {
        return _board[column][row].position;
    }

    private int GetGroupSize(int groupIndex)
    {
        Debug.Log($"GetGroupSize: {groupIndex} - {_groups[groupIndex].Count}");
        return _groups[groupIndex].Count;
    }

    private void ReleaseGamePieces(int groupIndex)
    {
        foreach (var obj in _groups[groupIndex])
        {
            var poolIndex = (int) obj.GetComponent<IGamePiece>().GetTileType();
            _pools[poolIndex].Release(obj);
        }
        _groups.Remove(groupIndex);
    }
}