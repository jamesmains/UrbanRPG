using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityFarming : Activity
{
    private void Awake()
    {
        foreach (var Action in ActivityActions)
        {
            if (Action.signature.actionType == ActionType.Harvest)
            {
                Action.eventChannel.AddListener(Harvest);
            }
            else if (Action.signature.actionType == ActionType.Inspect)
            {
                Action.eventChannel.AddListener(Inspect);
            }
            // need to have something to remove elements if they don't meet criteria
        }
    }

    public void Harvest()
    {
        print("It's farming time!");
        Destroy(this.gameObject);
    }

    public void Inspect()
    {
        print("It's a plant!");
    }
}
