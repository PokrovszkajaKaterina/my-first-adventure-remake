using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public int maxStamina = 100;
    public float drainRate = 20f;
    public float regenRate = 10f;
    public float regenDelay = 1.5f;
    public int runThreshold = 5;

    [Header("UI")]
    public Slider staminaBar;

    private float currentStamina;
    private bool isExhausted = false;
    private float regenTimer = 0f;

    void Start() 
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = currentStamina;
    }

    void Update()
    {
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);

        if (shiftHeld && CanRun())
        {
            currentStamina -= drainRate * Time.deltaTime;
            regenTimer = 0f;

            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isExhausted = true;
            }
        }
        else if (!shiftHeld)
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenDelay)
            {
                currentStamina += regenRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);

                if (isExhausted && currentStamina >= runThreshold)
                {
                    isExhausted = false;
                }
            }
        }

        staminaBar.value = Mathf.RoundToInt(currentStamina);
    }


    public bool CanRun()
    {
        return !isExhausted;
    }


}
