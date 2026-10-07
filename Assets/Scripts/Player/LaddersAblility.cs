using UnityEngine;

public class LaddersAblility : BaseAbility
{
    [SerializeField] private float climbSpeed;
    [SerializeField] private float ladderMaxTime;
    private float ladderMaxTimer;

    private bool climb;

    public bool canGoOnLadder;
    public float ladderPositionX;

    private string laddderAnimParameterString = "Climb";
    private int ladderAnimParameterID;

    protected override void Initialize()
    {
        base.Initialize();
        ladderMaxTimer = ladderMaxTime;
        linkedGatherInput.SetMoveVerticalPerformedInstructions(TryToClimb);
        linkedGatherInput.SetMoveVerticalCancelledInstruction(StopClimb);
        ladderAnimParameterID = Animator.StringToHash(laddderAnimParameterString);
    }
    private void OnDisable()
    {
        linkedGatherInput.RemoveMoveVerticalPerformedInstructions(TryToClimb);
        linkedGatherInput.RemoveMoveVerticalCancelledInstructions(StopClimb);
    }
    private void TryToClimb()
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;
        linkedAnimator.enabled = true;
        if (linkedStateMachine.currentState == PlayerStates.State.Ladders || linkedStateMachine.currentState == PlayerStates.State.Dash || !canGoOnLadder || linkedStateMachine.currentState == PlayerStates.State.Reload)
            return;

        linkedStateMachine.ChangeState(PlayerStates.State.Ladders);
        linkedPhysicsControl.DisableGravity();
        linkedPhysicsControl.ResetVelocity();
        transform.position = new Vector3(ladderPositionX, transform.position.y, transform.position.z);
        climb = true;
        ladderMaxTimer = ladderMaxTime;
    }

    private void StopClimb()
    {
        if (!isPermitted)
            return;
        if (linkedStateMachine.currentState != PlayerStates.State.Ladders)
            return;
        linkedPhysicsControl.ResetVelocity();
        linkedAnimator.enabled = false;
    }

    public override void EnterAbility()
    {
        player.DeactivateCurrentWeapon();
    }

    public override void ProcessAbility()
    {
        if (climb)
            ladderMaxTimer -= Time.deltaTime;

        if(!canGoOnLadder && !linkedPhysicsControl.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }

        if(linkedPhysicsControl.grounded && ladderMaxTimer <= 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
    }

    public override void ProcessFixedAbility()
    {
        if (climb)
            linkedPhysicsControl.rb.linearVelocity = new Vector2(0, linkedGatherInput.valueY * climbSpeed);
    }

    public override void ExitAbility()
    {
        linkedPhysicsControl.EnableGravity();
        climb = false;
        linkedAnimator.enabled = true;
        player.ActivateCurrentWeapon();
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(ladderAnimParameterID, thisAbilityState == linkedStateMachine.currentState);
    }

}
