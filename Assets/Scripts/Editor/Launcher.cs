using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class LauncherWindow : EditorWindow
{
    private static string m_Level = GlobalConst.DEFAULT_LEVEL;
    private static string[] m_AvailableLevels;
    
    [MenuItem("Play/Level Launcher")]
    public static void ShowWindow()
    {
        GetWindow<LauncherWindow>("Level Launcher");
    }

    private void OnEnable()
    {
        Debug.Log("Launcher Enable");
        EditorApplication.playModeStateChanged += OnPlayModeChanged;

        m_AvailableLevels = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, GlobalConst.LEVEL_FOLDER), "*" + GlobalConst.LEVEL_EXTENSION, SearchOption.TopDirectoryOnly)
            .Select(path => Path.GetFileNameWithoutExtension(path))
            .ToArray();
    }

    private void OnDisable()
    {
        Debug.Log("Launcher Disable");
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
    }

    private void OnGUI()
    {
        GUILayout.Label(EditorApplication.isPlaying ? $"Playing: {WorldManager.Instance.CurrentLevel}" : "Editing", EditorStyles.boldLabel);
        
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
        
        GUILayout.Space(30);
        
        m_Level = GUILayout.TextField(m_Level);
        foreach (string level in m_AvailableLevels)
        {
            if (GUILayout.Button($"Level - {level}"))
            {
                m_Level = level;
            }
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