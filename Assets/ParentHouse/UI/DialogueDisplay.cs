using System.Collections;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ParentHouse.UI {
    public class DialogueDisplay : WindowPanel {
        [FoldoutGroup("Data")] [SerializeField]
        private float textSpeed;

        [FoldoutGroup("Data")] [SerializeField]
        private int textSfxSpeed;

        [FoldoutGroup("Display")] [SerializeField]
        private Image actorDisplayImage;

        [FoldoutGroup("Display")] [SerializeField]
        private TextMeshProUGUI actorNameText;

        [FoldoutGroup("Display")] [SerializeField]
        private TextMeshProUGUI displayText;

        [FoldoutGroup("Display")] [SerializeField]
        private AudioSource talkSfxSource;

        [SerializeField, FoldoutGroup("Debug"), ReadOnly]
        private Dialogue currentDialogue;

        [SerializeField, FoldoutGroup("Debug"), ReadOnly]
        private int dialogueSegmentIndex;

        [SerializeField, FoldoutGroup("Debug"), ReadOnly]
        private string currentDialogueSegmentText;

        [SerializeField, FoldoutGroup("Debug"), ReadOnly]
        private bool atEndOfDialogueSegment;

        [SerializeField, FoldoutGroup("Debug"), ReadOnly]
        private bool skipButtonProtection;

        private DialogueSegment currentSegment;

        public void StartDialogue(Dialogue incomingDialogue) {
            if (!incomingDialogue.IsConditionMet()) return;
            atEndOfDialogueSegment = false;
            skipButtonProtection = true;
            displayText.text = "";
            Global.PlayerLock++;
            currentDialogue = incomingDialogue;
            dialogueSegmentIndex = 0;
            Show();
            NextDialogueSegment();
        }

        private void SkipToEndOfSegement() {
            if (skipButtonProtection) {
                skipButtonProtection = false;
                return;
            }

            StopAllCoroutines();
            displayText.text = currentDialogueSegmentText;
            atEndOfDialogueSegment = true;
        }

        private void SetupNextDialogueSegment() {
            atEndOfDialogueSegment = false;
            dialogueSegmentIndex++;
            if (dialogueSegmentIndex >= currentDialogue.DialogueSegments.Count) {
                EndDialogue();
                if (currentSegment.IsConditionMet()) {
                    currentSegment?.OnFinishSegment?.Invoke();
                }
            }
            else {
                if (dialogueSegmentIndex < currentDialogue.DialogueSegments.Count) {
                    if (currentSegment.IsConditionMet()) {
                        currentSegment?.OnFinishSegment?.Invoke();
                    }
                }

                NextDialogueSegment();
            }
        }

        private void NextDialogueSegment() {
            currentSegment = currentDialogue.DialogueSegments[dialogueSegmentIndex];
            if (currentSegment.IsConditionMet()) {
                if (currentSegment.IsInstantSegment) {
                    currentSegment.OnStartSegment?.Invoke();
                    currentSegment.OnFinishSegment?.Invoke();
                    SetupNextDialogueSegment();
                }
                else {
                    StopAllCoroutines();
                    StartCoroutine(ProcessText());
                }
            }
            else SetupNextDialogueSegment();
        }

        IEnumerator ProcessText() {
            int charIndex = 0;

            currentSegment?.OnStartSegment?.Invoke();
            if (currentSegment != null) {
                bool hasActor = currentSegment.actor != null;
                actorNameText.transform.parent.gameObject.SetActive(hasActor);
                actorDisplayImage.enabled = hasActor;
                if (hasActor) {
                    var actor = currentSegment.actor;
                    actorNameText.text = actor.actorName;
                    actorDisplayImage.sprite = actor.actionIcon;
                    talkSfxSource.clip = actor.talkSfx;
                }
                else talkSfxSource.clip = null;

                displayText.text = "";
                currentDialogueSegmentText = currentSegment.speech;
            }

            int t = 0;
            while (charIndex < currentDialogueSegmentText.Length) {
                displayText.text += currentDialogueSegmentText[charIndex];
                if (talkSfxSource.clip != null && t == 0) {
                    talkSfxSource.pitch = Random.Range(0.775f, 1.15f);
                    talkSfxSource.Play();
                }

                t++;
                if (t >= textSfxSpeed) t = 0;
                charIndex++;
                yield return new WaitForSeconds(textSpeed);
            }

            atEndOfDialogueSegment = true;
        }

        private void EndDialogue() {
            StopAllCoroutines();
            Hide();
            Global.PlayerLock--;
            GameEvents.OnEndActivity.Invoke();
            GameEvents.ShowPlayerHud.Invoke();
            currentDialogue.EndDialogue();
        }
    }
}