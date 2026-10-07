public class StateMachine
{
    public PlayerStates.State previousState;
    public PlayerStates.State currentState;

    public BaseAbility[] allAbilities;

    public void ChangeState(PlayerStates.State newState)
    {
        foreach (BaseAbility ability in allAbilities)
        {
            if (ability.thisAbilityState == newState)
            {
                if (!ability.isPermitted)
                {
                    return;
                }
            }
        }

        foreach (BaseAbility ability in allAbilities)
        {
            if(currentState == ability.thisAbilityState)
            {
                ability.ExitAbility();
                previousState = currentState;
                break;
            }
        }

        foreach(BaseAbility ability in allAbilities)
        {
            if(ability.thisAbilityState == newState)
            {
                if (ability.isPermitted)
                {
                    currentState = newState;
                    ability.EnterAbility();
                }
                break;
            }
        }
    }

    public void ForceChangeState(PlayerStates.State newState)
    {
        previousState = currentState;
        currentState = newState;
    }
}
