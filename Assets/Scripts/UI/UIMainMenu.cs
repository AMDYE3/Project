using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public Button StartButton;
    public Button SelectButton;

    private void Start()
    {
        if (StartButton == null || SelectButton == null)
        {
            Debug.LogError("UIMainMenu: StartButton, SelectButton, null");
        }
        StartButton.onClick.AddListener(() =>
        {
            LevelSelector.Instance.StartFirstLevel();
        });
        
        SelectButton.onClick.AddListener(() =>
        {
            LevelSelector.Instance.SelectLevel();
        });
    }
}
