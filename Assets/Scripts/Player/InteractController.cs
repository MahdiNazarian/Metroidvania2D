using UnityEngine;

public class InteractController : MonoBehaviour
{
    private GatherInput gatherInput;
    private IInteractable currentInteractable;

    private void Start()
    {
        gatherInput = GetComponentInParent<GatherInput>();
        gatherInput.SetInteractionActionPerformedInstructions(TryToInteract);
    }
    private void OnDisable()
    {
        gatherInput.RemoveInteractionActionPerformedInstructions(TryToInteract);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        currentInteractable = null;
    }
    private void TryToInteract()
    {
        currentInteractable.CustomInteract();
    }
}
