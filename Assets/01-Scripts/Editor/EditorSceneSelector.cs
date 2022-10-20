using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityToolbarExtender;

static class ToolbarStyles
{
    public static readonly GUIStyle commandButtonStyle;
    public static readonly GUIStyle fixedStyle;

    static ToolbarStyles()
    {
        commandButtonStyle = new GUIStyle("Command")
        {
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageAbove,
            fontStyle = FontStyle.Bold,
            stretchWidth = true,
            margin = new RectOffset(5, 5, 0, 0)
        };
        
        fixedStyle = new GUIStyle()
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageAbove,
            fontStyle = FontStyle.Bold,
            stretchWidth = true
        };
    }
}

[InitializeOnLoad]
public class SceneSwitchLeftButton
{
    static SceneSwitchLeftButton()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(new GUIContent("Play", "Start Game"), ToolbarStyles.commandButtonStyle))
        {
            SceneHelper.StartScene(Strings.LoadingScene);
        }

        if (GUILayout.Button(new GUIContent("1", "Menu Scene"), ToolbarStyles.commandButtonStyle))
        {
            SceneHelper.SelectScene(Strings.MenuScene);
        }

        if (GUILayout.Button(new GUIContent("2", "Game Scene"), ToolbarStyles.commandButtonStyle))
        {
            SceneHelper.SelectScene(Strings.GameScene2D);
        }
    }
}

static class SceneHelper
{
    static string sceneToOpen;
    private static EditorApplication.CallbackFunction onUpdateSuccess;

    public static void StartScene(string sceneName)
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }

        sceneToOpen = sceneName;
        EditorApplication.update += OnUpdate;
        onUpdateSuccess += () =>
        {
            EditorApplication.isPlaying = true;
            onUpdateSuccess = null;
        };
    }

    public static void SelectScene(string sceneName)
    {
        if (EditorApplication.isPlaying)
        {
            return;
        }

        sceneToOpen = sceneName;
        EditorApplication.update += OnUpdate;
    }

    static void OnUpdate()
    {
        if (sceneToOpen == null ||
            EditorApplication.isPlaying || EditorApplication.isPaused ||
            EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
        {
            return;
        }

        EditorApplication.update -= OnUpdate;

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            // need to get scene via search because the path to the scene
            // file contains the package version so it'll change over time
            string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
            if (guids.Length == 0)
            {
                Debug.LogWarning("Couldn't find scene file");
            }
            else
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                EditorSceneManager.OpenScene(scenePath);
                onUpdateSuccess?.Invoke();
            }
        }

        sceneToOpen = null;
    }
}