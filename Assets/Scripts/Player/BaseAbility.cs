using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    protected Player player;

    public PlayerStates.State thisAbilityState;
    public bool isPermitted = true;

    protected GatherInput linkedGatherInput;
    protected StateMachine linkedStateMachine;
    protected Animator linkedAnimator;
    protected PhysicsControl linkedPhysicsControl;

    protected virtual void Start()
    {
        Initialize();
    }

    public virtual void EnterAbility()
    {

    }

    public virtual void ExitAbility()
    {

    }
    public virtual void ProcessAbility()
    {

    }
    public virtual void ProcessFixedAbility()
    {

    }

    public virtual void UpdateAnimator()
    {

    }

    protected virtual void Initialize()
    {
        player = GetComponent<Player>();
        
        if (player != null)
        {
            linkedStateMachine = player.stateMachine;
            linkedGatherInput = player.gatherInput;
            linkedAnimator = player.animator;
            linkedPhysicsControl = player.physicsControl;
        }
    }
}
