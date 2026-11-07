using UnityEngine;

[System.Serializable]
public struct GameplayData
{
  public Vector3 position;
  public string sceneName;
  public int currentHealth;
  public float currentStamina;
  public int currentLevel;
  public int currentXP;
  public int xpToNextLevel;
  
  public string saveTimestamp;
  
} 