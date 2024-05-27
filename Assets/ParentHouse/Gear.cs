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
        // public GearType GearType;
        // public string spriteSheetId;
        // public List<GearEffect> GearEffects { get; set; } = new();
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