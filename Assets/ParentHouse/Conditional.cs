using System;
using System.Collections.Generic;
using System.Linq;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    #region Generic

    [Serializable]
    public abstract class Condition {
        public abstract bool IsConditionMet();
        public abstract void Use();
    }

    public class AndCondition : Condition {
        [field: SerializeField] public List<Condition> Conditions { get; } = new();

        public override bool IsConditionMet() {
            return Conditions.TrueForAll(c => c.IsConditionMet());
        }

        public override void Use() {
            Conditions.ForEach(c => c.Use());
        }
    }

    public class OrCondition : Condition {
        [field: SerializeField] public List<Condition> Conditions { get; } = new();

        public override bool IsConditionMet() {
            return Conditions.Exists(c => c.IsConditionMet());
        }

        public override void Use() {
            Conditions.ForEach(c => c.Use());
        }
    }

    public class NotCondition : Condition {
        [field: SerializeField] public List<Condition> Conditions { get; } = new();

        public override bool IsConditionMet() {
            return !Conditions.Exists(c => c.IsConditionMet());
        }

        public override void Use() {
            Conditions.ForEach(c => c.Use());
        }
    }

    #endregion

    #region Calendar

    public abstract class CalendarCondition : Condition {
        public abstract bool IsConditionMet(int day, int month);

        public override bool IsConditionMet() {
            throw new NotImplementedException();
        }

        public override void Use() {
            throw new NotImplementedException();
        }
    }

    public class CalendarAndCondition : CalendarCondition {
        [field: SerializeField] public List<CalendarCondition> Conditions { get; } = new();

        public override bool IsConditionMet(int day, int month) {
            return Conditions.TrueForAll(c => c.IsConditionMet(day, month));
        }
    }

    public class CalendarOrCondition : CalendarCondition {
        [field: SerializeField] public List<CalendarCondition> Conditions { get; } = new();

        public override bool IsConditionMet(int day, int month) {
            return Conditions.Exists(c => c.IsConditionMet(day, month));
        }
    }

    public class CalendarNotCondition : CalendarCondition {
        [field: SerializeField] public List<CalendarCondition> Conditions { get; } = new();

        public override bool IsConditionMet(int day, int month) {
            return !Conditions.Exists(c => c.IsConditionMet(day, month));
        }
    }

    public class DayOfWeekCondition : CalendarCondition {
        [field: SerializeField] public Day TargetDay { get; }

        public override bool IsConditionMet(int day, int month) {
            day %= 7;
            return day == (int) TargetDay;
        }
    }

    public class SpecificDayCondition : CalendarCondition {
        [field: SerializeField]
        [field: Range(0, 27)]
        public int TargetDate { get; }

        public override bool IsConditionMet(int day, int month) {
            return day == TargetDate;
        }
    }

    public class SpecificMonthCondition : CalendarCondition {
        [field: SerializeField] public Month TargetMonth { get; }

        public override bool IsConditionMet(int day, int month) {
            return month == (int) TargetMonth;
        }
    }

    public class DateRangeCondition : CalendarCondition {
        [field: SerializeField] [field: MinMaxSlider(0, 27)]
        public Vector2Int Range;

        public override bool IsConditionMet(int day, int month) {
            return day >= Range.x && day <= Range.y;
        }
    }

    #endregion

    public class QuestStateConditional : Condition {
        [field: SerializeField] private Quest Quest { get; }
        [field: SerializeField] private QuestState State { get; }

        public override bool IsConditionMet() {
            return Quest.CurrentState == State;
        }

        public override void Use() {
        }
    }

    public class QuestStepConditional : Condition {
        [field: SerializeField] private Quest Quest { get; }
        [field: SerializeField] private QuestTask Step { get; }

        public override bool IsConditionMet() {
            return Quest.CurrentStep == Step;
        }

        public override void Use() {
        }
    }

    public class ItemConditional : Condition {
        [field: SerializeField] public Item Item { get; }

        [field: SerializeField] public int RequiredAmount { get; }

        [field: SerializeField]
        [field: Tooltip("This will use any item in the first slot")]
        public bool IsGift { get; }

        [field: SerializeField] public bool ConsumeOnUse { get; }

        [field: SerializeField] public Inventory Inventory { get; }

        public override bool IsConditionMet() {
            return Inventory.HasItem(Item, RequiredAmount) || (IsGift && Inventory.InventoryItems[0].Item != null);
        }

        public override void Use() {
            if (ConsumeOnUse) Inventory.TryUseItem(Item, RequiredAmount);
        }
    }

    public class QuestItemConditional : ItemConditional {
        [field: SerializeField] public Quest Quest { get; }

        [field: SerializeField] private QuestTask Step { get; }

        public override bool IsConditionMet() {
            return Inventory.HasItem(Item, RequiredAmount) ||
                   (IsGift && Inventory.HasItemAt(Item) != -1 && Quest.CurrentStep == Step);
        }

        public override void Use() {
            if (ConsumeOnUse) {
                var leftover = Inventory.TryUseItem(Item, RequiredAmount);
                Quest.TryCompleteTask(Step, RequiredAmount - leftover);
            }
        }
    }

    public class ReputationConditional : Condition {
        [field: SerializeField] public int RequiredReputation { get; }

        [field: SerializeField] public Actor TargetActor { get; }

        public override bool IsConditionMet() {
            return TargetActor.faction.currentReputation >= RequiredReputation;
        }

        public override void Use() {
        }
    }

    public class ReputationItemFavorConditional : ItemConditional {
        [field: SerializeField] public Actor TargetActor { get; }

        public override bool IsConditionMet() {
            Debug.Log("Checking");
            return IsGift && Inventory.InventoryItems[0].Item != null &&
                   TargetActor.faction.acceptedGifts.Any(i => i.giftItem == Inventory.InventoryItems[0].Item);
        }

        public override void Use() {
            var i = TargetActor.faction.acceptedGifts.FindIndex(i => i.giftItem == Inventory.InventoryItems[0].Item);
            TargetActor.AdjustReputation(TargetActor.faction.acceptedGifts[i].reputationChange);
            if (ConsumeOnUse) Inventory.TryUseItem(Inventory.InventoryItems[0].Item);
            GameEvents.OnEndActivity.Invoke();
        }
    }
}