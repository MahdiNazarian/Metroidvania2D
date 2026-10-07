using UnityEngine;

public class FirstBossStateMachine : BossStateMachine
{
    private BossPhysicsController physicsController;
    private Player player;

    [Header("Idle Setting")]
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    [SerializeField] private string idleAnimationName;
    private int idleAnimationHash;
    private float idleTimer;

    [Header("Teleport Setting")]
    [SerializeField] private string teleportInAnimationName;
    [SerializeField] private string teleportOutAnimationName;
    [SerializeField] private float minTeleportTime;
    [SerializeField] private float maxTeleportTime;
    [SerializeField] private float teleportOffsetFromPlayer;
    [SerializeField] private Transform[] teleportPoints;
    private int teleportInAnimationHash;
    private int teleportOutAnimationHash;
    private int teleportIndex;
    private int lastTeleportIndex;
    private bool canModifyTeleportData = false;
    private float teleportTimer;

    [Header("Meele Attack Setting")]
    [SerializeField] private string meeleAttackAnimationName;
    [SerializeField] private float meeleAttackCooldownTime;
    private int meeleAttackAnimationHash;
    private float meeleAttackCooldownTimer;

    [Header("Range Attack Settings")]
    [SerializeField] private string rangeAttackAnimationName;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileShootingPoint;
    private int rangeAttackAnimationHash;

    [Header("Death Settings")]
    [SerializeField] private string deathAnimationName;
    [SerializeField] private GameObject bossHeadPrefab;
    private int deathAnimationHash;

    protected override void Initialize()
    {
        base.Initialize();
        physicsController = GetComponent<BossPhysicsController>();
        idleAnimationHash = Animator.StringToHash(idleAnimationName);
        teleportInAnimationHash = Animator.StringToHash(teleportInAnimationName);
        teleportOutAnimationHash = Animator.StringToHash(teleportOutAnimationName);
        meeleAttackAnimationHash = Animator.StringToHash(meeleAttackAnimationName);
        rangeAttackAnimationHash = Animator.StringToHash(rangeAttackAnimationName);
        deathAnimationHash = Animator.StringToHash(deathAnimationName);
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    #region Idle
    public override void EnterIdle()
    {
        animator.Play(idleAnimationHash);
        idleTimer = Random.Range(minIdleTime, maxIdleTime);
    }
    public override void UpdateIdle()
    {
        meeleAttackCooldownTimer -= Time.deltaTime;
        if(meeleAttackCooldownTimer <= 0 && physicsController.playerInAttackRange)
        {
            ChangeState(BossStates.Attack);
            return;
        }
        idleTimer -= Time.deltaTime;

        if(idleTimer <= 0)
        {
            ChangeState(BossStates.Teleport);
        }
    }
    #endregion

    #region Teleport
    public override void EnterTeleport()
    {
        physicsController.DisableStatsCollider();
        physicsController.DisablePlayerDetectionCollider();
        teleportIndex = Random.Range(0, teleportPoints.Length);
        while(teleportIndex == lastTeleportIndex)
        {
            teleportIndex = Random.Range(0, teleportPoints.Length);
        }
        lastTeleportIndex = teleportIndex;
        animator.Play(teleportOutAnimationHash);
    }
    public override void UpdateTeleport()
    {
        if (!canModifyTeleportData)
            return;
        teleportTimer -= Time.deltaTime;
        if (physicsController.playerInAttackRange)
        {
            ChangeState(BossStates.Attack);
        }
        else if( teleportTimer <= 0)
        {
            ChangeState(BossStates.RangeAttack);
        }
    }
    public override void ExitTeleport()
    {
        canModifyTeleportData = false;
    }
    public void Teleport()
    {
        int randomChoice = Random.Range(0, 2);
        if(randomChoice == 0)
        {
            transform.position = teleportPoints[teleportIndex].position;
        }
        else
        {
            if(player != null)
            {
                transform.position = player.transform.position + Vector3.up * teleportOffsetFromPlayer;
            }
            else
            {
                transform.position = teleportPoints[teleportIndex].position;
            }
        }
        animator.Play(teleportInAnimationHash);
    }
    public void EnableCheckingTeleport()
    {
        canModifyTeleportData = true;
        teleportTimer = Random.Range(minTeleportTime, maxTeleportTime);
        physicsController.EnableStatsCollider();
        physicsController.EnablePlayerDetectionCollider();
        animator.Play(idleAnimationHash);
    }
    #endregion
    #region MeeleAttack
    public override void EnterAttack()
    {
        physicsController.DisablePlayerDetectionCollider();
        physicsController.playerInAttackRange = false;
        animator.Play(meeleAttackAnimationHash);
    }
    public override void ExitAttack()
    {
        physicsController.EnablePlayerDetectionCollider();
        meeleAttackCooldownTimer = meeleAttackCooldownTime;
    }
    public void EndAttack()
    {
        ChangeState(BossStates.Idle);
    }
    #endregion

    #region Range Attack
    public override void EnterRangeAttack()
    {
        animator.Play(rangeAttackAnimationHash);
    }
    public void ShootProjectile()
    {
        FirstBossProjectileController projectile = Instantiate(projectilePrefab, projectileShootingPoint).GetComponent<FirstBossProjectileController>();
        if(player != null)
        {
            projectile.MakeProjectile(player.transform);
        }
        else
        {
            Destroy(projectile.gameObject);
        }
    }
    #endregion
    #region Death
    public override void EnterDeath()
    {
        physicsController.DisableAllColliders();
        animator.Play(deathAnimationHash);
    }
    public void DeathAnimationEvent()
    {
        Instantiate(bossHeadPrefab, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
    #endregion
}
