using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public GatherInput gatherInput;
    [HideInInspector] public StateMachine stateMachine;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PhysicsControl physicsControl;
    [HideInInspector] public PlayerStats playerStats;
    private BaseAbility[] allAbilities;

    [HideInInspector] public bool facingRight = true;

    [Header("Current Weapon")]
    public GameObject currentWeapon;
    public ItemTypes currentWeaponType;
    public Weapon currentWeaponScript;

    [Header("Primary Weapon")]
    public GameObject primaryWeaponPrefab;

    [Header("Secondary Weapon")]
    public GameObject secondaryWeaponPrefab;

    [HideInInspector] public Vector3 defaultWeaponPositionVector;
    [HideInInspector] public Transform currentShootPosition;

    [Header("Shoot Positions")]
    public Transform standingShootPosition;
    public Transform crouchShootPosition;
    public Transform shootUpPosition;

    [Header("Secondary Shoot Positions")]
    public Transform secondaryStandingShootPosition;
    public Transform secondaryCrouchShootPosition;
    public Transform secondaryShootUpPosition;

    [HideInInspector] public Weapon[] allWeaponsList;

    private void Awake()
    {
        gatherInput = GetComponent<GatherInput>();
        stateMachine = new StateMachine();
        allAbilities = GetComponents<BaseAbility>();
        animator = GetComponent<Animator>();
        physicsControl = GetComponent<PhysicsControl>();
        playerStats = GetComponentInChildren<PlayerStats>();
        currentWeaponScript = currentWeapon.GetComponent<Weapon>();
        stateMachine.allAbilities = allAbilities;
        currentShootPosition = standingShootPosition;
        defaultWeaponPositionVector = currentShootPosition.localPosition;
        allWeaponsList = GetComponentsInChildren<Weapon>(true);
    }

    private void OnDisable()
    {
        foreach(Weapon weapon in allWeaponsList)
        {
            weapon.SaveWeaponData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(BaseAbility ability in allAbilities)
        {
            if(ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessAbility();
            }
            ability.UpdateAnimator();
        }
        Debug.Log("State is : " + stateMachine.currentState);
    }

    private void FixedUpdate()
    {
        foreach (BaseAbility ability in allAbilities)
        {
            if (ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessFixedAbility();
            }
        }
    }

    public void Flip()
    {
        if (facingRight && gatherInput.valueX < 0)
        {
            transform.Rotate(0, 180, 0);
            facingRight = !facingRight;
        }
        else if (!facingRight && gatherInput.valueX > 0)
        {
            transform.Rotate(0, 180, 0);
            facingRight = !facingRight;
        }
    }

    public void ForceFlip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    public bool WallJumpConditionCheck()
    {
        return !physicsControl.grounded && physicsControl.wallDetected;
    }

    public void SetWeaponPosition()
    {
        if(stateMachine.currentState == PlayerStates.State.Crouch)
        {
            SetCrouchShootPostion();
        }
        else if(stateMachine.currentState == PlayerStates.State.ShootUp)
        {
            SetShootUpPosition();
        }
        else
        {
            SetStandingShootPosition();
        }
    }

    public void SetCrouchShootPostion()
    {
        if(currentWeaponType == ItemTypes.PrimaryWeapon)
        {
            currentShootPosition = crouchShootPosition;
            currentWeapon.transform.position = crouchShootPosition.position;
        }
        else if(currentWeaponType == ItemTypes.SecondaryWeapon)
        {
            currentShootPosition = secondaryCrouchShootPosition;
            currentWeapon.transform.position = secondaryCrouchShootPosition.position;
        }
        defaultWeaponPositionVector = currentShootPosition.localPosition;
        changeWeaponRotation(0);
    }
    public void SetStandingShootPosition()
    {
        if(currentWeaponType == ItemTypes.PrimaryWeapon)
        {
            currentShootPosition = standingShootPosition;
            currentWeapon.transform.position = standingShootPosition.position;
            changeWeaponRotation(0);
        }
        else if (currentWeaponType == ItemTypes.SecondaryWeapon)
        {
            currentShootPosition = secondaryStandingShootPosition;
            currentWeapon.transform.position = secondaryStandingShootPosition.position;
        }
        defaultWeaponPositionVector = currentShootPosition.localPosition;
        changeWeaponRotation(0);
    }
    public void SetShootUpPosition()
    {
        if (currentWeaponType == ItemTypes.PrimaryWeapon)
        {
            currentShootPosition = shootUpPosition;
            currentWeapon.transform.position = shootUpPosition.position;
        }
        else if (currentWeaponType == ItemTypes.SecondaryWeapon)
        {
            currentShootPosition = secondaryShootUpPosition;
            currentWeapon.transform.position = secondaryShootUpPosition.position;
        }
        defaultWeaponPositionVector = currentShootPosition.localPosition;
        changeWeaponRotation(90);
    }
    public void changeWeaponRotation(float zRotation)
    {
        currentWeapon.transform.localRotation = Quaternion.Euler(0, 0, zRotation);
    }
    public void ActivateCurrentWeapon()
    {
        currentWeapon.SetActive(true);
    }
    public void DeactivateCurrentWeapon()
    {
        currentWeapon.SetActive(false);
    }
}
