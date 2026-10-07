using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;
    public string defaultSaveFolder = "SavedData";
    [Header("Spawn Save Settings")]
    public string spawnSaveFolder = "SavedData";
    public string spawnSaveFile = "SpawnData.json";
    [Header("Checkpoint Save Settings")]
    public string checkpointSaveFile = "Checkpoint.json";
    [Header("Minimap Save Settings")]
    public string minimapSaveFile = "Minimap.json";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save<T>(T dataToSave, string saveFolder, string saveFileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath, saveFolder, saveFileName);
        Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        File.WriteAllText(savePath, JsonUtility.ToJson(dataToSave));
    }

    public T Load<T>(string loadFolder, string loadFileName)
    {
        string loadPath = Path.Combine(Application.persistentDataPath, loadFolder, loadFileName);
        T loadData = default;
        if (File.Exists(loadPath))
        {
            string result = File.ReadAllText(loadPath);
            loadData = JsonUtility.FromJson<T>(result);
        }
        return (loadData != null) ? loadData : default;
    }

    public void DeleteFolder(string folderName)
    {
        string path = Path.Combine(Application.persistentDataPath, folderName);
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }
    public void DeleteFile(string folderName, string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
