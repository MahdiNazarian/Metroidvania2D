using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Vector2 knockbackForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockBackAblility knockBackAblility = collision.GetComponentInParent<KnockBackAblility>();
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if(knockBackAblility != null && playerStats != null)
        {
            knockBackAblility.StartKnockBack(knockbackDuration, knockbackForce, transform.parent);
            playerStats.DamagePlayer(damage);
        }

    }
}
