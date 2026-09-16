using UnityEngine;

[System.Serializable]
public struct PlayerSaveData
{
  public Vector3 position;
  public string sceneName;
  public int currentHealth;
  public float currentStamina;
  public int currentLevel;
  public int currentXP;
  public int xpToNextLevel;
  
} 