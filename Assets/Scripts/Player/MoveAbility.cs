using UnityEngine;

public class MoveAbility : BaseAbility
{
    private string runAnimParameterName = "Run";
    private int runAnimParameterID;

    [SerializeField] private float speed;
    protected override void Initialize()
    {
        base.Initialize();
        runAnimParameterID = Animator.StringToHash(runAnimParameterName);
    }
    public override void EnterAbility()
    {
        player.Flip();
    }
    public override void ProcessAbility()
    {
        player.Flip();
        if (linkedPhysicsControl.grounded && linkedGatherInput.valueX == 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
        if (!linkedPhysicsControl.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }

    }
    public override void ProcessFixedAbility()
    {
        linkedPhysicsControl.rb.linearVelocityX = speed * linkedGatherInput.valueX;
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(runAnimParameterID, thisAbilityState == linkedStateMachine.currentState);
    }


}
