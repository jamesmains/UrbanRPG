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
        private static PlayerSaveSlot _playerSaveSlot;
        
        public static void SaveInventory(InventorySaveData saveData)
        {
            if (_playerSaveSlot == null)
            {
                LoadSaveSlot();
            }
            
            var binaryFormatter = new BinaryFormatter();
            string directory = $"{Application.persistentDataPath}/{_playerSaveSlot.saveSlot}/Inventories/";
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
            if (_playerSaveSlot == null)
            {
                LoadSaveSlot();
            }

            string filePath = $"{Application.persistentDataPath}/{_playerSaveSlot.saveSlot}/Inventories/{inventoryName}.inv";
            
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
            if (_playerSaveSlot == null)
            {
                LoadSaveSlot();
            }
            
            var binaryFormatter = new BinaryFormatter();
            string directory = $"{Application.persistentDataPath}/{_playerSaveSlot.saveSlot}/Quests/";
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
            if (_playerSaveSlot == null)
            {
                LoadSaveSlot();
            }
            string filePath = $"{Application.persistentDataPath}/{_playerSaveSlot.saveSlot}/Quests/{questName}.quest";

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

        public static void SavePlayerData(PlayerSaveData saveData)
        {
            if (_playerSaveSlot == null)
            {
                LoadSaveSlot();
            }
            
            var binaryFormatter = new BinaryFormatter();
            string directory = $"{Application.persistentDataPath}/{_playerSaveSlot.saveSlot}";
            string filePath = $"{directory}/player.location";
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            var fileStream = File.Create(filePath);

            binaryFormatter.Serialize(fileStream, saveData);
            fileStream.Close();
        }

        public static PlayerSaveData LoadPlayerData()
        {
            if (_playerSaveSlot == null)
            {
                LoadSaveSlot();
            }
            Debug.Log(_playerSaveSlot);
            string filePath = $"{Application.persistentDataPath}/{_playerSaveSlot.saveSlot}/player.location";
    
            if (!File.Exists(filePath))
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Open(filePath, FileMode.Open);
            var saveData = (PlayerSaveData) binaryFormatter.Deserialize(fileStream);

            fileStream.Close();
            return saveData;
        }
        
        private static void LoadSaveSlot()
        {
            _playerSaveSlot = Resources.Load("Player Data Variable") as PlayerSaveSlot;
        }
    }
}