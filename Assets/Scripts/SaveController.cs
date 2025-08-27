using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveController
{
    public static TowerData towerData;
    public static List<TowerData> leveleMateuszka = new List<TowerData>();
    private const string towerPath = "Tower/TowerData";
    public static string currentCampaign = "BaseCampaign";
    public static Levels levels = new Levels();


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
        var otherData = Resources.LoadAll<TextAsset>("Tower/BaseCampaign");

        foreach (var towerText in otherData)
        {
            leveleMateuszka.Add(JsonUtility.FromJson<TowerData>(towerText.ToString()));
        }
    }

    public static void SaveLevelStars()
    {
        var path = Application.persistentDataPath + "/" + currentCampaign + "Stars.json";

        string saveLevels = JsonUtility.ToJson(levels);
        File.WriteAllText(path, saveLevels);
        Debug.Log("Good");
    }

    public static void LoadLevelStars()
    {
        var path = Application.persistentDataPath + "/" + currentCampaign + "Stars.json";
        if (File.Exists(path))
        {
            Debug.Log(path);

            string loadedLevels = File.ReadAllText(path);
            var data = JsonUtility.FromJson<Levels>(loadedLevels);

            levels = data;

            Debug.Log(loadedLevels);
        }
    }

    public static void UpdateLevelStars(int level, int stars)
    {
        levels.stars[level] = stars;
        SaveLevelStars();
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
    public bool normal;
}

[Serializable]
public class Levels
{
    public int[] stars = new int[15];
}