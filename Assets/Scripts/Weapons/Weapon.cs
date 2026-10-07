using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string ID;
    public ItemTypes itemType;
    public float damage;
    public float shootCooldown;
    public bool isAutomatic;

    [Header("Ammo")]
    public int currentAmmo;
    public int maxAmmo;
    public int storageAmmo;

    [Header("Realod")]
    public float reloadTime;
    public bool isRealoading = false;

    [Header("Recoil Effect")]
    public float recoilForce;
    public float recoilTime;

    [Header("References")]
    public Transform shootingPoint;
    public Transform shellSpawnPoint;
    public GameObject shellPrefab;
    public GameObject shootingEffectPrefab;
    public GameObject shootHitParticleEffect;
    public Sprite weaponIconSprite;

    [Header("Line Renderer")]
    public float visibleLineTime;
    public float lineWidthMultiplier;

    private WeaponData weaponData;

    public bool ReloadCheck()
    {
        int neededAmmo = maxAmmo - currentAmmo;
        if (neededAmmo <= 0 || storageAmmo <= 0)
            return false;
        return true;
    }

    public void Reload()
    {
        isRealoading = true;
        int neededAmmo = maxAmmo - currentAmmo;
        int reloadAmmo = Mathf.Min(neededAmmo, storageAmmo);
        currentAmmo += reloadAmmo;
        storageAmmo -= reloadAmmo;
        isRealoading = false;
    }

    public void SaveWeaponData()
    {
        if (weaponData == null)
            weaponData = new WeaponData();
        weaponData.ID = ID;
        weaponData.currentAmmo = currentAmmo;
        weaponData.storageAmmo = storageAmmo;
        SaveLoadManager.instance.Save<WeaponData>(weaponData, SaveLoadManager.instance.defaultSaveFolder, ID + ".json");
    }

    public void LoadWeaponData()
    {
        weaponData = SaveLoadManager.instance.Load<WeaponData>(SaveLoadManager.instance.defaultSaveFolder, ID + ".json");
        if(weaponData != null)
        {
            if(!string.IsNullOrEmpty(weaponData.ID))
            {
                currentAmmo = weaponData.currentAmmo;
                storageAmmo = weaponData.storageAmmo;
            }
        }
    }
}
