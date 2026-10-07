using UnityEngine;

public class CrouchAbility : BaseAbility
{
    [SerializeField] private float crouchSpeed;
    private string crouchAnimParameterString = "Crouch";
    private int crouchAnimParameterID;

    private string speedXAnimParameterString = "speedX";
    private int speedXAnimParameterID;

    private bool wantToStand;
    protected override void Initialize()
    {
        base.Initialize();
        linkedGatherInput.SetCrouchPerformedInstructions(TryToCrouch);
        linkedGatherInput.SetCrouchCancelledInstructions(StopCrouch);
        crouchAnimParameterID = Animator.StringToHash(crouchAnimParameterString);
        speedXAnimParameterID = Animator.StringToHash(speedXAnimParameterString);
    }
    private void OnDisable()
    {
        linkedGatherInput.RemoveCrouchPerformedInstructions(TryToCrouch);
        linkedGatherInput.RemoveCrouchCancelledInstructions(StopCrouch);
    }
    private void TryToCrouch()
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;
        if (!linkedPhysicsControl.grounded || linkedStateMachine.currentState == PlayerStates.State.Dash || linkedStateMachine.currentState == PlayerStates.State.Ladders
            || linkedStateMachine.currentState == PlayerStates.State.Reload)
            return;
        linkedStateMachine.ChangeState(PlayerStates.State.Crouch);
        wantToStand = false;
    }
    private void StopCrouch()
    {
        if (!isPermitted)
            return;
        if (linkedStateMachine.currentState != PlayerStates.State.Crouch)
            return;

        if (linkedPhysicsControl.ceilingDetected)
        {
            wantToStand = true;
            return;
        }

        if(linkedGatherInput.valueX == 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
        else if(linkedGatherInput.valueX != 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Run);
        }
    }
    public override void EnterAbility()
    {
        linkedPhysicsControl.activateCrouch();
        player.playerStats.EnableStatsCrouchCollider();
        player.SetCrouchShootPostion();
    }
    public override void ExitAbility()
    {
        linkedPhysicsControl.activateStand();
        player.playerStats.EnableStatsStandCollider();
        player.SetStandingShootPosition();
        wantToStand = false;
    }

    public override void ProcessAbility()
    {
        player.Flip();

        if (wantToStand && !linkedPhysicsControl.ceilingDetected)
        {
            if (linkedGatherInput.valueX == 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            }
            else if (linkedGatherInput.valueX != 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            }
        }

        if (!linkedPhysicsControl.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }
    }

    public override void ProcessFixedAbility()
    {
        linkedPhysicsControl.rb.linearVelocityX = linkedGatherInput.valueX * crouchSpeed;
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(crouchAnimParameterID, linkedStateMachine.currentState == thisAbilityState);
        linkedAnimator.SetFloat(speedXAnimParameterID, Mathf.Abs(linkedPhysicsControl.rb.linearVelocityX));
    }
}
