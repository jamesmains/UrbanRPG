using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    public class NPC : MonoBehaviour {
        [SerializeField] private Actor actor;

        public string TestValue;
        public Vector3 TestVector;

        [Button]
        private void DebugSave() {
            var data = new TestData();
            data.TestString = TestValue;
            data.TestVector = TestVector;
            SaveLoad.SaveToJson(data, SaveLoad.DebugSaveDataFilePath());
        }

        [Button]
        private void DebugLoad() {
            var data = SaveLoad.LoadFromJson<TestData>(SaveLoad.DebugSaveDataFilePath());
            TestValue = data.TestString;
            TestVector = data.TestVector;
        }
    }
}