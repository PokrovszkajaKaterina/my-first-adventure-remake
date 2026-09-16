using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerSave: MonoBehaviour
{

  static string saveFilePath = "playerSaveData.json";

  public KeyCode saveKey = KeyCode.F5;

  void Start()
  {
    if (MainMenuController.IsNewGame)
    {
        Save();
        MainMenuController.IsNewGame = false;
    }
  }

  void Update()
  {
    if (Input.GetKeyDown(saveKey))
    {
        Save();
    }
  }

  void Save()
  {
    var data = new PlayerSaveData
      {
        position = transform.position,
        sceneName = SceneManager.GetActiveScene().name,
        currentHealth = GetComponent<PlayerHealth>().CurrentHealth,
        currentStamina = GetComponent<PlayerStamina>().CurrentStamina,
        currentLevel = GetComponent<PlayerExperience>().currentLevel,
        currentXP = GetComponent<PlayerExperience>().currentXP,
        xpToNextLevel = GetComponent<PlayerExperience>().xpToNextLevel,
      };
      
      File.WriteAllText(saveFilePath, JsonUtility.ToJson(data));
  }

  void Load()
  {
    if (System.IO.File.Exists(saveFilePath))
    {
      string json = System.IO.File.ReadAllText(saveFilePath);
      var saveData = JsonUtility.FromJson<PlayerSaveData>(json);
      // Apply saveData to the current player state
    }
  }
}