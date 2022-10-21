using System;
using System.Collections;
using System.Collections.Generic;
using HappyTroll;
using UnityEngine;

public class GamePiece2D : MonoBehaviour, IGamePiece, IPoolObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> icons;
    
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    public void Enable()
    {
        Debug.Log("Enabling GamePiece");
    }

    public void Disable()
    {
        Debug.Log("Disabling GamePiece");
    }

    public void SetIcon(int iconIndex)
    {
        Debug.Log("Setting Icon");
    }

    public void PopTiles()
    {
        Debug.Log("Popping Tiles");
    }
}
