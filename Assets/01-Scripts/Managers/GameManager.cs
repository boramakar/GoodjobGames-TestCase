using System;
using System.Collections;
using System.Collections.Generic;
using HappyTroll;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Required]
    public GameParameters parameters;
    public List<LevelParameters> levelParameters;
    public LevelParameters currentLevel;

    public ITransitionHandler _transitionHandler;
    private string _nextScene;
    
    private void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
        }

        _transitionHandler = GetComponentInChildren<ITransitionHandler>();
    }

    private void Start()
    {
        ChangeScene(SceneType.MenuScene);
    }

    public void ChangeScene(SceneType sceneType)
    {
        _nextScene = sceneType switch
        {
            SceneType.LoadingScene => Strings.LoadingScene,
            SceneType.MenuScene => Strings.MenuScene,
            SceneType.GameScene2D => Strings.GameScene2D,
            SceneType.GameScene3D => Strings.GameScene3D,
            _ => throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null)
        };

        _transitionHandler.FadeIn(LoadScene);
    }

    private void LoadScene()
    {
        SceneManager.LoadSceneAsync(_nextScene);
    }

    public void GameStart()
    {
        EventManager.GameStart();
        _transitionHandler.FadeOut(null);
    }

    public void SetNextLevel(int levelID)
    {
        var validID = levelID % levelParameters.Count;
        currentLevel = levelParameters[validID];
    }
}
