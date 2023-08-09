using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Items/Effect")]
public class Effect : Item
{
    public EffectType effectType;
    public int hospitalCostToCure = 0;
}
