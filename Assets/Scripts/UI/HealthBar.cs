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

    private float maxHealth;
    private float currentHealth;

    public void DisplaySpecified(float max, float current) { 
        maxHealth = max;
        currentHealth = current;
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
            Debug.Log($"{healthFill.fillAmount}, {time}, {start}, {end}");
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
