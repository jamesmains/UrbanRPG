using System.Collections.Generic;
using Parent_House_Framework.Cores;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gnomes.Interactions {
    public class TriggerEffect : SerializedMonoBehaviour
    {
        [Title("Effect Name")]
        public string customName = "Default Name";
    
        [SerializeField, FoldoutGroup("Settings")]
        public readonly List<ObjectEffect> Behaviors = new();

        private void Awake() {
            Initialize();
        }
    
#if UNITY_EDITOR
        private void OnValidate() {
            Initialize();
        }
#endif

        private void Initialize() {
            foreach (var behavior in Behaviors) {
                behavior.Initialize(this.gameObject);
            }
        }

        public void HandleStateChange(bool state) {
            foreach (var behavior in Behaviors) {
                behavior.HandleState(state);
            }
        }
    }
}
