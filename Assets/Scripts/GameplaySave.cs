using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplaySave : MonoBehaviour
{

  static string SaveFolder => Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Saves"));

  public static string CurrentSavePath;

  public KeyCode saveKey = KeyCode.F5;

  public static bool HasSaveData()
  {
    return ListSaveFiles().Any();
  }

  public static IEnumerable<string> ListSaveFiles()
  {
    if (!Directory.Exists(SaveFolder))
      return Enumerable.Empty<string>();

    return Directory.GetFiles(SaveFolder, "*.json")
                    .OrderByDescending(path => File.GetLastWriteTime(path));
  }

  public static GameplayData Read(string path)
  {
    return JsonUtility.FromJson<GameplayData>(File.ReadAllText(path));
  }

  public static string CreateNewSavePath()
  {
    Directory.CreateDirectory(SaveFolder);
    return Path.Combine(SaveFolder, $"save_{DateTime.Now:yyyyMMdd_HHmmss_fff}.json");
  }

  void Start()
  {
    bool isNewGame = CurrentSavePath == null;
    if (isNewGame)
      Save();
    else
      Load();
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
    CurrentSavePath = CreateNewSavePath();
    var data = new GameplayData
    {
      position = transform.position,
      sceneName = SceneManager.GetActiveScene().name,
      currentHealth = GetComponent<PlayerHealth>().CurrentHealth,
      currentStamina = GetComponent<PlayerStamina>().CurrentStamina,
      currentLevel = GetComponent<PlayerExperience>().currentLevel,
      currentXP = GetComponent<PlayerExperience>().currentXP,
      xpToNextLevel = GetComponent<PlayerExperience>().xpToNextLevel,
      saveTimestamp = DateTime.Now.ToString("o")
    };

    File.WriteAllText(CurrentSavePath, JsonUtility.ToJson(data, true));

    if (SaveIndicator.Instance != null)
      SaveIndicator.Instance.Show();
  }

  void Load()
  {
    GameplayData data = Read(CurrentSavePath);
    RestorePosition(data.position);
    GetComponent<PlayerHealth>().RestoreHealth(data.currentHealth);
    GetComponent<PlayerStamina>().RestoreStamina(data.currentStamina);
    GetComponent<PlayerExperience>().RestoreExperience(data.currentLevel, data.currentXP, data.xpToNextLevel);
  }

  void RestorePosition(Vector3 position)
  {
    GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    transform.position = position;
  }
}