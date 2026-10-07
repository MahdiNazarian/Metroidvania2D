using UnityEngine;

public class PickupAmmoController : MonoBehaviour
{
    [SerializeField] private string weaponID;
    [SerializeField] private int ammo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Shooting shooting))
        {
            shooting.AddAmmoToStorage(weaponID, ammo);
            Destroy(gameObject);
        }
    }
}
