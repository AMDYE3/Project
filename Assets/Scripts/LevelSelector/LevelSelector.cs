using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SimpleFileBrowser; // Make sure this namespace is available

public class LevelSelector : MonoBehaviour
{
#if !UNITY_EDITOR
    private void Start()
    {
        ShowPanel();
    }
#endif

    private void ShowPanel()
    {
        Debug.Log("LevelSelector ShowPanel");
        string startPath = Path.Combine(Application.streamingAssetsPath, GlobalConst.LEVEL_FOLDER);
        
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Level Files", GlobalConst.LEVEL_EXTENSION));
        FileBrowser.SetDefaultFilter(".xlsx");
        
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
    }

    private void OnCancel()
    {
        Debug.Log("Level file selection canceled.");
    }
}