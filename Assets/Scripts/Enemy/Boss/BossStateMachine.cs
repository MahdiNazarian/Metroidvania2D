using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    [HideInInspector] public BossStates previousState;
    [HideInInspector] public BossStates currentState;
    protected Animator animator;
    public bool facingRight = true;
    public enum BossStates
    {
        Idle,
        Teleport,
        Attack,
        RangeAttack,
        Summon,
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
            case BossStates.Idle:
                UpdateIdle();
                break;
            case BossStates.Teleport:
                UpdateTeleport();
                break;
            case BossStates.Attack:
                UpdateAttack();
                break;
            case BossStates.RangeAttack:
                UpdateRangeAttack();
                break;
            case BossStates.Summon:
                UpdateSummon();
                break;
            case BossStates.Death:
                UpdateDeath();
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case BossStates.Idle:
                FixedUpdateIdle();
                break;
            case BossStates.Teleport:
                FixedUpdateTeleport();
                break;
            case BossStates.Attack:
                FixedUpdateAttack();
                break;
            case BossStates.RangeAttack:
                FixedUpdateRangeAttack();
                break;
            case BossStates.Summon:
                FixedUpdateSummon();
                break;
            case BossStates.Death:
                FixedUpdateDeath();
                break;
        }
    }


    public void ChangeState(BossStates newState)
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
            case BossStates.Idle:
                EnterIdle();
                break;
            case BossStates.Teleport:
                EnterTeleport();
                break;
            case BossStates.Attack:
                EnterAttack();
                break;
            case BossStates.RangeAttack:
                EnterRangeAttack();
                break;
            case BossStates.Summon:
                EnterSummon();
                break;
            case BossStates.Death:
                EnterDeath();
                break;
        }
    }
    protected void ExitState()
    {
        switch (currentState)
        {
            case BossStates.Idle:
                ExitIdle();
                break;
            case BossStates.Teleport:
                ExitTeleport();
                break;
            case BossStates.Attack:
                ExitAttack();
                break;
            case BossStates.RangeAttack:
                ExitRangeAttack();
                break;
            case BossStates.Summon:
                ExitSummon();
                break;
            case BossStates.Death:
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
    public virtual void EnterTeleport()
    {

    }
    public virtual void UpdateTeleport()
    {

    }
    public virtual void FixedUpdateTeleport()
    {

    }
    public virtual void ExitTeleport()
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
    #region Range Attack Phases
    public virtual void EnterRangeAttack()
    {

    }
    public virtual void UpdateRangeAttack()
    {

    }
    public virtual void FixedUpdateRangeAttack()
    {

    }
    public virtual void ExitRangeAttack()
    {

    }
    #endregion

    #region Summon Phases
    public virtual void EnterSummon()
    {

    }
    public virtual void UpdateSummon()
    {

    }
    public virtual void FixedUpdateSummon()
    {

    }
    public virtual void ExitSummon()
    {

    }
    #endregion

}