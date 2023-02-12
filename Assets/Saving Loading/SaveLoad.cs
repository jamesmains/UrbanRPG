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
        private static StringVariable saveSlot;
        
        public static void SaveInventory(InventorySaveData saveData)
        {
            if (saveSlot == null)
            {
                LoadSaveSlot();
            }
            
            var binaryFormatter = new BinaryFormatter();
            string directory = $"{Application.persistentDataPath}/{saveSlot.Value}/Inventories/";
            string filePath = $"{directory}{saveData.Name}.inv";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            var fileStream = File.Create(filePath);

            binaryFormatter.Serialize(fileStream, saveData);
            fileStream.Close();
        }
        
        public static InventorySaveData LoadInventory(string inventoryName)
        {
            if (saveSlot == null)
            {
                LoadSaveSlot();
            }

            string filePath = $"{Application.persistentDataPath}/{saveSlot.Value}/Inventories/{inventoryName}.inv";
            
            if (!File.Exists(filePath))
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Open(filePath, FileMode.Open);
            var saveData = (InventorySaveData) binaryFormatter.Deserialize(fileStream);

            fileStream.Close();
            return saveData;
        }

        public static void SaveQuest(QuestSaveData saveData)
        {
            if (saveSlot == null)
            {
                LoadSaveSlot();
            }
            
            var binaryFormatter = new BinaryFormatter();
            string directory = $"{Application.persistentDataPath}/{saveSlot.Value}/Quests/";
            string filePath = $"{directory}{saveData.Name}.quest";
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            var fileStream = File.Create(filePath);

            binaryFormatter.Serialize(fileStream, saveData);
            fileStream.Close();
        }

        public static QuestSaveData LoadQuest(string questName)
        {
            if (saveSlot == null)
            {
                LoadSaveSlot();
            }

            string filePath = $"{Application.persistentDataPath}/{saveSlot.Value}/Quests/{questName}.quest";

            if (!File.Exists(filePath))
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Open(filePath, FileMode.Open);
            var saveData = (QuestSaveData) binaryFormatter.Deserialize(fileStream);

            fileStream.Close();
            return saveData;
        }

        public static void SavePlayerLocation(PlayerSpawnData saveData)
        {
            if (saveSlot == null)
            {
                LoadSaveSlot();
            }
            
            var binaryFormatter = new BinaryFormatter();
            string directory = $"{Application.persistentDataPath}/{saveSlot.Value}";
            string filePath = $"{directory}/loc.ation";
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            var fileStream = File.Create(filePath);

            binaryFormatter.Serialize(fileStream, saveData);
            fileStream.Close();
        }

        public static PlayerSpawnData LoadPlayerLocation()
        {
            if (saveSlot == null)
            {
                LoadSaveSlot();
            }

            string filePath = $"{Application.persistentDataPath}/{saveSlot.Value}/loc.ation";

            if (!File.Exists(filePath))
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Open(filePath, FileMode.Open);
            var saveData = (PlayerSpawnData) binaryFormatter.Deserialize(fileStream);

            fileStream.Close();
            return saveData;
        }
        
        private static void LoadSaveSlot()
        {
            saveSlot = Resources.Load("CurrentSaveSlot") as StringVariable;
        }
    }
}