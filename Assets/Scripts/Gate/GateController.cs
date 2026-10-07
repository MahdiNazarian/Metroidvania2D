using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private string levelName;
    [Header("Spawn Setting")]
    public SpawnData spawnDataForNextRoom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaveLoadManager.instance.Save<SpawnData>(spawnDataForNextRoom, SaveLoadManager.instance.spawnSaveFolder, SaveLoadManager.instance.spawnSaveFile);
            Player player = collision.GetComponent<Player>();
            player.gatherInput.DisablePlayerMainMap();
            player.physicsControl.ResetVelocity();
            LevelManager.instance.LoadLevelString(levelName);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
