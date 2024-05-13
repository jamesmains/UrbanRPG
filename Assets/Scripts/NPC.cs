using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private Actor actor;

    public string TestValue;
    public Vector3 TestVector;

    [Button]
    private void DebugSave() {
        var data = new TestData();
        data.TestString = TestValue;
        data.TestVector = TestVector;
        SaveLoad.Save(data,SaveLoad.DebugSaveDataFilePath());
    }

    [Button]
    private void DebugLoad() {
        var data = SaveLoad.Load<TestData>(SaveLoad.DebugSaveDataFilePath());
        TestValue = data.TestString;
        TestVector = data.TestVector;
    }
}
