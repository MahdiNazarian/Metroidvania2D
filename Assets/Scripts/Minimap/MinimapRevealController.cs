using UnityEngine;

public class MinimapRevealController : MonoBehaviour
{
    private MinimapDisplayController minimapDisplayController;
    [SerializeField] private string minimapKeyToAdd;
    void Start()
    {
        minimapDisplayController = FindAnyObjectByType<MinimapDisplayController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            minimapDisplayController.LoadMinimapData();
            minimapDisplayController.minimapData.AddToListWithCheck(minimapKeyToAdd);
            minimapDisplayController.DisplayUnlockedMinimaps();
        }
    }

}
