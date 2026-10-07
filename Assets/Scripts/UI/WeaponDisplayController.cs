using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WeaponDisplayController : MonoBehaviour
{
    [SerializeField] private Image currentWeaponImage;
    [SerializeField] private TextMeshProUGUI weaponAmmoText;
    [SerializeField] private TextMeshProUGUI storageAmmoText;

    public static Action<Sprite, int, int, int> onUpdateAllDisplayInfo;
    public static Action<int, int, int> onUpdateAmmoDisplayInfo;

    private void OnEnable()
    {
        onUpdateAllDisplayInfo += SetAllDisplayInfo;
        onUpdateAmmoDisplayInfo += SetTextDisplayInfo;
    }
    private void OnDisable()
    {
        onUpdateAllDisplayInfo -= SetAllDisplayInfo;
        onUpdateAmmoDisplayInfo -= SetTextDisplayInfo;
    }

    private void SetAllDisplayInfo(Sprite currentImage, int currentAmmo, int maxAmmo, int storageAmmo)
    {
        currentWeaponImage.sprite = currentImage;
        weaponAmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        storageAmmoText.text = storageAmmo.ToString();
    }

    private void SetTextDisplayInfo(int currentAmmo, int maxAmmo, int storageAmmo)
    {
        weaponAmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        storageAmmoText.text = storageAmmo.ToString();
    }
}
