using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGamePiece
{
    public void SetIcon(int iconIndex);
    public TileType GetTileType();
    void SetGroupIndex(int groupIndex);
    public void MoveToPosition(Vector3 targetPosition, float delay);
    public void StopMovement();
}
