using UnityEngine;

public class TraderController : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue dialogue;
    public void CustomInteract()
    {
        DialogueManager.instance.StartConversation(dialogue);
    }
}
