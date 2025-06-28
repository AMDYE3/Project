using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class LauncherWindow : EditorWindow
{
    private const string m_LevelKey = "ACTIVE_LEVEL";
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

        if (!EditorPrefs.HasKey(m_LevelKey))
        {
            EditorPrefs.SetString(m_LevelKey, GlobalConst.DEFAULT_LEVEL);
        }
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
        
        var activeLevel = EditorPrefs.GetString(m_LevelKey);  
        activeLevel = GUILayout.TextField(activeLevel);
        foreach (string level in m_AvailableLevels)
        {
            if (GUILayout.Button($"Level - {level}"))
            {
                activeLevel = level;
            }
        }
        EditorPrefs.SetString(m_LevelKey, activeLevel);
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
        }

        else if (state == PlayModeStateChange.ExitingPlayMode)
        {
            Debug.Log("Exiting Playmode");
        }
    }

    private static void LaunchLevel()
    {
        var activeLevel = EditorPrefs.GetString(m_LevelKey);
        Debug.Log($"Entered Play Mode, Level {activeLevel}");
        WorldManager.Instance.Load(activeLevel);
    }
}