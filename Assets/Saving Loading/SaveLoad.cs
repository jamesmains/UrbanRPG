using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace I302.Manu
{
    public static class SaveLoad
    {
        [SerializeField] private static StringVariable saveSlot;
        
        public static void SaveLastKnownTime(DateTime lastKnownTime)
        {
            string filePath = Application.persistentDataPath + $"/TempTimerName_Progress.save";
            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Create(filePath);
            binaryFormatter.Serialize(fileStream, lastKnownTime);
            fileStream.Close();
        }

        public static DateTime LoadLastKnownTime()
        {
            DateTime time = new DateTime();
            string filePath = Application.persistentDataPath + $"/TempTimerName_Progress.save";
            if (!File.Exists(filePath))
            {
                return time;
            }
            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Open(filePath,FileMode.Open);
            time = (DateTime) binaryFormatter.Deserialize(fileStream);

            fileStream.Close();
            return time;

        }

        public static void SaveItem(ItemSaveData saveData)
        {
            var binaryFormatter = new BinaryFormatter();
            if (!Directory.Exists(Application.persistentDataPath + "/Items"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Items");
            }
            
            var fileStream = File.Create(Application.persistentDataPath + $"/Items/{saveData.Name}.save");

            binaryFormatter.Serialize(fileStream, saveData);
            fileStream.Close();
        }

        public static ItemSaveData LoadItem(string itemName)
        {
            if (!File.Exists(Application.persistentDataPath + $"/Items/{itemName}.save"))
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Open(Application.persistentDataPath + $"/Items/{itemName}.save", FileMode.Open);
            var saveData = (ItemSaveData) binaryFormatter.Deserialize(fileStream);

            fileStream.Close();
            return saveData;
        }
        
        public static void SaveInventory(InventorySaveData saveData)
        {
            var binaryFormatter = new BinaryFormatter();
            if (!Directory.Exists(Application.persistentDataPath + "/Inventories"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Inventories");
            }
            
            var fileStream = File.Create(Application.persistentDataPath + $"/Inventories/{saveData.Name}.save");

            binaryFormatter.Serialize(fileStream, saveData);
            fileStream.Close();
        }
        
        public static InventorySaveData LoadInventory(string inventoryName)
        {
            if (!File.Exists(Application.persistentDataPath + $"/Inventories/{inventoryName}.save"))
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Open(Application.persistentDataPath + $"/Inventories/{inventoryName}.save", FileMode.Open);
            var saveData = (InventorySaveData) binaryFormatter.Deserialize(fileStream);

            fileStream.Close();
            return saveData;
        }
    }
}