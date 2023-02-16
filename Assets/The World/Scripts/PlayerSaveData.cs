using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Player Data Variable", menuName = "Variables/Player Data Variable")]
public class PlayerSaveData : ScriptableObject
{
    public LevelTransitionSignature LoadSpawn;
    public LevelTransitionSignature NextLevelTransition;
    public bool Loaded;
    
    private void OnEnable()
    {
        Loaded = false;
    }

    public void LoadLocation()
    {
        LoadSpawn = ScriptableObject.CreateInstance<LevelTransitionSignature>();
        PlayerSpawnData spawnData = SaveLoad.LoadPlayerLocation();
        if (spawnData == null)
        {
            LoadSpawn = null;
            NextLevelTransition = null;
            return;
        }
        LoadSpawn.TargetScene = spawnData.SavedScene;
        LoadSpawn.SpawnLocation = new Vector3(
            spawnData.SavedSpawnLocationX,
            spawnData.SavedSpawnLocationY,
            spawnData.SavedSpawnLocationZ
        );
        NextLevelTransition = LoadSpawn;
        Loaded = true;
    }
    
    public void SaveLocation()
    {
        SaveLoad.SavePlayerLocation(new PlayerSpawnData(SceneManager.GetActiveScene().name,PlayerMotor.playerLocation));
    }

    public void SetValue(LevelTransitionSignature targetLevelSignature)
    {
        NextLevelTransition = targetLevelSignature;
    }
}

[Serializable]
public class PlayerSpawnData
{
    public String SavedScene;
    public float SavedSpawnLocationX;
    public float SavedSpawnLocationY;
    public float SavedSpawnLocationZ;

    public PlayerSpawnData(string sceneName, Vector3 location)
    {
        SavedScene = sceneName;
        SavedSpawnLocationX = location.x;
        SavedSpawnLocationY = location.y;
        SavedSpawnLocationZ = location.z;
    }
}
