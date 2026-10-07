using UnityEngine;

public class EnemySimpleStateMachine : MonoBehaviour
{
    [HideInInspector] public EnemyStates previousState;
    [HideInInspector] public EnemyStates currentState;
    protected Animator animator;
    public bool facingRight = true;
    public enum EnemyStates
    {
        Idle,
        Move,
        Attack,
        Death
    }

    public void ForceFlip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    private void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyStates.Idle:
                UpdateIdle();
                break;
            case EnemyStates.Move:
                UpdateMove();
                break;
            case EnemyStates.Attack:
                UpdateAttack();
                break;
            case EnemyStates.Death:
                UpdateDeath();
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyStates.Idle:
                FixedUpdateIdle();
                break;
            case EnemyStates.Move:
                FixedUpdateMove();
                break;
            case EnemyStates.Attack:
                FixedUpdateAttack();
                break;
            case EnemyStates.Death:
                FixedUpdateDeath();
                break;
        }
    }


    public void ChangeState(EnemyStates newState)
    {
        if (newState == currentState)
            return;

        ExitState();
        previousState = currentState;
        currentState = newState;
        EnterState();
    }

    protected void EnterState()
    {
        switch (currentState)
        {
            case EnemyStates.Idle:
                EnterIdle();
                break;
            case EnemyStates.Move:
                EnterMove();
                break;
            case EnemyStates.Attack:
                EnterAttack();
                break;
            case EnemyStates.Death:
                EnterDeath();
                break;
        }
    }
    protected void ExitState()
    {
        switch (currentState)
        {
            case EnemyStates.Idle:
                ExitIdle();
                break;
            case EnemyStates.Move:
                ExitMove();
                break;
            case EnemyStates.Attack:
                ExitAttack();
                break;
            case EnemyStates.Death:
                ExitDeath();
                break;
        }
    }

    #region Idle Phases
    public virtual void EnterIdle()
    {

    }
    public virtual void UpdateIdle()
    {

    }
    public virtual void FixedUpdateIdle()
    {

    }
    public virtual void ExitIdle()
    {

    }
    #endregion

    #region Move Phases
    public virtual void EnterMove()
    {

    }
    public virtual void UpdateMove()
    {

    }
    public virtual void FixedUpdateMove()
    {

    }
    public virtual void ExitMove()
    {

    }
    #endregion

    #region Attack Phases
    public virtual void EnterAttack()
    {

    }
    public virtual void UpdateAttack()
    {

    }
    public virtual void FixedUpdateAttack()
    {

    }
    public virtual void ExitAttack()
    {

    }
    #endregion
    #region Death Phases
    public virtual void EnterDeath()
    {

    }
    public virtual void UpdateDeath()
    {

    }
    public virtual void FixedUpdateDeath()
    {

    }
    public virtual void ExitDeath()
    {

    }
    #endregion

}
