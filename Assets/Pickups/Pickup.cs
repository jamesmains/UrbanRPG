using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pickup : Activity
{
    public Inventory pockets;
    public Item item;
    public int amount;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float expulseForce;
    [SerializeField] private float expulseHeightMultiplier;
    [SerializeField] private float activationTime = 2f;
    [SerializeField] private SpriteRenderer litRenderer;
    [SerializeField] private SpriteRenderer shadowRenderer;
    [SerializeField] private Inventory lostInventory;
    [SerializeField] private GameEvent onPickupItem;
    [SerializeField] private FloatVariable despawnTimer;
    private bool canPickup = false;

    private void Awake()
    {
        // Todo block behind toggle?
        if(item != null)
            Setup(item,amount);
    }

    public void Setup(Item incomingItem, int quantity)
    {
        item = incomingItem;
        amount = quantity;
        shadowRenderer.sprite = litRenderer.sprite = item.Sprite;
        ExpulsePickup();
        StartCoroutine(DelayActivate());
        StartCoroutine(Despawn());
    }

    [Button]
    private void ExpulsePickup()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(0.25f, 1f)*expulseHeightMultiplier;
        float z = Random.Range(-1f, 1f);
        rb.AddForce(new Vector3(x,y,z) * expulseForce);
    }
    
    IEnumerator DelayActivate()
    {
        yield return new WaitForSeconds(activationTime);
        canPickup = true;
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTimer.Value);
        lostInventory.TryAddItem(item, amount);
        onPickupItem.Raise();
        Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && canPickup)
        {
            amount = pockets.TryAddItem(item, amount);
            onPickupItem.Raise();
            if(amount == 0)
                Destroy(this.gameObject); 
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
        }
    }
}
