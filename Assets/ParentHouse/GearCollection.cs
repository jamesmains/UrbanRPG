using System.Collections.Generic;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Gear Collection", menuName = "Collections/GearCollection")]
    public class GearCollection : ScriptableObject {
        public string header;
        public Sprite icon;
        public GearType type;
        public bool allowNull;
        public List<Gear> gear = new();
    }
}