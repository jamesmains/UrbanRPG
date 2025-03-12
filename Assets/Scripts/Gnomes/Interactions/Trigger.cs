using System;
using System.Collections;
using Gnomes.Actor.Extension.Interactions;
using Sirenix.OdinInspector;
using UnityEngine;

// Todo: Need callback for when interaction is done to optionally reset Trigger
// Todo: Need dynamic bools of some kind to serialize their state in between game sessions

namespace Gnomes.Interactions {
    public class Trigger : MonoBehaviour, IInteractable {
        [SerializeField, FoldoutGroup("Settings")]
        private InteractionSettings Settings;

        [SerializeField] [FoldoutGroup("Status"), ReadOnly]
        private bool m_Activated;

        public bool Activated {
            get => m_Activated;
            private set {
                m_Activated = value;
                OnChangeState?.Invoke(m_Activated);
            }
        }

        [SerializeField] [FoldoutGroup("Events")]
        public Action<bool> OnChangeState;

        private void OnEnable() {
            StartCoroutine(Delay());
            IEnumerator Delay() {
                yield return new WaitForEndOfFrame();
                Activated = Settings.ActiveOnEnable;
            }
        
        }

    

        public void Notify(NotifyState state) {
            if (RequireButtonToChangeState()) return;
            ChangeState();
        }

        public bool RequireButtonToChangeState() {
            return Settings.RequireKeyToActivate;
        }

        public void ChangeState() {
            if (Settings.Toggles) {
                Activated = !Activated;
            }
            else {
                if (Activated)
                    return;
                Activated = true;
            }
        }
    
        public void SetState(bool state) {
            if(Activated != state)
                Activated = state;
        }
    }
}