using UnityEngine;

public class PatrollingEnemyDamagePlayerController : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerStats playerStats))
        {
            playerStats.DamagePlayer(damage);
        }
    }
}
