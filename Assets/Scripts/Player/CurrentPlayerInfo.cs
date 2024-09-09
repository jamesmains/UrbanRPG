using System;
using UnityEngine;

public static class CurrentPlayerInfo {
    public static PlayerData Data;

    public static void SavePlayer() {
        if (Data == null) return;
        SaveLoad.Save(Data,SaveLoad.PlayerSavePath(Data.SlotId));
    }

    public static void LoadPlayer(int slotId) {
        Data = (PlayerData)SaveLoad.Load(SaveLoad.PlayerSavePath(slotId));
    }
}

[Serializable]
public class PlayerData : SaveData {
    public int SlotId;
    public string Room = "Room 0";
    public Vector2 Position;
    public string UniqueId;
    public string PlayerName;
}
