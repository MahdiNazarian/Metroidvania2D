using UnityEngine;

public class WallJumpAbility : BaseAbility
{
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private float wallJumpMaximumTime;
    private float wallJumpMaxTimer;
    [SerializeField] private float wallJumpMininumTime;
    private float wallJumpMinimumTimer;

    private void OnDisable()
    {
        linkedGatherInput.RemoveJumpInstructions(TryToWallJump);
    }
    protected override void Initialize()
    {
        base.Initialize();
        linkedGatherInput.SetJumpInstructions(TryToWallJump);
    }
    private void TryToWallJump()
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;
        if (player.WallJumpConditionCheck())
        {
            linkedStateMachine.ChangeState(PlayerStates.State.WallJump);
            wallJumpMaxTimer = wallJumpMaximumTime;
            wallJumpMinimumTimer = wallJumpMininumTime;
            player.ForceFlip();
            if (player.facingRight)
                linkedPhysicsControl.rb.linearVelocity = new Vector2(wallJumpForce.x, wallJumpForce.y);
            else
                linkedPhysicsControl.rb.linearVelocity = new Vector2(-wallJumpForce.x, wallJumpForce.y);
        }
    }

    public override void ProcessAbility()
    {
        //player.Flip();
        wallJumpMaxTimer -= Time.deltaTime;
        wallJumpMinimumTimer -= Time.deltaTime;

        if (linkedPhysicsControl.grounded)
        {
            if (linkedGatherInput.valueX != 0)
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            else
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
        if (wallJumpMaxTimer <= 0)
        {
            if (linkedPhysicsControl.grounded)
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            else
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            return;
        }

        if (wallJumpMinimumTimer <= 0 && linkedPhysicsControl.wallDetected)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.WallSlide);
            wallJumpMaxTimer = -1;
        }
    }
}
