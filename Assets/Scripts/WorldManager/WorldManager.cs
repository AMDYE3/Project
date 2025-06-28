using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }

    public ObjectBinding Binding;

    private Camera m_Camera;
    private GameObject m_Root;
    private Vector2Int m_WorldSize;
    private Dictionary<Vector2Int, GameObject> m_World;
    private string m_CurrentLevel;
    private string m_NextLevel;

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

    private void Start()
    {
        m_Camera = Camera.main;
    }
    
    public void Load(string level)
    {
        if (m_Root != null)
        {
            Destroy(m_Root);
        }
        m_Root = new GameObject("Root");
        m_World = new Dictionary<Vector2Int, GameObject>();
        m_CurrentLevel = level;
        
        try
        {
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
            m_NextLevel = propSheet.Cells["B3"].Text;
            
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
        }
        catch (Exception e)
        {
            LarkUtil.SendMessage($"Load Level \"{level}\" Error: {e}");
        }
    }

    public void NextLevel()
    {
        if (string.IsNullOrEmpty(m_NextLevel))
        {
            Debug.LogError($"{m_CurrentLevel}: Next level is empty");
            return;
        }
        Load(m_NextLevel);
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

    public GameObject CreateObject(int type, Vector2Int idx)
    {
        if (m_World.ContainsKey(idx))
        {
            Debug.LogError($"Block [{idx.x},{idx.y}] already exists");
            return null;
        }
        
        var prefab = Binding.Value[type];
        if (prefab == null)
        {
            throw new NullReferenceException($"type {type} binding is null");
        }
                        
        var go = Instantiate(prefab, m_Root.transform);
        go.name = $"[{idx.x},{idx.y}] {prefab.name} {type}";
        go.transform.localPosition = new Vector3(idx.x + 0.5f, idx.y + 0.5f, 0.0f);
        m_World[idx] = go;
        return go;
    }

    private void OnDisable()
    {
        Destroy(m_Root);
    }

#if UNITY_EDITOR
    [MenuItem("Debug/Load Debug Level")]
    public static void LoadDebugLevel()
    {
        if (Instance == null)
        {
            Debug.LogError("Application is not running");
            return;
        }
        Instance.Load("Debug");
    }
#endif

}
