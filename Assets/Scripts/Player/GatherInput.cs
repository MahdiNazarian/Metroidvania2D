using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    private PlayerInput playerInputComponent;
    private InputActionMap playerMainMap;
    private InputActionMap uIMap;
    private InputActionMap MinimapMap;
    private InputActionMap changeMenuMap;
    private InputActionMap dialogueMap;


    [Header("Player Main Actions Map")]
    [SerializeField] private InputActionReference moveActionRef;
    [SerializeField] private InputActionReference jumpActionRef;
    [SerializeField] private InputActionReference dashActionRef;
    [SerializeField] private InputActionReference moveVerticalActionRef;
    [SerializeField] private InputActionReference crouchActionRef;
    [SerializeField] private InputActionReference interactActionRef;
    [SerializeField] private InputActionReference shootActionRef;
    [SerializeField] private InputActionReference reloadActionRef;
    public CustomInputBlueprint<bool> shootUpActionBlueprint;
    public CustomInputBlueprint<float> changeWeaponActionBlueprint;
    public CustomInputBlueprint<bool> nextDialogueActionBlueprint;

    [Header("Minmap Actions Map")]
    [SerializeField] private InputActionReference minimapMoveActionRef;
    [SerializeField] private InputActionReference minimapZoomActionRef;

    [Header("ChangeMenu Actions Map")]
    [SerializeField] private InputActionReference minimapActivator;

    #region Player Main Action Values
    [HideInInspector] public float valueX;
    [HideInInspector] public float valueY;
    [HideInInspector] public bool tryToJump;
    [HideInInspector] public bool tryToDash;
    [HideInInspector] public bool tryToCrouch;
    [HideInInspector] public bool tryToIntercat;
    [HideInInspector] public bool tryToShoot;
    [HideInInspector] public bool tryToReload;
    #endregion

    #region Minimap Action Values
    [HideInInspector] public Vector2 minimapMoveValue;
    [HideInInspector] public float minimapZoomValue;
    #endregion

    #region ChangeMenu Action Values
    #endregion

    #region player main action instructions
    private Action jumpActionInstructions;
    private Action jumpActionCancelledInstructions;

    private Action dashActionInstructions;

    private Action moveVerticalActionPerformedInstructions;
    private Action moveVerticalActionCancelledInstructions;

    private Action crouchActionPerformedInstructions;
    private Action crouchActionCancelledInstructions;

    private Action InteractActionPerformedInstructions;
    private Action InteractActionCancelledInstructions;

    private Action ShootActionPerformedInstructions;
    private Action ShootActionCancelledInstructions;

    private Action ReloadActionPerformedInstructions;
    private Action ReloadActionCancelledInstructions;
    #endregion

    #region Minimap Actions Instructions
    #endregion

    #region ChangeMenu Actions Instructions
    private Action minimapActivatorActionPerformedInstructions;
    private Action minimapActivatorActionCancelledInstructions;
    #endregion

    private void Awake()
    {
        playerInputComponent = GetComponent<PlayerInput>();
        playerMainMap = playerInputComponent.actions.FindActionMap("PlayerMain");
        uIMap = playerInputComponent.actions.FindActionMap("UI");
        changeMenuMap = playerInputComponent.actions.FindActionMap("ChangeMenu");
        MinimapMap = playerInputComponent.actions.FindActionMap("Minimap");
        dialogueMap = playerInputComponent.actions.FindActionMap("DialogueMap");
    }

    private void OnEnable()
    {
        jumpActionRef.action.performed += JumpActionPerformed;
        jumpActionRef.action.canceled += JumpActionCancelled;
        dashActionRef.action.performed += DashActionPerformed;
        dashActionRef.action.canceled += DashActionCancelled;
        moveVerticalActionRef.action.performed += MoveVerticalActionPerformed;
        moveVerticalActionRef.action.canceled += MoveVerticalActionCancelled;
        crouchActionRef.action.performed += CrouchActionPerformed;
        crouchActionRef.action.canceled += CrouchActionCancelled;
        interactActionRef.action.performed += InteractActionPerformed;
        interactActionRef.action.canceled += InteractActionCancelled;
        minimapActivator.action.performed += MinimapActivatorActionPerformed;
        minimapActivator.action.canceled += MinimapActivatorActionCancelled;
        shootActionRef.action.performed += ShootActionPerformed;
        shootActionRef.action.canceled += ShootActionCancelled;
        reloadActionRef.action.performed += ReloadActionPerformed;
        reloadActionRef.action.performed += ReloadActionCancelled;
        shootUpActionBlueprint.Initilization();
        changeWeaponActionBlueprint.Initilization();
        nextDialogueActionBlueprint.Initilization();
        playerInputComponent.SwitchCurrentActionMap(playerMainMap.id.ToString());
        changeMenuMap.Enable();
    }
    private void OnDisable()
    {
        playerMainMap.Disable();
        jumpActionRef.action.performed -= JumpActionPerformed;
        jumpActionRef.action.canceled -= JumpActionCancelled;
        dashActionRef.action.performed -= DashActionPerformed;
        dashActionRef.action.canceled -= DashActionCancelled;
        moveVerticalActionRef.action.performed -= MoveVerticalActionPerformed;
        moveVerticalActionRef.action.canceled -= MoveVerticalActionCancelled;
        crouchActionRef.action.performed -= CrouchActionPerformed;
        crouchActionRef.action.canceled -= CrouchActionCancelled;
        interactActionRef.action.performed -= InteractActionPerformed;
        interactActionRef.action.canceled -= InteractActionCancelled;
        minimapActivator.action.performed -= MinimapActivatorActionPerformed;
        minimapActivator.action.canceled -= MinimapActivatorActionCancelled;
        shootActionRef.action.performed -= ShootActionPerformed;
        shootActionRef.action.canceled -= ShootActionCancelled;
        reloadActionRef.action.performed -= ReloadActionPerformed;
        reloadActionRef.action.performed -= ReloadActionCancelled;
        shootUpActionBlueprint.Destruction();
        changeWeaponActionBlueprint.Destruction();
        nextDialogueActionBlueprint.Destruction();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        valueX = moveActionRef.action.ReadValue<float>();
        valueY = moveVerticalActionRef.action.ReadValue<float>();
        minimapMoveValue = minimapMoveActionRef.action.ReadValue<Vector2>();
        minimapZoomValue = minimapZoomActionRef.action.ReadValue<float>();
    }

    public void EnablePlayerMainMap()
    {
        playerMainMap.Enable();
    }
    public void DisablePlayerMainMap()
    {
        playerMainMap.Disable();
    }
    public void ChangeToMinimapMap()
    {
        playerInputComponent.SwitchCurrentActionMap(MinimapMap.id.ToString());
    }
    public void ChangeToPlayerMainMap()
    {
        playerInputComponent.SwitchCurrentActionMap(playerMainMap.id.ToString());
    }

    public void ChangeToDialogueMap()
    {
        playerInputComponent.SwitchCurrentActionMap(dialogueMap.id.ToString());
    }

    private void JumpActionPerformed(InputAction.CallbackContext context)
    {
        tryToJump = context.ReadValueAsButton();
        jumpActionInstructions?.Invoke();
    }
    private void JumpActionCancelled(InputAction.CallbackContext context)
    {
        tryToJump = context.ReadValueAsButton();
        jumpActionCancelledInstructions?.Invoke();
    }
    public void SetJumpInstructions(Action actionInstructions)
    {
        jumpActionInstructions += actionInstructions;
    }
    public void RemoveJumpInstructions(Action actionInstruction)
    {
        jumpActionInstructions -= actionInstruction;
    }
    public void SetJumpActionCancelledInstructions(Action instructions)
    {
        jumpActionCancelledInstructions += instructions;
    }
    public void RemoveJumpActionCancelledInstructions(Action instructions)
    {
        jumpActionCancelledInstructions -= instructions;
    }

    private void DashActionPerformed(InputAction.CallbackContext context)
    {
        tryToDash = context.ReadValueAsButton();
        dashActionInstructions?.Invoke();
    }
    private void DashActionCancelled(InputAction.CallbackContext context)
    {
        tryToDash = context.ReadValueAsButton();
    }

    public void SetDashInstructions(Action instruction)
    {
        dashActionInstructions += instruction;
    }
    public void RemoveDashInstructions(Action instruction)
    {
        dashActionInstructions -= instruction;
    }

    private void MoveVerticalActionPerformed(InputAction.CallbackContext context)
    {
        moveVerticalActionPerformedInstructions?.Invoke();
    }
    private void MoveVerticalActionCancelled(InputAction.CallbackContext context)
    {
        moveVerticalActionCancelledInstructions?.Invoke();
    }

    public void SetMoveVerticalPerformedInstructions(Action instructions)
    {
        moveVerticalActionPerformedInstructions += instructions;
    }
    public void RemoveMoveVerticalPerformedInstructions(Action instructions)
    {
        moveVerticalActionPerformedInstructions -= instructions;
    }
    public void SetMoveVerticalCancelledInstruction(Action instructions)
    {
        moveVerticalActionCancelledInstructions += instructions;
    }
    public void RemoveMoveVerticalCancelledInstructions(Action instructions)
    {
        moveVerticalActionCancelledInstructions -= instructions;
    }

    private void CrouchActionPerformed(InputAction.CallbackContext context)
    {
        tryToCrouch = context.ReadValueAsButton();
        crouchActionPerformedInstructions?.Invoke();
    }
    private void CrouchActionCancelled(InputAction.CallbackContext context)
    {
        tryToCrouch = context.ReadValueAsButton();
        crouchActionCancelledInstructions?.Invoke();
    }

    public void SetCrouchPerformedInstructions(Action instructions)
    {
        crouchActionPerformedInstructions += instructions;
    }
    public void RemoveCrouchPerformedInstructions(Action instructions)
    {
        crouchActionPerformedInstructions -= instructions;
    }
    public void SetCrouchCancelledInstructions(Action instructions)
    {
        crouchActionCancelledInstructions += instructions;
    }
    public void RemoveCrouchCancelledInstructions(Action instructions)
    {
        crouchActionCancelledInstructions -= instructions;
    }

    private void InteractActionPerformed(InputAction.CallbackContext context)
    {
        tryToIntercat = context.ReadValueAsButton();
        InteractActionPerformedInstructions?.Invoke();
    }
    private void InteractActionCancelled(InputAction.CallbackContext context)
    {
        tryToIntercat = context.ReadValueAsButton();
        InteractActionCancelledInstructions?.Invoke();
    }
    
    public void SetInteractionActionPerformedInstructions(Action instructions)
    {
        InteractActionPerformedInstructions += instructions;
    }
    public void RemoveInteractionActionPerformedInstructions(Action instructions)
    {
        InteractActionPerformedInstructions -= instructions;
    }
    public void SetInteractionActionCancelledInstructions(Action instructions)
    {
        InteractActionCancelledInstructions += instructions;
    }
    public void RemoveInteractionActionCancelledInstructions(Action instructions)
    {
        InteractActionCancelledInstructions -= instructions;
    }

    private void MinimapActivatorActionPerformed(InputAction.CallbackContext context)
    {
        if (playerMainMap.enabled)
            ChangeToMinimapMap();
        else
            ChangeToPlayerMainMap();
        minimapActivatorActionPerformedInstructions?.Invoke();
    }
    private void MinimapActivatorActionCancelled(InputAction.CallbackContext context)
    {
        minimapActivatorActionCancelledInstructions?.Invoke();
    }

    public void SetMinimapActivatorActionPerformedInstructions(Action instructions)
    {
        minimapActivatorActionPerformedInstructions += instructions;
    }
    public void RemoveMinimapActivatorActionPerformedInstructions(Action instructions)
    {
        minimapActivatorActionPerformedInstructions -= instructions;
    }
    public void SetMinimapActivatorActionCancelledInstructions(Action instructions)
    {
        minimapActivatorActionCancelledInstructions += instructions;
    }
    public void RemoveMinimapActivatorActionCancelledInstructions(Action instructions)
    {
        minimapActivatorActionCancelledInstructions -= instructions;
    }

    private void ShootActionPerformed(InputAction.CallbackContext context)
    {
        ShootActionPerformedInstructions?.Invoke();
        tryToShoot = context.ReadValueAsButton();
    }
    private void ShootActionCancelled(InputAction.CallbackContext context)
    {
        tryToShoot = context.ReadValueAsButton();
        ShootActionCancelledInstructions?.Invoke();
    }

    public void SetShootActionPerformedInstructions(Action instructions)
    {
        ShootActionPerformedInstructions += instructions;
    }
    public void RemoveShootActionPerformedInstructions(Action instructions)
    {
        ShootActionPerformedInstructions -= instructions;
    }
    public void SetShootActionCancelledInstructions(Action instructions)
    {
        ShootActionCancelledInstructions += instructions;
    }
    public void RemoveShootActionCancelledInstructions(Action instructions)
    {
        ShootActionCancelledInstructions -= instructions;
    }

    private void ReloadActionPerformed(InputAction.CallbackContext context)
    {
        ReloadActionPerformedInstructions?.Invoke();
    }
    private void ReloadActionCancelled(InputAction.CallbackContext context)
    {
        ReloadActionCancelledInstructions?.Invoke();
    }

    public void SetReloadActionPerformedInstructions(Action instructions)
    {
        ReloadActionPerformedInstructions += instructions;
    }
    public void SetReloadActionCancelledInstructions(Action instructions)
    {
        ReloadActionCancelledInstructions += instructions;
    }
    public void RemoveReloadActionPerformedInstructions(Action instructions)
    {
        ReloadActionPerformedInstructions -= instructions;
    }
    public void RemoveReloadActionCancelledInstructions(Action instructions)
    {
        ReloadActionCancelledInstructions -= instructions;
    }
}
