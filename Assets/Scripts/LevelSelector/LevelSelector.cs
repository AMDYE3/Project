using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SimpleFileBrowser; // Make sure this namespace is available

public class LevelSelector : MonoBehaviour
{
    public static LevelSelector Instance { get; private set; }

    public GameObject MainMenu;
    public string MainMenuLevel;
    public string FirstLevel;
    
    private GameObject m_MainMenu;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("LevelSelector Initialized");
    }


    private void Start()
    {
#if !UNITY_EDITOR
        ShowMainMenu();
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMainMenu();
        }
    }

    public void HideMainMenu()
    {
        if (m_MainMenu != null)
        {
            Destroy(m_MainMenu);
        }
    }

    public void ShowMainMenu()
    {
        HideMainMenu();
        m_MainMenu = Instantiate(MainMenu);
        m_MainMenu.name = MainMenuLevel;
        WorldManager.Instance.Load(MainMenuLevel);
    }

    public void StartFirstLevel()
    {
        Debug.Log("LevelSelector Start First Level");
        FileBrowser.HideDialog();
        WorldManager.Instance.Load(FirstLevel);
        HideMainMenu();
    }

    public void SelectLevel()
    {
        Debug.Log("LevelSelector ShowPanel");
        string startPath = Path.Combine(Application.streamingAssetsPath, GlobalConst.LEVEL_FOLDER);
        
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Level Files", GlobalConst.LEVEL_EXTENSION));
        FileBrowser.SetDefaultFilter(".xlsx");
        FileBrowser.ClearQuickLinks();
        
        FileBrowser.ShowLoadDialog(
            OnSelected,
            OnCancel,
            FileBrowser.PickMode.Files,
            false,
            startPath
        );
    }

    private void OnSelected(string[] paths)
    {
        var path = paths[0];
        Debug.Log("Selected Level file: " + path);
        var level = Path.GetFileNameWithoutExtension(path);
        WorldManager.Instance.Load(level);
        HideMainMenu();
    }

    private void OnCancel()
    {
        Debug.Log("Level file selection canceled.");
    }
}