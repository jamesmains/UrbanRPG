using ParentHouse.Utils;
using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Effect", menuName = "Items/Effect")]
    public class Effect : Item
    {
        public EffectType effectType;
        public int hospitalCostToCure = 0;
    }
}
