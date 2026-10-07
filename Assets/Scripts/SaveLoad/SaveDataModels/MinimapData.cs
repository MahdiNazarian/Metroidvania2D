using System.Collections.Generic;

[System.Serializable]
public class MinimapData
{
    public List<string> minimapKeys;

    public void AddToListWithCheck(string key)
    {
        if (minimapKeys.Contains(key))
        {
            return;
        }

        minimapKeys.Add(key);
    }
}