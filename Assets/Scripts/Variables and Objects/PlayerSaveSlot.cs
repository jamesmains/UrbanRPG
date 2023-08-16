using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Player Data Variable", menuName = "Variables/Player Data Variable")]
public class PlayerSaveSlot : Actor
{
    public string saveSlot;
    public string cityName;
    public SceneTransition NewSaveFileSpawnLocation;
    public SceneTransition NextSceneTransition;
    public VectorVariable PlayerPositionVariable;
    public IntVariable PlayerMoneyVariable;
    public bool Loaded;
    
    private void OnEnable()
    {
        Loaded = false;
    }

    public void LoadData(string slotName)
    {
        if(UrbanDebugger.DebugLevel>=1)
        {
            Debug.Log($"Trying to load save slot [{slotName}] (PlayerSaveSlot.cs)");
        }
        saveSlot = slotName;
        LoadData();
    }

    public void LoadData()
    {
        NextSceneTransition = ScriptableObject.CreateInstance<SceneTransition>();
        PlayerSaveData saveData = SaveLoad.LoadPlayerData();
        if (saveData == null)
        {
            if(UrbanDebugger.DebugLevel>=1)
            {
                Debug.Log($"Failed to load save slot [{saveSlot}] because it does not exist (PlayerSaveSlot.cs)");
            }
            NextSceneTransition = NewSaveFileSpawnLocation;
            return;
        }
        NextSceneTransition.TargetScene = saveData.SavedScene;
        actorName = saveData.SavedCharacterName;
        cityName = saveData.SavedCityName;
        NextSceneTransition.SpawnLocation = new Vector3(
            saveData.SavedSpawnLocationX,
            saveData.SavedSpawnLocationY,
            saveData.SavedSpawnLocationZ
        );
        PlayerMoneyVariable.Value = saveData.SavedMoney;
        Loaded = true;
    }
    
    [Button] public void SaveData()
    {
        SaveLoad.SavePlayerData(new PlayerSaveData(
            SceneManager.GetActiveScene().name,
            actorName,
            cityName,
            PlayerPositionVariable.Value,
            PlayerMoneyVariable.Value));
    }
}

[Serializable]
public class PlayerSaveData
{
    public String SavedScene;
    public String SavedCharacterName;
    public String SavedCityName;
    public float SavedSpawnLocationX;
    public float SavedSpawnLocationY;
    public float SavedSpawnLocationZ;
    public int SavedMoney;

    public PlayerSaveData(string sceneName, string characterName, string cityName, Vector3 location, int money)
    {
        SavedScene = sceneName;
        SavedCharacterName = characterName;
        SavedCityName = cityName;
        SavedSpawnLocationX = location.x;
        SavedSpawnLocationY = location.y;
        SavedSpawnLocationZ = location.z;
        SavedMoney = money;
    }
}
