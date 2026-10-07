using UnityEngine;

public class DeathAbility : BaseAbility
{
    private string deathAnimationParameterString = "Death";
    private int deathAnimationParameterId;
    protected override void Initialize()
    {
        base.Initialize();
        deathAnimationParameterId = Animator.StringToHash(deathAnimationParameterString);
    }
    public override void EnterAbility()
    {
        player.DeactivateCurrentWeapon();
        SpawnMode.spawnFromCheckpoint = true;
        linkedGatherInput.DisablePlayerMainMap();
        linkedPhysicsControl.ResetVelocity();
        if (linkedPhysicsControl.grounded)
        {
            linkedAnimator.SetBool(deathAnimationParameterId, true);
        }
        else
        {
            linkedAnimator.SetBool(deathAnimationParameterId, true);
        }
    }

    public void ResetGame()
    {
        CheckpointData checkData = SaveLoadManager.instance.Load<CheckpointData>(SaveLoadManager.instance.defaultSaveFolder, SaveLoadManager.instance.checkpointSaveFile);
        if (checkData != null)
        {
            LevelManager.instance.LoadLevelString(checkData.SceneToLoad);
        }
        else
        {
            LevelManager.instance.ResetLevel();
        }
    }
}
