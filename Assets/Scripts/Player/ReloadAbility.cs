using System.Collections;
using UnityEngine;

public class ReloadAbility : BaseAbility
{
    [SerializeField] private ReloadBarControl reloadBarControl;

    private Coroutine reloadCoroutine;

    protected override void Initialize()
    {
        base.Initialize();
        linkedGatherInput.SetReloadActionPerformedInstructions(TryToReload);

    }
    private void OnDisable()
    {
        linkedGatherInput.RemoveReloadActionPerformedInstructions(TryToReload);
    }

    public override void EnterAbility()
    {
        linkedPhysicsControl.ResetVelocity();
        reloadCoroutine = StartCoroutine(ReloadCoroutine());
    }

    private void TryToReload()
    {
        if (!isPermitted || player.currentWeaponScript is null)
            return;
        if (!linkedPhysicsControl.grounded || linkedStateMachine.currentState == PlayerStates.State.Dash ||
            linkedStateMachine.currentState == PlayerStates.State.Ladders || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;
        if (!player.currentWeaponScript.ReloadCheck() || player.currentWeaponScript.isRealoading)
            return;
        linkedStateMachine.ChangeState(PlayerStates.State.Reload);
    }

    private IEnumerator ReloadCoroutine()
    {
        float elapsedTime = 0;
        reloadBarControl.ActivateReloadBar();
        player.currentWeaponScript.isRealoading = true;
        while(elapsedTime <= player.currentWeaponScript.reloadTime)
        {
            elapsedTime += Time.deltaTime;
            reloadBarControl.UpdateReloadBar(elapsedTime, player.currentWeaponScript.reloadTime);
            yield return null;
        }
        player.currentWeaponScript.Reload();
        WeaponDisplayController.onUpdateAmmoDisplayInfo?.Invoke(player.currentWeaponScript.currentAmmo, player.currentWeaponScript.maxAmmo, player.currentWeaponScript.storageAmmo);
        if (linkedStateMachine.currentState != PlayerStates.State.Death || linkedStateMachine.currentState != PlayerStates.State.KnockBack)
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
    }
    public override void ExitAbility()
    {
        reloadBarControl.DeactivateReloadBar();
        player.currentWeaponScript.isRealoading = false;
        if (reloadCoroutine != null)
            StopCoroutine(ReloadCoroutine());
    }
}
