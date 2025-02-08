using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour {
    //https://www.youtube.com/watch?v=0tDPxNB2JNs
    [SerializeField] 
    private Image healthFill;

    [SerializeField]
    private TextMeshProUGUI textDisplay;

    private float maxHealth;
    private float currentHealth;

    public void DisplaySpecified(float max, float current) { 
        maxHealth = max;
        currentHealth = current;
        UpdateDisplay();
    }

    public void Damage(float dmg) {
        currentHealth -= dmg;
        UpdateDisplay();
    }

    public void Heal(float hp) {
        currentHealth += hp;
        UpdateDisplay();
    }

    private void UpdateDisplay() {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthFill.fillAmount = currentHealth / maxHealth;
        textDisplay.text = $"{currentHealth}/{maxHealth}";
    }
}
