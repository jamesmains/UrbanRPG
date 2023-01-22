using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedInteraction : MonoBehaviour
{
    [SerializeField] private Need targetNeed;
    [SerializeField] private float modValue;
    [SerializeField] private int interactionUses;

    public void TriggerNeedInteraction()
    {
        targetNeed.Value += modValue;
        interactionUses--;
        if(interactionUses <= 0)
            Destroy(this.gameObject);
    }
}
