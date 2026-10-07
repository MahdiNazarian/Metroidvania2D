using UnityEngine;

public class LadderController : MonoBehaviour
{
    private LaddersAblility playerLadderComponent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerLadderComponent = collision.GetComponent<LaddersAblility>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(playerLadderComponent != null)
        {
            if (playerLadderComponent.isPermitted)
            {
                playerLadderComponent.canGoOnLadder = true;
                playerLadderComponent.ladderPositionX = transform.position.x;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerLadderComponent != null)
        {
            if (playerLadderComponent.isPermitted)
                playerLadderComponent.canGoOnLadder = false;
        }
    }
}
