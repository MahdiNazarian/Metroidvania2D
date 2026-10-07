[System.Serializable]
public class CheckpointData
{
    public CheckpointData()
    {
        SceneToLoad = "Level 1";
        CheckpointKey = "Check 1";
        facingRight = true;
    }
    public string SceneToLoad;
    public string CheckpointKey;
    public bool facingRight;
}
