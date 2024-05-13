using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Time", menuName = "Variables/Time")]
    public class TimeVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public float Value;

#if UNITY_EDITOR
        public bool ShowChain;
#endif


        [ShowIf("ShowChain")]
        public TimeVariable ChainVariable;

        [ShowIf("ShowChain")]
        public float MaxValue;
    
        public void SetValue(float value)
        {
            Value = value;
            if (Value >= MaxValue && ChainVariable != null)
            {
                Value = 0;
                ChainVariable.ApplyChange(1);
            }
            GameEvents.OnChangeTime.Invoke();
        }

        [Button]
        public void ApplyChange(float amount)
        {
            Value += amount;
            if (Value >= MaxValue && ChainVariable != null)
            {
                Value = 0;
                ChainVariable.ApplyChange(1);
            }
            GameEvents.OnChangeTime.Invoke();
        }
    }
}
