using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    public void OnClick()
    {
        _gameManager.SetNextLevel(Int32.Parse(inputField.text));
        _gameManager.ChangeScene(SceneType.GameScene2D);
    }
}
