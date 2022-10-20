using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeoutTransitionHandler : MonoBehaviour, ITransitionHandler
{
    [SerializeField] private Image blackout;
    [SerializeField] private Color visibleColor;
    [SerializeField] private Color invisibleColor;
    
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    public void FadeOut(Action callback)
    {
        var sequence = DOTween.Sequence();
        sequence.Append(blackout.DOColor(invisibleColor, _gameManager.parameters.fadeDuration))
            .AppendCallback(() => blackout.gameObject.SetActive(false))
            .AppendCallback(() => callback?.Invoke());
        sequence.Play();
    }

    public void FadeIn(Action callback)
    {
        blackout.gameObject.SetActive(true);
        blackout.DOColor(visibleColor, _gameManager.parameters.fadeDuration).OnComplete(() => callback?.Invoke());
    }
}
