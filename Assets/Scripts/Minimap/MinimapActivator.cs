using UnityEngine;

public class MinimapActivator : MonoBehaviour
{
    [SerializeField] private CanvasGroup minimapCanvasGroup;
    private Player playerRef;

    private void Start()
    {
        playerRef = GetComponent<Player>();
        playerRef.gatherInput.SetMinimapActivatorActionPerformedInstructions(activateMinimap);
    }
    private void OnDisable()
    {
        playerRef.gatherInput.RemoveMinimapActivatorActionPerformedInstructions(activateMinimap);
    }

    private void activateMinimap()
    {
        if(minimapCanvasGroup.alpha <= 0)
        {
            minimapCanvasGroup.alpha = 1;
        }
        else if(minimapCanvasGroup.alpha >= 1)
        {
            minimapCanvasGroup.alpha = 0;
        }
    }
}