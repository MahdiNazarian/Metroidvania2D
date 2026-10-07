using UnityEngine;

public class JumpAbility : BaseAbility
{
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float airSpeed;

    [SerializeField] private float minimumAirTime;
    private float minimumAirTimeCounter;

    private string jumpAnimParameterString = "Jump";
    private string speedYAnimparameterString = "speedY";

    private int jumpAnimParameterId;
    private int speedYAnimParameterId;

    private void OnDisable()
    {
        linkedGatherInput.RemoveJumpInstructions(TryToJump);
    }

    protected override void Initialize()
    {
        base.Initialize();
        linkedGatherInput.SetJumpInstructions(TryToJump);
        jumpAnimParameterId = Animator.StringToHash(jumpAnimParameterString);
        speedYAnimParameterId = Animator.StringToHash(speedYAnimparameterString);
    }

    private void TryToJump()
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;

        if (linkedStateMachine.currentState == PlayerStates.State.Crouch || linkedStateMachine.currentState == PlayerStates.State.Reload)
            return;

        if(linkedStateMachine.currentState == PlayerStates.State.Ladders)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysicsControl.rb.linearVelocityY = jumpSpeed;
            minimumAirTimeCounter = minimumAirTime;
        }
        else if (linkedPhysicsControl.coyoteTimer > 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysicsControl.rb.linearVelocityY = jumpSpeed;
            minimumAirTimeCounter = minimumAirTime;
            linkedPhysicsControl.coyoteTimer = -1;
        }
    }

    public override void ProcessAbility()
    {
        player.Flip();
        minimumAirTimeCounter -= Time.deltaTime;
        if(linkedPhysicsControl.grounded && minimumAirTimeCounter < 0)
        {
            if(linkedGatherInput.valueX != 0)
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            else
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
        if (player.WallJumpConditionCheck())
        {
            linkedStateMachine.ChangeState(PlayerStates.State.WallSlide);
        }
    }

    public override void ProcessFixedAbility()
    {
        if (!linkedPhysicsControl.grounded)
        {
            linkedPhysicsControl.rb.linearVelocityX = airSpeed * linkedGatherInput.valueX;
        }
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(jumpAnimParameterId, thisAbilityState == linkedStateMachine.currentState || linkedStateMachine.currentState == PlayerStates.State.WallJump);
        linkedAnimator.SetFloat(speedYAnimParameterId, linkedPhysicsControl.rb.linearVelocityY);
    }
}
