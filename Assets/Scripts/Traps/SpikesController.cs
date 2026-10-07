using UnityEngine;

public class SpikesController : MonoBehaviour
{
    [SerializeField] private float spikeDamage;
    [SerializeField] private Collider2D spikeCollider;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;

    public void EnableCollider()
    {
        spikeCollider.enabled = true;
    }
    public void DisableCollider()
    {
        spikeCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockBackAblility knockBackAblility = collision.GetComponentInParent<KnockBackAblility>();
        knockBackAblility.StartKnockBack(knockBackDuration, knockBackForce, transform);

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        playerStats.DamagePlayer(spikeDamage);
    }
}
