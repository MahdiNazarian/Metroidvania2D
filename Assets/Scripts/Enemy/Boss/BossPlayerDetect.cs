using UnityEngine;

public class BossPlayerDetect : MonoBehaviour
{
    private BossPhysicsController bossPhysics;

    private void Start()
    {
        bossPhysics = GetComponentInParent<BossPhysicsController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bossPhysics.playerInAttackRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        bossPhysics.playerInAttackRange = false;
    }
}
