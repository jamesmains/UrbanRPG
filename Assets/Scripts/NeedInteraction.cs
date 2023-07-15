using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class NeedInteraction : MonoBehaviour
{
    [SerializeField] private Need targetNeed;
    [FoldoutGroup("Data")][SerializeField] private float modValue;
    [FoldoutGroup("Data")][SerializeField] private int interactionUses;

    public void TriggerNeedInteraction()
    {
        targetNeed.Value += modValue;
        interactionUses--;
        if(interactionUses <= 0)
            Destroy(this.gameObject);
    }
}
