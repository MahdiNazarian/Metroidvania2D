using UnityEngine;

public class FallTrapController : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        playerStats.DamagePlayer(damage);
    }
}
