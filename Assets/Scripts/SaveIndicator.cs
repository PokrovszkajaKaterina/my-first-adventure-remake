using UnityEngine;
using UnityEngine.UI;

public class SaveIndicator : MonoBehaviour
{
    public static SaveIndicator Instance;

    [SerializeField] Image icon;
    [SerializeField] float totalDuration = 1.2f;
    [SerializeField] float blinkSpeed = 6f;
    [SerializeField] float fadeInDuration = 0.3f;
    [SerializeField] float fadeOutDuration = 0.3f;

    float showStartTime;
    bool blinking;

    void Awake()
    {
        Instance = this;
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0f);
        icon.gameObject.SetActive(false);
    }

    public void Show()
    {
        icon.gameObject.SetActive(true);
        blinking = true;
        showStartTime = Time.time;
    }

    void Update()
    {
        if (!blinking)
            return;

        float elapsed = Time.time - showStartTime;
        float remaining = totalDuration - elapsed;

        float alpha = 1f;
        if (elapsed < fadeInDuration)
            alpha = elapsed / fadeInDuration;
        else if (remaining < fadeOutDuration)
            alpha = remaining / fadeOutDuration;
        alpha = Mathf.Clamp01(alpha);

        float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        Color blinkColor = Color.Lerp(Color.black, Color.red, t);
        icon.color = new Color(blinkColor.r, blinkColor.g, blinkColor.b, alpha);

        if (elapsed >= totalDuration)
        {
            blinking = false;
            icon.gameObject.SetActive(false);
        }
    }
}
