using System.Collections;
using UnityEngine;

public class PatrollingEnemyStateMachine : EnemySimpleStateMachine
{
    private PatrollingPhysicsController physicsController;

    [Header("Idle Setting")]
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    [SerializeField] private string idleAnimationName;
    private int idleAnimationHash;
    private float idleTimer;

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minMoveTime;
    [SerializeField] private float maxMoveTime;
    [SerializeField] private float turnCooldownTime;
    [SerializeField] private string moveAnimationName;
    private int moveAnimationHash;
    private float moveTimer;
    private float turnCooldownTimer;

    [Header("Attack Settings")]
    [SerializeField] private float checkBehindResetDelay;
    [SerializeField] private string attackAnimationName;
    private int attackAnimationHash;

    [Header("Death Settings")]
    [SerializeField] private string deathAnimationName;
    private int deathAnimationHash;

    protected override void Initialize()
    {
        base.Initialize();
        physicsController = GetComponent<PatrollingPhysicsController>();
        idleAnimationHash = Animator.StringToHash(idleAnimationName);
        moveAnimationHash = Animator.StringToHash(moveAnimationName);
        attackAnimationHash = Animator.StringToHash(attackAnimationName);
        deathAnimationHash = Animator.StringToHash(deathAnimationName);
    }
    #region Idle
    public override void EnterIdle()
    {
        animator.Play(idleAnimationHash, 0);
        idleTimer = Random.Range(minIdleTime, maxIdleTime);
        physicsController.ResetVelocity();
    }

    public override void UpdateIdle()
    {
        idleTimer -= Time.deltaTime;

        if(physicsController.playerBehind)
        {
            ForceFlip();
            moveSpeed *= -1;
            turnCooldownTimer = turnCooldownTime;
            ChangeState(EnemyStates.Move);
        }

        if (idleTimer <= 0 || physicsController.playerFront)
        {
            ChangeState(EnemyStates.Move);
        }
        if (physicsController.playerInAttackRange)
        {
            ChangeState(EnemyStates.Attack);
        }
    }
    public override void ExitIdle()
    {
        idleTimer = -1;
    }
    #endregion

    #region Move
    public override void EnterMove()
    {
        animator.Play(moveAnimationHash, 0);
        moveTimer = Random.Range(minMoveTime, maxMoveTime);
        physicsController.ResetVelocity();
    }
    public override void UpdateMove()
    {
        moveTimer -= Time.deltaTime;
        if(moveTimer <= 0 && !physicsController.playerFront)
        {
            ChangeState(EnemyStates.Idle);
        }
        if (turnCooldownTimer > 0)
            turnCooldownTimer -= Time.deltaTime;

        if((physicsController.wallDetected || !physicsController.groundDetected || physicsController.playerBehind) && turnCooldownTimer <= 0)
        {
            ForceFlip();
            moveSpeed *= -1;
            turnCooldownTimer = turnCooldownTime;
        }
        if (physicsController.playerInAttackRange)
        {
            ChangeState(EnemyStates.Attack);
        }
    }
    public override void FixedUpdateMove()
    {
        if(physicsController.groundDetected && !physicsController.wallDetected)
        {
            physicsController.rb.linearVelocityX = moveSpeed;
        }
    }
    public override void ExitMove()
    {
        moveTimer = -1;
    }
    #endregion

    #region Attack
    public override void EnterAttack()
    {
        animator.Play(attackAnimationHash, 0, 0);
        physicsController.ResetVelocity();
        physicsController.canCheckBehind = false;
    }
    public void EndOfAttack()
    {
        if (physicsController.playerInAttackRange)
        {
            animator.Play(attackAnimationHash, 0, 0);
            physicsController.ResetVelocity();
            
        }
        else
        {
            ChangeState(previousState);
        }
        StartCoroutine(CheckBehindDelay());
    }
    private IEnumerator CheckBehindDelay()
    {
        yield return new WaitForSeconds(checkBehindResetDelay);
        physicsController.canCheckBehind = true;
    }
    public override void ExitAttack()
    {
        physicsController.DisableDamageCollider();
    }
    #endregion

    #region Death
    public override void EnterDeath()
    {
        animator.Play(deathAnimationHash);
        physicsController.ResetVelocity();
        physicsController.DisableDeathColliders();
    }
    #endregion

}
