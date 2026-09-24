using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class SaveController
{
    public static TowerData towerData;
    public static List<TowerData> leveleMateuszka = new List<TowerData>();
    private const string towerPath = "Tower/TowerData";
    public static string currentCampaign = "BaseCampaign";
    public static Levels levels = new Levels();
    private static string levelFilePath;
    private static string uploadUrl = "https://rotatrixlevelupload.onrender.com/api/File";
    public static IEnumerator UploadFile()
    {
        // Check if file exists
        if (!File.Exists(levelFilePath))
        {
            Debug.LogError("File does not exist: " + levelFilePath);
            yield break;
        }

        // Read file bytes
        byte[] fileData = File.ReadAllBytes(levelFilePath);
        string fileName = Path.GetFileName(levelFilePath);

        // Create form data
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>
        {
            new MultipartFormFileSection("file", fileData, fileName, "application/json")
        };

        // Create the request
        using (UnityWebRequest www = UnityWebRequest.Post(uploadUrl, formData))
        {
            // Set timeout (optional)
            www.timeout = 30;

            // Send the request
            yield return www.SendWebRequest();

            // Check for errors
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("File uploaded successfully!");
                Debug.Log("Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Upload failed: " + www.error);
                Debug.LogError("Response Code: " + www.responseCode);
                Debug.LogError("Response: " + www.downloadHandler.text);
            }
        }
    }
    public static IEnumerator UploadAllLevels()
    {

        string[] fileEntries = Directory.GetFiles(Application.persistentDataPath + "/SavedLevels/");
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        foreach (string file in fileEntries)
        {

            // Check if file exists
            if (!File.Exists(file))
            {
                Debug.LogError("File does not exist: " + file);
                yield break;
            }

            // Read file bytes
            byte[] fileData = File.ReadAllBytes(file);
            string fileName = Path.GetFileName(file);

            // Create form data
            formData.Add(new MultipartFormFileSection("file", fileData, fileName, "application/json"));
        }
        // Create the request
        using (UnityWebRequest www = UnityWebRequest.Post(uploadUrl, formData))
        {
            // Set timeout (optional)
            www.timeout = 30;

            // Send the request
            yield return www.SendWebRequest();

            // Check for errors
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("File uploaded successfully!");
                Debug.Log("Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Upload failed: " + www.error);
                Debug.LogError("Response Code: " + www.responseCode);
                Debug.LogError("Response: " + www.downloadHandler.text);
            }
        }
    }
    public static void UploadAllLeveles()
    {
        string[] fileEntries = Directory.GetFiles(Application.persistentDataPath + "/SavedLevels/");
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        foreach (string file in fileEntries)
        {

            // Check if file exists
            if (!File.Exists(file))
            {
                Debug.LogError("File does not exist: " + file);
                return;
            }

            // Read file bytes
            byte[] fileData = File.ReadAllBytes(file);
            string fileName = Path.GetFileName(file);

            // Create form data
            formData.Add(new MultipartFormFileSection("file", fileData, fileName, "application/json"));
        }
        // Create the request
        using (UnityWebRequest www = UnityWebRequest.Post(uploadUrl, formData))
        {
            // Set timeout (optional)
            www.timeout = 30;
            www.SendWebRequest();
            // Send the request
            //yield return www.SendWebRequest();

            // Check for errors
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("File uploaded successfully!");
                Debug.Log("Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Upload failed: " + www.error);
                Debug.LogError("Response Code: " + www.responseCode);
                Debug.LogError("Response: " + www.downloadHandler.text);
            }
        }

    }
    public static void SaveCreatedLevel(string name, CreatorController controller)
    {
        var path = Application.persistentDataPath + "/SavedLevels/" + name + ".json";
        var achievements = new TowerData();


        achievements.transforms = controller.transforms;
        achievements.colors = controller.materials;

        string saveLevels = JsonUtility.ToJson(achievements);
        Directory.CreateDirectory(Application.persistentDataPath + "/SavedLevels/");
        File.WriteAllText(path, saveLevels);
        Debug.Log(path);
        levelFilePath = path;
    }

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
        var otherData = Resources.LoadAll<TextAsset>("Tower/" + currentCampaign);

        leveleMateuszka = new List<TowerData>();

        foreach (var towerText in otherData)
        {
            Debug.Log(towerText.ToString());
            leveleMateuszka.Add(JsonUtility.FromJson<TowerData>(towerText.ToString()));
            Debug.Log(leveleMateuszka.Count.ToString());
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
        else
        {
            levels = new Levels();
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