using UnityEngine;

public class BossPhysicsController : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField] private Collider2D statsCollider;
    [SerializeField] private Collider2D playerDetectionCollider;
    [SerializeField] private Collider2D damagePlayerCollider;

    public bool playerInAttackRange;

    public void EnableStatsCollider()
    {
        statsCollider.enabled = true;
    }
    public void DisableStatsCollider()
    {
        statsCollider.enabled = false;
    }

    public void EnablePlayerDetectionCollider()
    {
        playerDetectionCollider.enabled = true;
    }
    public void DisablePlayerDetectionCollider()
    {
        playerDetectionCollider.enabled = false;
    }

    public void EnableDamagePlayerCollider()
    {
        damagePlayerCollider.enabled = true;
    }
    public void DisableDamagePlayerCollider()
    {
        damagePlayerCollider.enabled = false;
    }

    public void DisableAllColliders()
    {
        DisableStatsCollider();
        DisableDamagePlayerCollider();
        DisablePlayerDetectionCollider();
    }
}
