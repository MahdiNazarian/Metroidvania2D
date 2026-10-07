using System.Collections;
using UnityEngine;

public class KnockBackAblility : BaseAbility
{
    private Coroutine currentKnockBack;

    private string knockBackAnimParameterString = "KnockBack";
    private int knockBackAnimParameterID;

    protected override void Initialize()
    {
        base.Initialize();
        knockBackAnimParameterID = Animator.StringToHash(knockBackAnimParameterString);
    }
    public void StartKnockBack(float duration, Vector2 force, Transform enemyObject)
    {
        if (!player.playerStats.CanTakeDamage())
            return;
        if(currentKnockBack == null)
        {
            currentKnockBack = StartCoroutine(KnockBack(duration, force, enemyObject));
        }
        else
        {
            StopCoroutine(currentKnockBack);
            currentKnockBack = StartCoroutine(KnockBack(duration, force, enemyObject));
        }
    }
    public void StartSwingKnockBack(float duration, Vector2 force, int pushDirection)
    {
        if (!player.playerStats.CanTakeDamage())
            return;
        if (currentKnockBack == null)
        {
            currentKnockBack = StartCoroutine(SwingKnockBack(duration, force, pushDirection));
        }
        else
        {
            StopCoroutine(currentKnockBack);
            currentKnockBack = StartCoroutine(SwingKnockBack(duration, force, pushDirection));
        }
    }
    private IEnumerator KnockBack(float duration, Vector2 force, Transform enemyObject)
    {
        linkedStateMachine.ChangeState(PlayerStates.State.KnockBack);
        linkedPhysicsControl.ResetVelocity();
        if(transform.position.x > enemyObject.transform.position.x)
        {
            linkedPhysicsControl.rb.linearVelocity = force;
        }
        else
        {
            linkedPhysicsControl.rb.linearVelocity = new Vector2(-force.x, force.y);
        }
        yield return new WaitForSeconds(duration);
        if (player.playerStats.IsAlive())
        {
            if (linkedPhysicsControl.grounded)
            {
                if (linkedGatherInput.valueX != 0)
                    linkedStateMachine.ChangeState(PlayerStates.State.Run);
                else
                    linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            }
            else
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            }
        }
        else
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Death);
        }
    }
    private IEnumerator SwingKnockBack(float duration, Vector2 force, int pushDirection)
    {
        linkedStateMachine.ChangeState(PlayerStates.State.KnockBack);
        linkedPhysicsControl.ResetVelocity();
        force.x *= pushDirection;
        linkedPhysicsControl.rb.linearVelocity = force;
        yield return new WaitForSeconds(duration);
        if (player.playerStats.IsAlive())
        {
            if (linkedPhysicsControl.grounded)
            {
                if (linkedGatherInput.valueX != 0)
                    linkedStateMachine.ChangeState(PlayerStates.State.Run);
                else
                    linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            }
            else
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            }
        }
        else
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Death);
        }
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(knockBackAnimParameterID, linkedStateMachine.currentState == thisAbilityState);
    }
}
