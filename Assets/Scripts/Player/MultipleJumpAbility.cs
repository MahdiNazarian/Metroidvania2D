using UnityEngine;

public class MultipleJumpAbility : BaseAbility
{
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float airSpeed;

    [SerializeField] private float minimumAirTime;
    private float minimumAirTimeCounter;

    [SerializeField] private float maxJumpTime;
    private float maxJumpTimer;
    private bool jumping;

    [SerializeField] private float gravityScaleDivider;

    [SerializeField] private float maxNumberOfJumps;
    private float numberOfJumps;
    private bool canActivateAdditionalJumps;

    private string jumpAnimParameterString = "Jump";
    private string speedYAnimparameterString = "speedY";

    private int jumpAnimParameterId;
    private int speedYAnimParameterId;

    private void OnDisable()
    {
        linkedGatherInput.RemoveJumpInstructions(TryToJump);
        linkedGatherInput.RemoveJumpActionCancelledInstructions(StopJump);
    }

    protected override void Initialize()
    {
        base.Initialize();
        linkedGatherInput.SetJumpInstructions(TryToJump);
        linkedGatherInput.SetJumpActionCancelledInstructions(StopJump);
        jumpAnimParameterId = Animator.StringToHash(jumpAnimParameterString);
        speedYAnimParameterId = Animator.StringToHash(speedYAnimparameterString);
        numberOfJumps = maxNumberOfJumps;
    }

    private void TryToJump()
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;

        if (linkedStateMachine.currentState == PlayerStates.State.Crouch || linkedStateMachine.currentState == PlayerStates.State.Reload)
            return;

        if (linkedStateMachine.currentState == PlayerStates.State.Ladders)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysicsControl.rb.linearVelocityY = jumpSpeed;
            minimumAirTimeCounter = minimumAirTime;
            maxJumpTimer = maxJumpTime;
            jumping = true;

            numberOfJumps = maxNumberOfJumps;
            numberOfJumps--;
            canActivateAdditionalJumps = true;
            Debug.Log("ladderJump");
            return;
        }
        if (linkedPhysicsControl.coyoteTimer > 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysicsControl.rb.linearVelocityY = jumpSpeed;
            minimumAirTimeCounter = minimumAirTime;
            linkedPhysicsControl.coyoteTimer = -1;
            maxJumpTimer = maxJumpTime;
            jumping = true;

            numberOfJumps = maxNumberOfJumps;
            numberOfJumps--;
            canActivateAdditionalJumps = true;
            Debug.Log("normalJump");
            return;
        }

        if(numberOfJumps > 0 && canActivateAdditionalJumps)
        {
            linkedPhysicsControl.EnableGravity();
            linkedPhysicsControl.rb.linearVelocityY = jumpSpeed;
            minimumAirTimeCounter = minimumAirTime;
            linkedPhysicsControl.coyoteTimer = -1;
            maxJumpTimer = maxJumpTime;
            jumping = true;

            numberOfJumps--;
            Debug.Log("doubleJump");
        }
        else
        {
            canActivateAdditionalJumps = false;
        }
    }

    private void StopJump()
    {
        jumping = false;
    }
    public override void ProcessAbility()
    {
        player.Flip();
        minimumAirTimeCounter -= Time.deltaTime;
        if (jumping)
        {
            maxJumpTimer -= Time.deltaTime;
            if (maxJumpTimer <= 0)
            {
                jumping = false;
            }
        }
        if (linkedPhysicsControl.grounded && minimumAirTimeCounter < 0)
        {
            if (linkedGatherInput.valueX != 0)
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
            if (jumping)
                linkedPhysicsControl.rb.linearVelocity = new Vector2(airSpeed * linkedGatherInput.valueX, jumpSpeed);
            else
                linkedPhysicsControl.rb.linearVelocityX = airSpeed * linkedGatherInput.valueX;
        }
        if (linkedPhysicsControl.rb.linearVelocityY < 0)
        {
            linkedPhysicsControl.rb.gravityScale = linkedPhysicsControl.GetStartingGravity() / gravityScaleDivider;
        }
    }

    public override void ExitAbility()
    {
        linkedPhysicsControl.EnableGravity();
        canActivateAdditionalJumps = false;
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(jumpAnimParameterId, thisAbilityState == linkedStateMachine.currentState || linkedStateMachine.currentState == PlayerStates.State.WallJump);
        linkedAnimator.SetFloat(speedYAnimParameterId, linkedPhysicsControl.rb.linearVelocityY);
    }
}
