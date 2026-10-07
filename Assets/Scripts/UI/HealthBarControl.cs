using UnityEngine;
using UnityEngine.UI;

public class HealthBarControl : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient healthGradient;
    private Image fillImage;
    private void Awake()
    {
        fillImage = healthSlider.fillRect.GetComponent<Image>();
    }
    public void SetSliderValue(float currentHealth, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        fillImage.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }
}
