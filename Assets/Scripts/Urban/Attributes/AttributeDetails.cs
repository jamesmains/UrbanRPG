using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Urban.Attributes {
    [CreateAssetMenu(fileName = "New Attribute", menuName = "Attribute")]
    public class AttributeDetails : ScriptableObject {
        [SerializeField,BoxGroup("Settings")]
        public string Name;
        [SerializeField,BoxGroup("Settings")]
        public string Description;
        [SerializeField,BoxGroup("Settings")]
        public Sprite Icon;

        [SerializeField, BoxGroup("Status"), ReadOnly]
        private Guid Id;

        [Button]
        private void CreateNewId() => Id = Guid.NewGuid();

        private void Awake() {
            if(Id == Guid.Empty) {CreateNewId();}
        }

        public bool HasSameId(Guid id) => Id == id;
    }
}
