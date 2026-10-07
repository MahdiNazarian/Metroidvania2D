using UnityEngine;

public class PatrollingEnemyStats : EnemyStats
{
    protected EnemySimpleStateMachine enemyStateMachine;

    protected override void Initialize()
    {
        base.Initialize();
        enemyStateMachine = GetComponentInParent<EnemySimpleStateMachine>();
    }
    protected override void DamageProcess()
    {
        
    }
    protected override void DeathProcess()
    {
        enemyStateMachine.ChangeState(EnemySimpleStateMachine.EnemyStates.Death);
    }
}
