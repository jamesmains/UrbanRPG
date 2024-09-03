using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public static class SaveLoad {
    public static string PlayerSavePath(int slotId) => Application.persistentDataPath + $"Characters/{slotId}.xml";
    public static string PlayerInventoryPath(string uniqueId) => Application.persistentDataPath + $"Characters/Inventory/{uniqueId}.xml";
    
    public static void Save(SaveData saveData, string filePath) {
        try {
            if (!Directory.Exists(Path.GetDirectoryName(filePath))) {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            }
            XmlSerializer  serializer = new(typeof(SaveData));
            var stream = new FileStream(filePath, FileMode.Create);
            XmlTextWriter writer = new(stream, Encoding.Unicode);
            serializer.Serialize(writer, saveData);
            writer.Close();
            stream.Close();
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }

    public static SaveData Load(string filePath) {
        try {
            XmlSerializer  serializer = new(typeof(SaveData));
            var stream = new FileStream(filePath, FileMode.Open);
            var data = serializer.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        catch (Exception e) {
            Debug.LogError(e);
            return null;
        }
    }
    
    public static void DeleteFile(string filePath) {
        if (!HasPath(filePath)) return;
        try {
            File.Delete(filePath);
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }

    public static bool HasPath(string path) {
        return File.Exists(path);
    }
}
[XmlInclude(typeof(PlayerData))][XmlInclude(typeof(InventorySaveData))]
public abstract class SaveData {
}
