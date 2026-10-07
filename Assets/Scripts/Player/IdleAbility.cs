using UnityEngine;

public class IdleAbility : BaseAbility
{
    private string idleAnimParameterName = "Idle";
    private int idleAnimParameterId;
    protected override void Initialize()
    {
        base.Initialize();
        idleAnimParameterId = Animator.StringToHash(idleAnimParameterName);
    }

    public override void EnterAbility()
    {
        linkedPhysicsControl.rb.linearVelocityX = 0;
    }

    public override void ProcessAbility()
    {
        if (linkedPhysicsControl.grounded && linkedGatherInput.valueX != 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Run);
        }
        if (!linkedPhysicsControl.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(idleAnimParameterId, thisAbilityState == linkedStateMachine.currentState || linkedStateMachine.currentState == PlayerStates.State.Reload);
    }
}
