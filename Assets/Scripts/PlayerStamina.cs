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

    public float CurrentStamina { get; private set; }
    private bool isExhausted = false;
    private float regenTimer = 0f;

    void Start() 
    {
        CurrentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = CurrentStamina;
    }

    void Update()
    {
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);

        if (shiftHeld && CanRun())
        {
            CurrentStamina -= drainRate * Time.deltaTime;
            regenTimer = 0f;

            if (CurrentStamina <= 0)
            {
                CurrentStamina = 0;
                isExhausted = true;
            }
        }
        else if (!shiftHeld)
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenDelay)
            {
                CurrentStamina += regenRate * Time.deltaTime;
                CurrentStamina = Mathf.Min(CurrentStamina, maxStamina);

                if (isExhausted && CurrentStamina >= runThreshold)
                {
                    isExhausted = false;
                }
            }
        }

        staminaBar.value = Mathf.RoundToInt(CurrentStamina);
    }


    public bool CanRun()
    {
        return !isExhausted;
    }


}
