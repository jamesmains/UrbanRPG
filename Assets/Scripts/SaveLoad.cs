using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveLoad {

    public static string DebugSaveDataFilePath() => $"{Application.persistentDataPath}/Test.json";
    
    public static void SaveToJson<T>(T data, string filePath) {
        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(filePath, json);
    }

    public static T LoadFromJson<T>(string filePath) {
        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<T>(json);
    }
}

public abstract class SaveData {
    
}

public class TestData : SaveData {
    public string TestString;
    public Vector3 TestVector;
}