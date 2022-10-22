using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    public void OnClick()
    {
        _gameManager.ChangeScene(SceneType.MenuScene);
    }
}