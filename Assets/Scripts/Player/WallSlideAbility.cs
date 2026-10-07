using UnityEngine;

public class WallSlideAbility : BaseAbility
{
    [SerializeField] private float maxWallSlideSpeed;
    private string wallSlideAnimParameterString = "WallSlide";
    private int wallSlideAnimParameterID;

    protected override void Initialize()
    {
        base.Initialize();
        wallSlideAnimParameterID = Animator.StringToHash(wallSlideAnimParameterString);
    }

    public override void EnterAbility()
    {
        linkedPhysicsControl.rb.linearVelocity = Vector2.zero;
        player.DeactivateCurrentWeapon();
    }
    public override void ExitAbility()
    {
        player.ActivateCurrentWeapon();
    }

    public override void ProcessAbility()
    {
        if (linkedPhysicsControl.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
        else if((player.facingRight && linkedGatherInput.valueX < 0) || (!player.facingRight && linkedGatherInput.valueX > 0))
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysicsControl.wallDetected = false;
            linkedAnimator.SetBool(wallSlideAnimParameterID, false);
        }
        else if (!linkedPhysicsControl.wallDetected)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }
    }
    public override void ProcessFixedAbility()
    {
        linkedPhysicsControl.rb.linearVelocityY = Mathf.Clamp(linkedPhysicsControl.rb.linearVelocityY, -maxWallSlideSpeed, 1);
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(wallSlideAnimParameterID, thisAbilityState == linkedStateMachine.currentState);
    }
}
