using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using UnityEngine;

public class Pickup : Activity
{
    public Inventory pockets;
    public Item item;
    public int amount;

    private void OnTriggerEnter(Collider other)
    {
        var i = (item.GetType());
        amount = pockets.TryAddItem(item, amount);
        if(amount == 0)
            Destroy(this.gameObject);
    }
}
