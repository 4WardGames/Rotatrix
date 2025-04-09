using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveController
{
    public static TowerData towerData;
    private const string towerPath = "Tower/TowerData";
    public static void SaveTower()
    {
        var path = Application.persistentDataPath + "/Tower.json";
        var achievements = new TowerData();


        achievements.transforms = new List<BlockTransformation>
        {
            new BlockTransformation { splitPoint = 1 , rotate = false}
        };

        achievements.colors = new List<int> { 1, 2, 0, 1, 2, 3 };

        string saveTower = JsonUtility.ToJson(achievements);
        File.WriteAllText(path, saveTower);
        Debug.Log("Good");
    }

    public static void LoadLevel()
    {
        var otherData = Resources.Load(towerPath) as TextAsset;

        string tower = otherData.text;
        towerData = JsonUtility.FromJson<TowerData>(tower);
    }

    public static void LoadTower()
    {
        var path = Application.persistentDataPath + "/Tower.json";
        if (File.Exists(path))
        {
            Debug.Log(path);

            string tower = File.ReadAllText(path);
            towerData = JsonUtility.FromJson<TowerData>(tower);
            Debug.Log(tower);
        }
        else
        {
            Debug.Log("bad");
        }
    }

}

[Serializable]
public class TowerData
{
    public List<int> colors;

    public List<BlockTransformation> transforms;
}

[Serializable]
public class BlockTransformation
{
    public int splitPoint;
    public bool rotate;
}