using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Gear", menuName = "Items/Gear")]
    public class Gear : Item {
        [field: SerializeField] [field: PropertyOrder(60)] [field: Space(10)]
        public GearType GearType;

        public string spriteSheetId;

        [field: SerializeField]
        [field: PropertyOrder(80)]
        [field: Space(10)]
        public List<GearEffect> GearEffects { get; set; } = new();

#if UNITY_EDITOR
        [Button]
        public void SetTempName() {
            Name = spriteSheetId.Split("\\").Last();
        }

        [Button]
        public void SetID() {
            var relativePath =
                Path.GetDirectoryName(Application.dataPath + "/" +
                                      AssetDatabase.GetAssetPath(this).Split("Assets/")[1]);
            var relativeDir = Directory.GetFiles(relativePath).ToList();
            var targetString = relativeDir.FindAll(o => o.Contains(".png") && !o.Contains(".meta"));
            if (targetString[0].Contains(".png"))
                targetString[0] = targetString[0].Split("Resources\\")[1].Replace(".png", "");
            spriteSheetId = targetString[0];
        }
#endif
    }

    [Serializable]
    public abstract class GearEffect {
        public string effectText;
        public abstract void OnEquip();
        public abstract float GetEffectValue();
    }

    public class RideEffect : GearEffect {
        [SerializeField] private float modValue = new();

        public override void OnEquip() {
            GameEvents.OnChangeRide.Invoke();
        }

        public override float GetEffectValue() {
            return modValue;
        }
    }

    [Serializable]
    public class GearOption {
        public Gear gear;
        public GearType type;

        public GearOption(Gear g, GearType gearType) {
            gear = g;
            type = gearType;
        }
    }
}