using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class LauncherWindow : EditorWindow
{
    private static string m_Level = GlobalConst.DEFAULT_LEVEL;
    
    [MenuItem("Play/Level Launcher")]
    public static void ShowWindow()
    {
        GetWindow<LauncherWindow>("Level Launcher");
    }

    private void OnEnable()
    {
        Debug.Log("Launcher Enable");
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private void OnDisable()
    {
        Debug.Log("Launcher Disable");
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
    }

    private void OnGUI()
    {
        GUILayout.Label(EditorApplication.isPlaying ? $"Playing: {WorldManager.Instance.GetCurrentLevel()}" : "Editing", EditorStyles.boldLabel);
        
        m_Level = GUILayout.TextField(m_Level);
        if (GUILayout.Button("Launch"))
        {
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.EnterPlaymode();
            }
            else
            {
                LaunchLevel();
            }
        }

        if (GUILayout.Button("Stop"))
        {
            EditorApplication.ExitPlaymode();
        }
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            EditorSceneManager.OpenScene(GlobalConst.MAIN_SCENE, OpenSceneMode.Single);
        }
        
        else if (state == PlayModeStateChange.EnteredPlayMode)
        {
            LaunchLevel();
            Debug.Log($"Entered Play Mode, Level {m_Level}");
        }

        else if (state == PlayModeStateChange.ExitingPlayMode)
        {
            Debug.Log("Exiting Playmode");
        }
    }

    private static void LaunchLevel()
    {
        WorldManager.Instance.Load(m_Level);
    }
}