using System;
using UnityEngine;

public class EventManager
{
    public static event Action GameStartEvent;
    public static void GameStart()
    {
        GameStartEvent?.Invoke();
    }

    public static event Action GamePauseEvent;
    public static void GamePause()
    {
        GamePauseEvent?.Invoke();
    }

    public static event Action<int> PopTilesEvent;
    public static void PopTiles(int groupIndex)
    {
        Debug.Log($"PopTilesEvent: {groupIndex}");
        PopTilesEvent?.Invoke(groupIndex);
    }

    public static event Func<int, int, Vector3> GetBoardPositionEvent;
    public static Vector3 GetBoardPosition(int column, int row)
    {
        var position = GetBoardPositionEvent?.Invoke(column, row);
        return position ?? new Vector3(0, 0, -20);
    }

    public static event Func<int, int> GetGroupSizeEvent;
    public static int GetGroupSize(int groupIndex)
    {
        var groupSize = GetGroupSizeEvent?.Invoke(groupIndex);
        return groupSize ?? 0;
    }

    public static event Func<int, GameObject> GetGamePieceEvent;
    public static GameObject GetGamePiece(int tileType)
    {
        var obj = GetGamePieceEvent?.Invoke(tileType);
        return obj;
    }

    public static event Func<GameObject, int, int> AddGamePieceToBoardEvent;
    public static int AddGamePieceToBoard(GameObject gamePiece, int columnIndex)
    {
        var rowIndex = AddGamePieceToBoardEvent?.Invoke(gamePiece, columnIndex);
        return rowIndex ?? -1;
    }
}