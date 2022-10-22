using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HappyTroll/Data/Game Parameters", fileName = "GameParameters")]
public class GameParameters : ScriptableObject
{
    public float fadeDuration = 0.5f;
    public List<GameObject> gamePieces;
    public float borderSizeX = 2.82f;
    public float borderSizeY = 2.82f;
    public float borderOffsetX = 0.64f;
    public float borderOffsetY = 0.64f;
    public float tileOffsetX = 1.12f;
    public float tileOffsetY = 1.1f;
    public float tileOffsetZ = -0.01f;
    public float cameraSizeBase = 5f;
    public float cameraSizeOffset = 2.5f;
    public float tileMoveSpeed = 3.3f;
    public int minGroupSize = 2;
    public float gamePieceMoveDelay = 0.25f;
    public float gamePieceCascadingMoveDelay = 0.05f;
    public float failShakeDuration = 0.25f;
    public float groupingDelay = 0.8f;
    public float spawnDelay = 0.1f;
}
