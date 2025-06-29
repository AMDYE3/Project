using System;
using System.Collections.Generic;
using System.IO;
using EventSystem;
using Objects.Interactables;
using OfficeOpenXml;
using UnityEngine;
using UnityEngine.UI;
using Utils;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }

    public ObjectBinding Binding;
    
    public Vector2Int WorldSize {get; private set;}
    public string CurrentLevel {get; private set;}
    public string NextLevel {get; private set;}

    private Camera m_Camera;
    private GameObject m_Root;
    private GameObject m_Background;
    
    private Dictionary<Vector2Int, GameObject> m_World;
    private Dictionary<int, Vector2Int> m_Index;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("WorldManager Initialized");
    }
    
    public void Load(string level)
    {
        m_Camera = Camera.main;
        if (m_Root != null)
        {
            Destroy(m_Root);
        }
        m_Root = new GameObject("Root");
        m_World = new Dictionary<Vector2Int, GameObject>();
        m_Index = new Dictionary<int, Vector2Int>();
        CurrentLevel = level;
        
        var filePath = Path.Combine(GlobalConst.LEVEL_FOLDER, level + GlobalConst.LEVEL_EXTENSION);
        filePath = Path.Combine(Application.streamingAssetsPath, filePath);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(filePath);
        }

        ExcelPackage.License.SetNonCommercialOrganization(GlobalConst.GROUP_NAME);

        using var package = new ExcelPackage(new FileInfo(filePath));

        var propSheet = package.Workbook.Worksheets[GlobalConst.PROP_SHEET];

        int width = Convert.ToInt32(propSheet.Cells["B1"].Value);
        int height = Convert.ToInt32(propSheet.Cells["B2"].Value);
        
        WorldSize = new Vector2Int(width, height);
        
        NextLevel = propSheet.Cells["B3"].Text;
        
        string background = propSheet.Cells["B4"].Text;
        SetBackground(background);
        
        Debug.Log($"Load World Size: {width}x{height}");
        
        float aspect = m_Camera.aspect;
        float size = m_Camera.orthographicSize * 2;
        m_Root.transform.position = new Vector3(-size * aspect / 2, -size / 2, 0.0f);
        m_Root.transform.localScale = new Vector3(size * aspect / width, size / height, 1.0f);
        
        var levelSheet = package.Workbook.Worksheets[GlobalConst.MAP_SHEET];

        for (int x = 1; x <= width; x++)
        {
            for (int y = 1; y <= height; y++)
            {
                int i = x - 1;
                int j = height - y;
                
                var val = levelSheet.Cells[y, x].Value;
                if (val != null)
                {
                    int index = Convert.ToInt32(val);
                    CreateObject(index, new Vector2Int(i, j));
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            CreateObject(0, new Vector2Int(x, -1));
            CreateObject(0, new Vector2Int(x, height));
        }
        
        for (int y = 0; y < height; y++)
        {
            CreateObject(0, new Vector2Int(-1, y));
            CreateObject(0, new Vector2Int(width, y));
        }
        
        EventSystem.EventCenter.AddListener(EventSystem.EventType.ReachEnd, GoToNextLevel);
        
        Debug.LogWarning($"Load Level {CurrentLevel} successfully");
    
    }

    public void ResetCurrentLevel()
    {
        if (string.IsNullOrEmpty(CurrentLevel))
        {
            Debug.LogError("Current level is empty");
            return;
        }
        Load(CurrentLevel);
    }

    public void GoToNextLevel()
    {
        if (NextLevel == null)
            Debug.LogError("Next level is null");
        
        if (string.IsNullOrEmpty(NextLevel))
        {
            Debug.LogWarning($"{CurrentLevel}: Next level is empty");
            LevelSelector.Instance.ShowMainMenu();
            return;
        }
        
        EventCenter.Clear();
        
        Load(NextLevel);
    }

    public Vector2Int GetIndex(Vector3 worldPosition)
    {
        var localPosition = m_Root.transform.InverseTransformPoint(worldPosition);
        return new Vector2Int(Mathf.FloorToInt(localPosition.x), Mathf.FloorToInt(localPosition.y));
    }

    public Vector2Int GetIndex(GameObject obj)
    {
        return GetIndex(obj.transform.position);
    }

    public GameObject GetObject(Vector2Int idx)
    {
        return m_World.GetValueOrDefault(idx, null);
    }

    public void DestroyObject(Vector2Int idx)
    {
        if (m_World.ContainsKey(idx))
        {
            Destroy(m_World[idx]);
            m_World.Remove(idx);
        }
    }

    public GameObject CreateObject(int type, Vector2Int idx)
    {
        
        var prefab = Binding.Value[type];
        if (prefab == null)
        {
            throw new NullReferenceException($"type {type} binding is null");
        }
                        
        var go = Instantiate(prefab, m_Root.transform);
        go.name = $"{prefab.name} {type}";
        go.transform.localPosition = new Vector3(idx.x + 0.5f, idx.y + 0.5f, 0.0f);
        UpdateDepth(go);
        m_World[idx] = go;
        m_Index[go.GetInstanceID()] = idx;
        
        go.GetComponent<Interactable>()?.SetMoveCallback((_, _) =>
        {
            UpdatePosition(go);
        });
        
        return go;
    }

    public void UpdateDepth(GameObject go)
    {
        var pos = go.transform.localPosition;
        pos.z = -pos.x + pos.y;
        go.transform.localPosition = pos;
    }

    public void UpdatePosition(GameObject go)
    {
        UpdateDepth(go);
        var idx = GetIndex(go);
        var originalIdx = m_Index[go.GetInstanceID()];

        if (idx != originalIdx)
        {
            m_World.Remove(m_Index[go.GetInstanceID()]);
            m_Index[go.GetInstanceID()] = idx;
            m_World[idx] = go;
            Debug.Log($"Move {go.name} [{originalIdx.x},{originalIdx.y}] -> [{idx.x},{idx.y}]");
        }
    }

    private void SetBackground(string background)
    {
        if (m_Background != null)
        {
            Destroy(m_Background);
        }
        
        if (string.IsNullOrEmpty(background))
        {
            Debug.LogError("Background is empty");
            return;
        }
        
        var path = Path.Combine(GlobalConst.BACKGROUND_FOLDER, background);
        path = Path.Combine(Application.streamingAssetsPath, path);
        if (File.Exists(path))
        {
            m_Background = new GameObject("Background");
            m_Background.transform.position = new Vector3(0f, 0f, GlobalConst.MAX_DEPTH);
            
            m_Background.AddComponent<Canvas>();
            var image = m_Background.AddComponent<RawImage>();
            var rect = m_Background.GetComponent<RectTransform>();
            
            byte[] imageBytes = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            image.texture = tex;
            
            rect.sizeDelta = new Vector2(m_Camera.orthographicSize * m_Camera.aspect * 2, m_Camera.orthographicSize * 2);
        }
        else
        {
            Debug.LogError("File not found at: " + path);
        }
    }

    private void OnDisable()
    {
        Destroy(m_Root);
    }

}