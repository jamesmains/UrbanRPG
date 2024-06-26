using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace ParentHouse {
    public class CollisionTrigger : MonoBehaviour {
        // Need to make this more robust
        // Todo allow multiple tags
        // Todo allow onExit events
        // Todo allow onStay events
        // Todo allow to mark entities and not allow that specific one to invoke the events

        [SerializeField] private bool destroyAfterSomeTime;

        [SerializeField] [ShowIf("destroyAfterSomeTime")]
        private float destroyTimer;

        [SerializeField] protected string targetTag;
        [SerializeField] [PropertyOrder(100)] protected UnityEvent onEnter;

        private void Awake() {
            if (destroyAfterSomeTime)
                StartCoroutine(DestroyDelay());
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag(targetTag)) onEnter.Invoke();
        }

        private IEnumerator DestroyDelay() {
            yield return new WaitForSeconds(destroyTimer);
            Destroy(gameObject);
        }
    }
}