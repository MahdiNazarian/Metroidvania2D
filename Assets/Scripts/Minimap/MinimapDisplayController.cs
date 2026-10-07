using UnityEngine;

public class MinimapDisplayController : MonoBehaviour
{
    [SerializeField] private string firstMinimapToLoad;
    private MinimapID[] minimapIds;
    [HideInInspector] public MinimapData minimapData = new MinimapData();

    void Start()
    {
        minimapIds = GetComponentsInChildren<MinimapID>(true);
        LoadMinimapData();
        DisplayUnlockedMinimaps();
    }
    public void LoadMinimapData()
    {
        minimapData = SaveLoadManager.instance.Load<MinimapData>(SaveLoadManager.instance.defaultSaveFolder, SaveLoadManager.instance.minimapSaveFile);
        if(minimapData is null)
        {
            minimapData = new MinimapData();
        }
    }
    public void DisplayUnlockedMinimaps()
    {
        if(minimapData.minimapKeys.Count <= 0)
        {
            foreach(MinimapID mapId in minimapIds)
            {
                if(mapId.minimapKey == firstMinimapToLoad)
                {
                    mapId.gameObject.SetActive(true);
                    minimapData.AddToListWithCheck(mapId.minimapKey);
                    break;
                }
            }
        }
        else
        {
            foreach(string key in minimapData.minimapKeys)
            {
                foreach(MinimapID mapId in minimapIds)
                {
                    if(mapId.minimapKey == key)
                    {
                        mapId.gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
    }
    private void OnDisable()
    {
        SaveLoadManager.instance.Save<MinimapData>(minimapData, SaveLoadManager.instance.defaultSaveFolder, SaveLoadManager.instance.minimapSaveFile);
    }
}
