using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ParentHouse.UI {
    public class BannerDisplayPopup : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI messageDisplay;
        [SerializeField] private Animator animator;
        [SerializeField] private Queue<string> messageQueue = new();
        [SerializeField] private float waitTime = 2f;
        private float timer;

        private void Update() {
            if (messageQueue.Count > 0) {
                if (timer > 0) {
                    timer -= Time.deltaTime;
                }
                else {
                    DisplayMessage(messageQueue.Dequeue());
                    if (messageQueue.Count > 0)
                        timer = waitTime;
                }
            }
        }

        private void DisplayMessage(string message) {
            messageDisplay.text = message;
            animator.SetTrigger("Invoke");
        }

        private void QueueReputationChangeMessage(Actor actor) {
            messageQueue.Enqueue(
                $"Now <allcaps>{actor.GetCurrentReputationTier()}</allcaps> with <allcaps>{actor.actorName}</allcaps>!");
        }

        private void QueueGenericMessage(string message) {
            messageQueue.Enqueue(message);
        }
    }
}