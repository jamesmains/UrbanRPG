using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Inventory pockets;
    public Item item;

    private void OnTriggerEnter(Collider other)
    {
        var i = (item.GetType());
        pockets.TryAddItem(item ,1);
        Destroy(this.gameObject);
    }
}
