using UnityEngine;
using UnityEngine.UI;

public class ReloadBarControl : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image reloadFillImage;

    public void ActivateReloadBar()
    {
        canvasGroup.alpha = 1;
        reloadFillImage.fillAmount = 0;
    }
    public void DeactivateReloadBar()
    {
        canvasGroup.alpha = 0;
        reloadFillImage.fillAmount = 0;
    }

    public void UpdateReloadBar(float elapsedTime, float reloadTime)
    {
        reloadFillImage.fillAmount = Mathf.Clamp01(elapsedTime / reloadTime);
    }
}
