using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Moddable Float", menuName = "Variables/Moddable Float")]
    public class ModdableFloat : ScriptableObject {
        public float DefaultValue;
        public List<float> ModValues = new();
        private float moddedValue;

        public float Value {
            get {
                var v = moddedValue;
                foreach (var modValue in ModValues) v += modValue;

                return v;
            }
            private set => moddedValue = value;
        }

        private void OnEnable() {
            // Load Move Speed
            moddedValue = DefaultValue;
        }

#if UNITY_EDITOR
        [Button]
        private void UpdateValue() {
            moddedValue = DefaultValue;
        }
#endif
    }
}