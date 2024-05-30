using System.Collections;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ParentHouse {
    public class Pickup : MonoBehaviour {
        [FoldoutGroup("Data")] public Inventory pockets;

        [FoldoutGroup("Data")] [SerializeField]
        private Inventory lostInventory;

        [FoldoutGroup("Data")] public Item item;
        [FoldoutGroup("Data")] public int amount;

        [FoldoutGroup("Data")] [SerializeField]
        private float expulseForce;

        [FoldoutGroup("Data")] [SerializeField]
        private float expulseHeightMultiplier;

        [FoldoutGroup("Data")] [SerializeField]
        private float activationTime = 2f;

        [FoldoutGroup("Data")] [SerializeField]
        private float despawnTimer;

        [FoldoutGroup("Data")] [SerializeField]
        private StringVariable itemNameVariable;

        [FoldoutGroup("Data")] [SerializeField]
        private Rigidbody rb;

        [FoldoutGroup("Display")] [SerializeField]
        private SpriteRenderer litRenderer;

        [FoldoutGroup("Display")] [SerializeField]
        private SpriteRenderer shadowRenderer;

        private bool canPickup;

        private void Awake() {
            // Todo block behind toggle?
            if (item != null)
                Setup(item, amount);
        }

        private void OnDisable() {
        }

        private void OnMouseEnter() {
            itemNameVariable.Value = item.ItemName;
        }

        private void OnMouseExit() {
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) canPickup = true;
        }

        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("Player") && canPickup) {
                var displayAmount = amount;
                amount = pockets.AddItem(item, amount);
                if (displayAmount != amount) {
                    item.OnPickupItem.Invoke();
                    GameEvents.OnPickupItem.Invoke();
                    GameEvents.OnCreateImageStringMessage.Invoke(item.ItemIcon, $"+{displayAmount}");
                }

                if (amount <= 0)
                    Destroy(gameObject);
            }
        }

        public void Setup(Item incomingItem, int quantity) {
            item = incomingItem;
            amount = quantity;
            shadowRenderer.sprite = litRenderer.sprite = item.ItemIcon;
            ExpulsePickup();
            StartCoroutine(DelayActivate());
            StartCoroutine(Despawn());
        }

        [Button]
        private void ExpulsePickup() {
            var x = Random.Range(-1f, 1f);
            var y = Random.Range(0.25f, 1f) * expulseHeightMultiplier;
            var z = Random.Range(-1f, 1f);
            rb.AddForce(new Vector3(x, y, z) * expulseForce);
        }

        private IEnumerator DelayActivate() {
            yield return new WaitForSeconds(activationTime);
            canPickup = true;
        }

        private IEnumerator Despawn() {
            yield return new WaitForSeconds(despawnTimer);
            lostInventory.AddItem(item, amount);
            GameEvents.OnDespawnItem.Invoke();
            Destroy(gameObject);
        }
    }
}