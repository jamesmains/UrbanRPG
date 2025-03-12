using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gnomes.Interactions {
    public class TriggerListener : MonoBehaviour {
        [SerializeField, FoldoutGroup("Settings")]
        public Trigger Trigger;

        [SerializeField, FoldoutGroup("Settings")]
        private bool AutomaticallyGetEffects = true;

        [SerializeField, FoldoutGroup("Dependencies"), HideIf("AutomaticallyGetEffects")]
        [ValueDropdown(nameof(GetAllTriggerEffects), IsUniqueList = true)]
        public TriggerEffect[] BehaviorEffects;

        private IEnumerable<ValueDropdownItem> GetAllTriggerEffects()
        {
            return FindObjectsOfType<TriggerEffect>()
                .Select(effect => new ValueDropdownItem($"{effect.customName} ({effect.gameObject.name})", effect));
        }
    
        private void Awake() {
            if (AutomaticallyGetEffects)
                BehaviorEffects = GetComponentsInChildren<TriggerEffect>();
        }

        private void OnEnable() {
            Trigger.OnChangeState += Handle;
        }

        private void OnDisable() {
            Trigger.OnChangeState -= Handle;
        }

        private void Handle(bool state) {
            foreach (var target in BehaviorEffects) {
                target.HandleStateChange(state);
            }
        }
    }
}