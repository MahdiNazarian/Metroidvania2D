using UnityEngine;

public class PatrollingEnemyDetectPlayer : MonoBehaviour
{
    private PatrollingPhysicsController physicsController;
    void Start()
    {
        physicsController = GetComponentInParent<PatrollingPhysicsController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        physicsController.playerInAttackRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        physicsController.playerInAttackRange = false;
    }
}
