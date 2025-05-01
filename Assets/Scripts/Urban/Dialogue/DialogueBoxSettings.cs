using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Urban.Dialogue {
    [Serializable]
    [CreateAssetMenu(fileName = "New Dialogue Box Settings", menuName = "Dialogue/Dialogue Box Settings")]
    public class DialogueBoxSettings : ScriptableObject {
        [SerializeField, FoldoutGroup("Settings")]
        public TMP_FontAsset Font;

        [SerializeField, FoldoutGroup("Settings")]
        public float FontSize = 18f;

        [SerializeField, FoldoutGroup("Settings")]
        public Sprite Background;

        [SerializeField, FoldoutGroup("Settings")]
        public Sprite Nameplate;

        [SerializeField, FoldoutGroup("Settings")]
        public float TypeSpeed;
    }
}