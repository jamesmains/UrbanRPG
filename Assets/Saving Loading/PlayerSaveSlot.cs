using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Player Data Variable", menuName = "Variables/Player Data Variable")]
public class PlayerSaveSlot : ScriptableObject
{
    public string saveSlot;
    public string characterName;
    public string cityName;
    public LevelTransitionSignature NewSaveFileSpawnLocation;
    public LevelTransitionSignature NextLevelTransition;
    public VectorVariable PlayerPositionVariable;
    public bool Loaded;
    
    private void OnEnable()
    {
        Loaded = false;
    }

    public void LoadData(string slotName)
    {
        Debug.Log($"Slot Name : {slotName}");
        saveSlot = slotName;
        LoadData();
    }

    public void LoadData()
    {
        NextLevelTransition = ScriptableObject.CreateInstance<LevelTransitionSignature>();
        PlayerSaveData saveData = SaveLoad.LoadPlayerData();
        if (saveData == null)
        {
            Debug.Log("Save Data is Empty");
            NextLevelTransition = NewSaveFileSpawnLocation;
            return;
        }
        NextLevelTransition.TargetScene = saveData.SavedScene;
        characterName = saveData.SavedCharacterName;
        cityName = saveData.SavedCityName;
        NextLevelTransition.SpawnLocation = new Vector3(
            saveData.SavedSpawnLocationX,
            saveData.SavedSpawnLocationY,
            saveData.SavedSpawnLocationZ
        );
        Loaded = true;
    }
    
    [Button]
    public void SaveData()
    {
        SaveLoad.SavePlayerData(new PlayerSaveData(
            SceneManager.GetActiveScene().name,
            characterName,
            cityName,
            PlayerPositionVariable.Value));
    }

    public void SetValue(LevelTransitionSignature targetLevelSignature)
    {
        NextLevelTransition = targetLevelSignature;
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

    public PlayerSaveData(string sceneName, string characterName, string cityName, Vector3 location)
    {
        SavedScene = sceneName;
        SavedCharacterName = characterName;
        SavedCityName = cityName;
        SavedSpawnLocationX = location.x;
        SavedSpawnLocationY = location.y;
        SavedSpawnLocationZ = location.z;
    }
}
