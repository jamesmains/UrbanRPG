using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Pickup : MonoBehaviour
{
    [FoldoutGroup("Data")]public Inventory pockets;
    [FoldoutGroup("Data")][SerializeField] private Inventory lostInventory;
    [FoldoutGroup("Data")]public Item item;
    [FoldoutGroup("Data")]public int amount;
    [FoldoutGroup("Data")][SerializeField] private float expulseForce;
    [FoldoutGroup("Data")][SerializeField] private float expulseHeightMultiplier;
    [FoldoutGroup("Data")][SerializeField] private float activationTime = 2f;
    [FoldoutGroup("Data")][SerializeField] private FloatVariable despawnTimer;
    [FoldoutGroup("Data")] [SerializeField] private StringVariable itemNameVariable;
    [FoldoutGroup("Data")][SerializeField] private Rigidbody rb;
    [FoldoutGroup("Display")][SerializeField] private SpriteRenderer litRenderer;
    [FoldoutGroup("Display")][SerializeField] private SpriteRenderer shadowRenderer;

    [FoldoutGroup("Events")] [SerializeField] private GameEvent onMouseEnter;
    [FoldoutGroup("Events")] [SerializeField] private GameEvent onMouseExit;
    [FoldoutGroup("Events")] [SerializeField] private GameEvent onPickupItem;

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

    private void OnMouseEnter()
    {
        onMouseEnter.Raise();
        itemNameVariable.Value = item.Name;
    }

    private void OnMouseExit()
    {
        onMouseExit.Raise();
    }

    private void OnDisable()
    {
        onMouseExit.Raise();
    }
}
