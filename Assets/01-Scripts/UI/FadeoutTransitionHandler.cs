using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FadeoutTransitionHandler : MonoBehaviour, ITransitionHandler
{
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Color visibleColor;
    [SerializeField] private Color invisibleColor;
    
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    public void FadeOut(Action callback)
    {
        renderer.DOColor(invisibleColor, _gameManager.parameters.fadeDuration).OnComplete(callback.Invoke);
    }

    public void FadeIn(Action callback)
    {
        renderer.DOColor(visibleColor, _gameManager.parameters.fadeDuration).OnComplete(callback.Invoke);
    }
}
