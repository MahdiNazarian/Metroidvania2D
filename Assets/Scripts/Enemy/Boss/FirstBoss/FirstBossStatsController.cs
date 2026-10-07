using UnityEngine;

public class FirstBossStatsController : EnemyStats
{
    [SerializeField] private HealthBarControl bossHealthBar;
    private FirstBossStateMachine stateMachine;
    protected override void Initialize()
    {
        base.Initialize();
        bossHealthBar.SetSliderValue(health, maxHealth);
        stateMachine = GetComponentInParent<FirstBossStateMachine>();
    }
    protected override void DamageProcess()
    {
        bossHealthBar.SetSliderValue(health, maxHealth);
    }
    protected override void DeathProcess()
    {
        stateMachine.ChangeState(BossStateMachine.BossStates.Death);
    }
}
