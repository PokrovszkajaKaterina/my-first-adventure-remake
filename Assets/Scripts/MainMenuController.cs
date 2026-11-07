using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private VisualTreeAsset saveEntryTemplate;

    private VisualElement mainPanel;
    private VisualElement loadPanel;
    private ScrollView saveList;
    private Button loadGameButton;

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;
        mainPanel = root.Q<VisualElement>("main-panel");
        loadPanel = root.Q<VisualElement>("load-panel");
        saveList = root.Q<ScrollView>("save-list");
        loadGameButton = root.Q<Button>("load-game");

        root.Q<Button>("new-game").clicked += OnNewGameClicked;
        loadGameButton.clicked += ShowLoadPanel;
        root.Q<Button>("exit").clicked += OnExitClicked;
        root.Q<Button>("back").clicked += ShowMainPanel;

        ShowMainPanel();
    }

    private void ShowMainPanel()
    {
        loadPanel.style.display = DisplayStyle.None;
        mainPanel.style.display = DisplayStyle.Flex;
        loadGameButton.style.display = GameplaySave.HasSaveData() ? DisplayStyle.Flex : DisplayStyle.None;
    }
    
    private void ShowLoadPanel()
    {
        PopulateSaveList();
        mainPanel.style.display = DisplayStyle.None;
        loadPanel.style.display = DisplayStyle.Flex;
    }

    private void PopulateSaveList()
    {
        saveList.Clear();
        foreach (string path in GameplaySave.ListSaveFiles())
            AddSaveEntry(path);
    }

    private void AddSaveEntry(string path)
    {
        try
        {
            saveList.Add(CreateSaveEntry(path, GameplaySave.Read(path)));
        }
        catch (ArgumentException)
        {
            Debug.LogWarning($"Skipping unreadable save file: {path}");
        }
    }

    private VisualElement CreateSaveEntry(string path, GameplayData data)
    {
        TemplateContainer entry = saveEntryTemplate.Instantiate();
        entry.Q<Label>("saved-location").text = data.sceneName;
        entry.Q<Label>("saved-lvl").text = $"level {data.currentLevel}";
        entry.Q<Label>("saved-timestamp").text = FormatSaveTime(data.saveTimestamp);
        entry.Q<Button>("save-entry").clicked += () => LoadSave(path, data.sceneName);
        return entry;
    }

    private static string FormatSaveTime(string saveTimestamp)
    {
        return DateTime.Parse(saveTimestamp).ToString("yyyy.MM.dd HH:mm");
    }


    private void OnNewGameClicked()
    {
        GameplaySave.CurrentSavePath = null;
        SceneManager.LoadScene("GardenScene");
    }

    private void LoadSave(string path, string sceneName)
    {
        GameplaySave.CurrentSavePath = path;
        SceneManager.LoadScene(sceneName);
    }

    private void OnExitClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
