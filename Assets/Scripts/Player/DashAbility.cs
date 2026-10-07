using UnityEngine;

public class DashAbility : BaseAbility
{
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashMaxTime;
    private float dashMaxTimer;

    private string dashAnimParameterString = "Dash";
    private int dashAnimParameterId;

    protected override void Initialize()
    {
        base.Initialize();
        linkedGatherInput.SetDashInstructions(TryToDash);
        dashAnimParameterId = Animator.StringToHash(dashAnimParameterString);
    }

    private void OnDisable()
    {
        linkedGatherInput.RemoveDashInstructions(TryToDash);
    }
    public override void EnterAbility()
    {
        player.playerStats.DisableTakeDamage();
        //or
        //player.playerStats.DisableStatsCollider();
    }
    private void TryToDash()
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;

        if (linkedStateMachine.currentState == PlayerStates.State.Dash || linkedPhysicsControl.wallDetected || linkedStateMachine.currentState == PlayerStates.State.Reload)
            return;

        linkedStateMachine.ChangeState(PlayerStates.State.Dash);

        linkedPhysicsControl.DisableGravity();
        linkedPhysicsControl.ResetVelocity();
        dashMaxTimer = dashMaxTime;
        if (player.facingRight)
            linkedPhysicsControl.rb.linearVelocityX = dashSpeed;
        else
            linkedPhysicsControl.rb.linearVelocityX = -dashSpeed;

    }

    public override void ProcessAbility()
    {
        dashMaxTimer -= Time.deltaTime;
        if (linkedPhysicsControl.wallDetected)
            dashMaxTimer = -1;
        if (dashMaxTimer <= 0)
        {
            if (linkedPhysicsControl.grounded)
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            else
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }

    }

    public override void ExitAbility()
    {
        linkedPhysicsControl.EnableGravity();
        linkedPhysicsControl.ResetVelocity();
        player.playerStats.EnableTakeDamage();
        //or
        //player.playerStats.EnableStatsCollider();
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(dashAnimParameterId, thisAbilityState == linkedStateMachine.currentState);
    }

}
