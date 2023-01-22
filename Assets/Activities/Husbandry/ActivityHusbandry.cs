using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityHusbandry : Activity
{
    private void Awake()
    {
        foreach (var Action in ActivityActions)
        {
            print(Action.signature.actionType);
            if (Action.signature.actionType == ActionType.Harvest)
            {
                Action.eventChannel.AddListener(Milk);
            }
            else if (Action.signature.actionType == ActionType.Inspect)
            {
                Action.eventChannel.AddListener(Inspect);
            }
            // need to have something to remove elements if they don't meet criteria
        }
    }

    public void Milk()
    {
        print("It's milking time!");
        Destroy(this.gameObject);
    }

    public void Inspect()
    {
        print("It's a cow!");
    }
}
