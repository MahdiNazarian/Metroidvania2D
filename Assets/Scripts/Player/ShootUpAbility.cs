using UnityEngine;

public class ShootUpAbility : BaseAbility
{
    [SerializeField] private float airSpeed;
    private bool shootUpActive;
    private string animationParamString = "ShootUp";
    private int animationParamId;

    protected override void Initialize()
    {
        base.Initialize();
        linkedGatherInput.shootUpActionBlueprint.SetActionPerformedInstructions(TryToShootUp);
        linkedGatherInput.shootUpActionBlueprint.SetActionCancelledInstructions(CancellShootUp);
        animationParamId = Animator.StringToHash(animationParamString);

    }
    private void OnDisable()
    {
        linkedGatherInput.shootUpActionBlueprint.RemoveActionPerformedInstructions(TryToShootUp);
        linkedGatherInput.shootUpActionBlueprint.RemoveActionCancelledInstructions(CancellShootUp);
    }
    private void TryToShootUp()
    {
        if (!isPermitted || player.currentWeaponScript == null)
            return;
        if (linkedStateMachine.currentState == PlayerStates.State.Ladders || linkedStateMachine.currentState == PlayerStates.State.WallSlide ||
            linkedStateMachine.currentState == PlayerStates.State.KnockBack || linkedStateMachine.currentState == PlayerStates.State.Crouch ||
            linkedStateMachine.currentState == PlayerStates.State.Dash || linkedStateMachine.currentState == PlayerStates.State.Death ||
            linkedStateMachine.currentState == PlayerStates.State.Reload || linkedStateMachine.currentState == PlayerStates.State.ShootUp)
            return;
        linkedStateMachine.ChangeState(PlayerStates.State.ShootUp);
        if (linkedPhysicsControl.grounded)
            linkedPhysicsControl.ResetVelocity();
        shootUpActive = true;
    }
    private void CancellShootUp()
    {
        if (!shootUpActive)
            return;

        if (linkedStateMachine.currentState == PlayerStates.State.Ladders)
            return;

        if (!linkedPhysicsControl.grounded)
        {
            if(linkedGatherInput.valueX != 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            }
            else
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            }
        }
        else
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }
        shootUpActive = false;
    }
    public override void EnterAbility()
    {
        player.SetShootUpPosition();
        shootUpActive = true;
    }
    public override void ProcessFixedAbility()
    {
        if (!linkedPhysicsControl.grounded)
        {
            linkedPhysicsControl.rb.linearVelocityX = airSpeed * linkedGatherInput.valueX;
        }
        else
        {
            linkedPhysicsControl.rb.linearVelocityX = 0.0f;
        }
    }
    public override void ExitAbility()
    {
        player.SetStandingShootPosition();
        shootUpActive = false;
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(animationParamId, thisAbilityState == linkedStateMachine.currentState);
    }
}
