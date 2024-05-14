using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Player Data Variable", menuName = "Variables/Player Data Variable")]
    public class PlayerSaveSlot : Actor {
        public string saveSlot;
        public string cityName;
        public SceneTransition NewSaveFileSpawnLocation;
        public SceneTransition NextSceneTransition;
        public Vector2 PlayerPositionVariable;
        public int PlayerMoneyVariable;
        public bool Loaded;

        private void OnEnable() {
            Loaded = false;
        }

        public void LoadData(string slotName) {
            saveSlot = slotName;
            LoadData();
        }

        // Need to restructure using Json
        // Vectors just work!
        public void LoadData() {
            // NextSceneTransition = ScriptableObject.CreateInstance<SceneTransition>();
            // PlayerSaveData saveData = SaveLoad.LoadPlayerData();
            // if (saveData == null)
            // {
            //     Debug.Log($"Failed to load save slot [{saveSlot}] because it does not exist (PlayerSaveSlot.cs)");
            //     NextSceneTransition = NewSaveFileSpawnLocation;
            //     return;
            // }
            // NextSceneTransition.TargetScene = saveData.SavedScene;
            // actorName = saveData.SavedCharacterName;
            // cityName = saveData.SavedCityName;
            // NextSceneTransition.SpawnLocation = new Vector3(
            //     saveData.SavedSpawnLocationX,
            //     saveData.SavedSpawnLocationY,
            //     saveData.SavedSpawnLocationZ
            // );
            // PlayerMoneyVariable = saveData.SavedMoney;
            // Loaded = true;
        }

        [Button]
        public void SaveData() {
            // SaveLoad.SavePlayerData(new PlayerSaveData(
            //     SceneManager.GetActiveScene().name,
            //     actorName,
            //     cityName,
            //     PlayerPositionVariable,
            //     PlayerMoneyVariable));
        }
    }

    [Serializable]
    public class PlayerSaveData {
        public string SavedScene;
        public string SavedCharacterName;
        public string SavedCityName;
        public float SavedSpawnLocationX;
        public float SavedSpawnLocationY;
        public float SavedSpawnLocationZ;
        public int SavedMoney;

        public PlayerSaveData(string sceneName, string characterName, string cityName, Vector3 location, int money) {
            SavedScene = sceneName;
            SavedCharacterName = characterName;
            SavedCityName = cityName;
            SavedSpawnLocationX = location.x;
            SavedSpawnLocationY = location.y;
            SavedSpawnLocationZ = location.z;
            SavedMoney = money;
        }
    }
}