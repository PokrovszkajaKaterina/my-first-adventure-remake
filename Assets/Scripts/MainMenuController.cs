using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    public static bool IsNewGame;

    private void OnEnable()
    {
       var root = uiDocument.rootVisualElement;

        var newGameButton = root.Q<Button>("new-game");
        var exitGameButton = root.Q<Button>("exit");

        newGameButton.clicked += OnNewGameClicked;
        exitGameButton.clicked += OnExitClicked;
    }

    private void OnNewGameClicked()
    {
        IsNewGame = true;
        SceneManager.LoadScene("GardenScene");
    }

    private void OnExitClicked()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
