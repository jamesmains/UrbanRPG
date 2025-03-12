using System;
using System.Collections.Generic;
using UnityEngine;

namespace Parent_House_Framework {
    [Serializable]

    public abstract class Condition {
        public abstract bool IsConditionMet();
    }

    public class AndCondition : Condition {
        [field: SerializeField] public List<Condition> Conditions { get; private set; } = new();

        public override bool IsConditionMet() {
            return Conditions.TrueForAll(c => c.IsConditionMet());
        }
    }

    public class OrCondition : Condition {
        [field: SerializeField] public List<Condition> Conditions { get; private set; } = new();

        public override bool IsConditionMet() {
            return Conditions.Exists(c => c.IsConditionMet());
        }
    }

    public class NotCondition : Condition {
        [field: SerializeField] public List<Condition> Conditions { get; private set; } = new();

        public override bool IsConditionMet() {
            return !Conditions.Exists(c => c.IsConditionMet());
        }
    }
}




