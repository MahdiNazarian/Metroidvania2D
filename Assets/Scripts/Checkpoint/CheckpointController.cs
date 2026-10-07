using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private Sprite CheckPointActiveSprite; 
    [SerializeField] private Sprite CheckPointDeactiveSprite;
    [SerializeField] private CheckpointData checkData;
    private SpriteRenderer spriter;
    private BoxCollider2D boxCollider;
    private bool activated = false;
    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        CheckpointData loadedCheckpointData = SaveLoadManager.instance.Load<CheckpointData>(SaveLoadManager.instance.defaultSaveFolder, SaveLoadManager.instance.checkpointSaveFile);
        if(loadedCheckpointData == null)
        {
            spriter.sprite = CheckPointDeactiveSprite;
            activated = false;
        }
        else if(checkData.CheckpointKey == loadedCheckpointData.CheckpointKey && checkData.SceneToLoad == loadedCheckpointData.SceneToLoad)
        {
            spriter.sprite = CheckPointActiveSprite;
            activated = true;
        }
        else
        {
            spriter.sprite = CheckPointDeactiveSprite;
            activated = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<ActivateCheckpoint>().checkpointController = this;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<ActivateCheckpoint>().checkpointController = null;
        }
    }

    public void Activate()
    {
        spriter.sprite = CheckPointActiveSprite;
        SaveLoadManager.instance.Save<CheckpointData>(checkData, SaveLoadManager.instance.defaultSaveFolder, SaveLoadManager.instance.checkpointSaveFile);
        activated = true;
    }

    private void Update()
    {
        if (activated)
        {
            spriter.sprite = CheckPointActiveSprite;
        }
        else
        {
            spriter.sprite = CheckPointDeactiveSprite;
        }
    }

}
