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
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private TileType tileType;
    [SerializeField] private List<Sprite> icons;
    [SerializeField] private UnityEvent onPop;

    private GameManager _gameManager;
    private int _groupIndex;
    private bool _isClickable;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _isClickable = true;
    }

    private void OnEnable()
    {
        EventManager.PopTilesEvent += PopTile;
        EventManager.SetClickableStateEvent += SetClickableState;
    }

    private void OnDisable()
    {
        EventManager.PopTilesEvent -= PopTile;
        EventManager.SetClickableStateEvent -= SetClickableState;
    }

    public void Enable()
    {
        Debug.Log($"GamePiece Enable - {gameObject.name}");
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        transform.localScale = Vector3.one;
        SetIcon(0);
    }

    public void Disable()
    {
        Debug.Log($"GamePiece Disable - {gameObject.name}");
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        _groupIndex = -1;
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
        if (!_isClickable) return;
        
        Debug.Log("Clicked Tile");
        var canPop = EventManager.GetGroupSize(_groupIndex) >= _gameManager.parameters.minGroupSize;
        if (canPop)
        {
            EventManager.SetClickableState(false);
            DOVirtual.DelayedCall(_gameManager.parameters.gamePieceMoveDelay, () => EventManager.PopTiles(_groupIndex));
        }
        else
        {
            transform.DOShakeRotation(_gameManager.parameters.failShakeDuration, new Vector3(0, 0, 25), 20, 45, true,
                ShakeRandomnessMode.Harmonic);
        }
    }

    public void MoveToPosition(Vector3 targetPosition, float delay)
    {
        var distance = (targetPosition - transform.position).magnitude;
        if (distance == 0) return;
        var duration = distance / _gameManager.parameters.tileMoveSpeed;
        Debug.Log($"MovingTile - Distance: {distance} - Duration: {duration}");
        transform.DOMove(targetPosition, duration).SetEase(Ease.OutExpo).SetDelay(delay);
    }

    public void PlayVFX()
    {
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);
    }

    private void PopTile(int groupIndex)
    {
        if (_groupIndex != groupIndex) return;
        Debug.Log($"Popping Tile: {gameObject.name}");
        //Play VFX
        onPop?.Invoke();
    }

    private void SetClickableState(bool clickableState)
    {
        _isClickable = clickableState;
    }
}