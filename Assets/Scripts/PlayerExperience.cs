using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    [Header("Experience")]
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    [Header("UI")]
    public Slider xpBar;

    void Start()
    {
        xpBar.maxValue = xpToNextLevel;
        xpBar.value = currentXP;
    }

    public void GainXP(int amount)
    {
        currentXP += amount;
        xpBar.value = currentXP;

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
        xpBar.maxValue = xpToNextLevel;
        xpBar.value = currentXP;


        Debug.Log("Leveled up to " + currentLevel + "!");
    }

    void Awake()
    {
        UpdateXPBar();
    }

    public void RestoreExperience(int level, int xp, int xpNeeded)
    {
        currentLevel = level;
        currentXP = xp;
        xpToNextLevel = xpNeeded;
        UpdateXPBar();
    }

    void UpdateXPBar()
    {
        xpBar.maxValue = xpToNextLevel;
        xpBar.value = currentXP;
    }
}