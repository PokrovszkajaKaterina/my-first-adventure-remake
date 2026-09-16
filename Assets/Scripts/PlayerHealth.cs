using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int CurrentHealth { get; private set; }
    
    [Header("UI")]
    public Slider healthBar;

    void Start() 
    {
        CurrentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = CurrentHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        healthBar.value = CurrentHealth;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle player death (e.g., play animation, disable controls, etc.)
        Debug.Log("Player has died.");
    }

}
