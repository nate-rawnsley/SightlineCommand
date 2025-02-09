using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HealthBar : MonoBehaviour {
    //https://www.youtube.com/watch?v=0tDPxNB2JNs
    [SerializeField] 
    private Image healthFill;

    [SerializeField]
    private Image healthLoss;

    [SerializeField]
    private TextMeshProUGUI textDisplay;

    [SerializeField, Tooltip("What colours the health bar should be.\nFirst is for humans, second for aliens.")]
    private Color[] healthColors = new Color[2];

    private float maxHealth;
    private float currentHealth;

    public void DisplaySpecified(float max, float current, PlayerTeam team) { 
        maxHealth = max;
        currentHealth = current;
        if (team == PlayerTeam.ALIEN) {
            healthFill.color = healthColors[1];
            healthLoss.color = healthColors[1];
        } else {
            healthFill.color = healthColors[0];
            healthLoss.color = healthColors[0];
        }
        UpdateDisplay();
    }

    public void IndicateDamage(float dmg) {
        healthFill.fillAmount = (currentHealth - dmg) / maxHealth;
        healthLoss.fillAmount = dmg / maxHealth;
        healthLoss.transform.localPosition = new Vector2(healthFill.fillAmount * 180, 0);
    }

    public void StopIndicating() {
        healthLoss.fillAmount = 0;
        healthLoss.transform.localPosition = Vector3.zero;
        UpdateDisplay();
    }

    public void Damage(float dmg) {
        StopIndicating();
        StartCoroutine(AnimateChange(currentHealth, currentHealth - dmg));
        currentHealth -= dmg;
    }

    public void Heal(float hp) {
        StopIndicating();
        StartCoroutine(AnimateChange(currentHealth, currentHealth + hp));
        currentHealth += hp;
    }

    private IEnumerator AnimateChange(float start, float end) {
        float time = 0;
        start = start / maxHealth;
        end = end / maxHealth;
        while (time < 1) {
            healthFill.fillAmount = Mathf.Lerp(start, end, time);
            time += Time.deltaTime * 4;
            yield return null;
        }
        UpdateDisplay();
    }

    private void UpdateDisplay() {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthFill.fillAmount = currentHealth / maxHealth;
        textDisplay.text = $"{currentHealth}/{maxHealth}";
    }
}
