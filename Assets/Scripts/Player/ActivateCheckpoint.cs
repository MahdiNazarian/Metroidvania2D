using UnityEngine;

public class ActivateCheckpoint : MonoBehaviour
{
    private Player player;
    [HideInInspector]
    public CheckpointController checkpointController;

    private void Start()
    {
        player = GetComponent<Player>();
        player.gatherInput.SetInteractionActionPerformedInstructions(TryToActivateCheckpoint);
    }

    private void OnDisable()
    {
        player.gatherInput.RemoveInteractionActionPerformedInstructions(TryToActivateCheckpoint);
    }

    private void TryToActivateCheckpoint()
    {
        if(checkpointController != null)
        {
            checkpointController.Activate();
        }
    }
}
