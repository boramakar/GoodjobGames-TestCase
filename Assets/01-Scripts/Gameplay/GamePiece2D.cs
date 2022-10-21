using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HappyTroll;
using UnityEngine;
using UnityEngine.Events;

public class GamePiece2D : MonoBehaviour, IGamePiece, IPoolObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private TileType tileType;
    [SerializeField] private List<Sprite> icons;
    [SerializeField] private UnityEvent onPop;
    
    private GameManager _gameManager;
    private int _groupIndex;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        EventManager.PopTilesEvent += PopTile;
    }

    private void OnDisable()
    {
        EventManager.PopTilesEvent -= PopTile;
    }

    public void Enable()
    {
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        _groupIndex = -1;
        SetIcon(0);
    }

    public void Disable()
    {
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    public void SetIcon(int iconIndex)
    {
        spriteRenderer.sprite = icons[iconIndex];
    }

    public TileType GetTileType()
    {
        return tileType;
    }

    public void SetGroupIndex(int groupIndex)
    {
        _groupIndex = groupIndex;
    }

    public void OnClick()
    {
        Debug.Log("Clicked Tile");
        var canPop = EventManager.GetGroupSize(_groupIndex) >= _gameManager.parameters.minGroupSize;
        if(canPop)
            EventManager.PopTiles(_groupIndex);
    }

    private void PopTile(int groupIndex)
    {
        if (_groupIndex != groupIndex) return;
        
        //Play VFX
        onPop?.Invoke();
    }

    public void MoveToCell(int column, int row, float delay)
    {
        var targetPosition = EventManager.GetBoardPosition(column, row);
        var distance = (targetPosition - transform.position).magnitude;
        var duration = distance / _gameManager.parameters.tileMoveSpeed;
        transform.DOMove(targetPosition, duration).SetDelay(delay);
    }
}