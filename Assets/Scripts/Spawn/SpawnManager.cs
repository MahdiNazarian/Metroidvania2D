using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    private SpawnIdentifier[] spawnPoints;
    private CheckpointIdentifiers[] checkpoints;
    private SpawnData spawnData = new SpawnData();
    private CheckpointData checkData = new CheckpointData();
    private Player player;
    private bool canLoadFromCheckpoint = false;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        checkData = SaveLoadManager.instance.Load<CheckpointData>(SaveLoadManager.instance.defaultSaveFolder, SaveLoadManager.instance.checkpointSaveFile);
        if (checkData != null)
        {
            if (checkData.SceneToLoad == SceneManager.GetActiveScene().name)
            {
                canLoadFromCheckpoint = true;
            }
        }
        if (SpawnMode.spawnFromCheckpoint && canLoadFromCheckpoint)
        {
            checkpoints = GetComponentsInChildren<CheckpointIdentifiers>();
            foreach(CheckpointIdentifiers checkId in checkpoints)
            {
                if (checkId.checkpointKey.Equals(checkData.CheckpointKey))
                {
                    player.transform.position = checkId.transform.position;
                    if (!spawnData.facingRight) player.ForceFlip();
                    break;
                }
            }
            SpawnMode.spawnFromCheckpoint = false;
        }
        else
        {
            spawnPoints = GetComponentsInChildren<SpawnIdentifier>();
            spawnData = SaveLoadManager.instance.Load<SpawnData>(SaveLoadManager.instance.spawnSaveFolder, SaveLoadManager.instance.spawnSaveFile);
            foreach (SpawnIdentifier spawnId in spawnPoints)
            {
                if (spawnId.spawnKey.Equals(spawnData.spawnPointKey))
                {
                    player.transform.position = spawnId.transform.position;
                    if (!spawnData.facingRight) player.ForceFlip();
                    break;
                }
            }
        }
    }

    
}
