using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    public class NeedInteraction : MonoBehaviour {
        [SerializeField] private Need targetNeed;

        [FoldoutGroup("Data")] [SerializeField]
        private float modValue;

        [FoldoutGroup("Data")] [SerializeField]
        private int interactionUses;

        public void TriggerNeedInteraction() {
            targetNeed.Value += modValue;
            interactionUses--;
            if (interactionUses <= 0)
                Destroy(gameObject);
        }
    }
}