using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "String", menuName = "Variables/String")]
    public class StringVariable : ScriptableObject {
#if UNITY_EDITOR
        [Multiline] public string DeveloperDescription = "";
#endif
        public string Value;

        public void SetValue(string value) {
            Value = value;
        }
    }
}