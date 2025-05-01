using System;
using System.Collections;
using System.Collections.Generic;
using Parent_House_Framework.Interactions;
using Parent_House_Framework.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Urban.Dialogue {
    /// <summary>
    /// Todo: tie user input to handle user input function
    /// Todo: add transition effects for Begin, Next Segment, and End
    /// Todo: cosmetic, restructure class to be more readable
    /// </summary>
    public class DialogueManager : MonoBehaviour {
        private enum DialogueManagerState {
            Idle,
            Typing,
            Waiting,
            WaitingWithOptions
        }

        #region Parameters & Member Variables

        // Non-static Parameters
        [SerializeField, FoldoutGroup("Dependencies")]
        private DialogueBoxSettings DefaultBoxSettings;

        [SerializeField, FoldoutGroup("Dependencies")]
        private GameObject DialogueOptionBoxPrefab;

        [SerializeField, FoldoutGroup("Dependencies")]
        private Transform DialogueOptionBoxParent;

        [SerializeField, FoldoutGroup("Dependencies")]
        private TextMeshProUGUI SpeakerText;

        [SerializeField, FoldoutGroup("Dependencies")]
        private TextMeshProUGUI SpeechText;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private DialogueManagerState CurrentState = DialogueManagerState.Idle;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Dialogue CurrentDialogue;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private DialogueSegment CurrentSegment;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private List<DialogueOption> CurrentOptions;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private List<GameObject> CurrentOptionObjects;

        // Toggles GUI elements associated with the Dialogue Sequence
        [SerializeField, FoldoutGroup("Events")]
        private Trigger ToggleMenuTrigger;

        // Static Parameters
        public static bool IsConversationActive; // True if dialogue is open and active, false otherwise

        private Coroutine m_CurrentSegmentRoutine;

        #endregion
    
        #region Events

        // Static Events
        public static Action<Dialogue> StartConversation;
        public static Action EndConversation;

        #endregion

        #region Unity Functions

        private void OnEnable() {
            StartConversation += Begin;
            EndConversation += End;
        }

        private void OnDisable() {
            StartConversation -= Begin;
            EndConversation -= End;
        }

        #endregion
    
        #region Functions
    
        [Button]
        private void Begin(Dialogue dialogue) {
            IsConversationActive = true;
    
            CurrentDialogue = dialogue;
            if (CurrentDialogue == null) {
                Debug.LogError("No dialogue found!");
                return;
            }

            CurrentSegment = CurrentDialogue.GetFirstSegment();
            if (CurrentSegment == null) {
                Debug.LogError("No segment found in dialogue");
                return;
            }
            ToggleMenuTrigger.OnChangeState.Invoke(true, null);
            ClearText();
            m_CurrentSegmentRoutine = StartCoroutine(TypeSegment());
        }

        private void BeginAt(Dialogue dialogue, DialogueSegment beginSegment) {
            IsConversationActive = true;

            CurrentDialogue = dialogue;
            if (CurrentDialogue == null) {
                Debug.LogError("No dialogue found!");
                return;
            }

            CurrentSegment = beginSegment;
            if (CurrentSegment == null) {
                Debug.LogError("No segment found in dialogue");
                return;
            }

            ClearText();
            m_CurrentSegmentRoutine = StartCoroutine(TypeSegment());
        }

        [Button]
        private void End() {
            IsConversationActive = false;
            CurrentState = DialogueManagerState.Idle;
            ToggleMenuTrigger.OnChangeState.Invoke(false, null);
        }

        [Button]
        private void HandleUserInput() {
            if (CurrentState == DialogueManagerState.Idle ||
                CurrentState == DialogueManagerState.WaitingWithOptions) return;
        
            if (CurrentState == DialogueManagerState.Waiting)
                GotoNextSegment();
            else if (CurrentState == DialogueManagerState.Typing)
                SkipToEndOfSegment();
        }

        [Button]
        private void GotoNextSegment() {
            if (m_CurrentSegmentRoutine != null) {
                StopCoroutine(m_CurrentSegmentRoutine);
            }

            if (EndConversationCheck()) return;
            GetNextSegment();
            SetDialogueBoxDisplay();
            ClearText();
            m_CurrentSegmentRoutine = StartCoroutine(TypeSegment());
        }

        private bool EndConversationCheck() {
            if (CurrentSegment.Type == DialogueSegment.SegmentType.EndConversation) {
                End();
                return true;
            }

            return false;
        }

        private void GetNextSegment() {
            var nextSegmentInfo = CurrentDialogue.GetNextSegment();
            if (nextSegmentInfo.Item1 == null) {
                EndConversation?.Invoke();
                return;
            }

            CurrentSegment = nextSegmentInfo.Item1;
            CurrentOptions = nextSegmentInfo.Item2;
        }

        private void ClearText() {
            SpeakerText.text = String.Empty;
            SpeechText.text = String.Empty;
            ClearOptions();
        }

        private void ClearOptions() {
            foreach (var option in CurrentOptionObjects) {
                option.SetActive(false);
            }
        }

        [Button]
        private void SkipToEndOfSegment() {
            if (CurrentState != DialogueManagerState.Typing)
                return;
            if (m_CurrentSegmentRoutine != null) {
                StopCoroutine(m_CurrentSegmentRoutine);
            }

            SpeakerText.text = CurrentSegment.GetSpeakerName();
            SpeechText.text = CurrentSegment.GetContent();
            var hasOptions = ShowOptions();
            CurrentState = hasOptions ? DialogueManagerState.WaitingWithOptions : DialogueManagerState.Waiting;
        }

        // Types out the current dialogue segment content to the speech text
        private IEnumerator TypeSegment() {
            CurrentState = DialogueManagerState.Typing;
            float typeSpeed = CurrentSegment.GetDialogueBoxSettings()
                ? CurrentSegment.GetDialogueBoxSettings().TypeSpeed
                : DefaultBoxSettings.TypeSpeed;
            string content = CurrentSegment.GetContent();
            SpeakerText.text = CurrentSegment.GetSpeakerName();
            int contentIndex = 0;

            while (contentIndex < content.Length) {
                if (contentIndex < content.Length) {
                    SpeechText.text += content[contentIndex++];
                }

                yield return new WaitForSeconds(typeSpeed);
            }

            SkipToEndOfSegment();
        }

        // Todo: This needs some refactoring to be more elegant and validating
        private bool ShowOptions() {
            var options = CurrentSegment.GetOptions();
            if (options == null || options.Count == 0) {
                return false;
            }

            foreach (var option in options) {
                if (option.Conditions != null && !option.Conditions.IsConditionMet()) continue;
                var optionObject = Pooler.Spawn(DialogueOptionBoxPrefab, DialogueOptionBoxParent);
                CurrentOptionObjects.Add(optionObject);
                optionObject.TryGetComponent<Button>(out var button);
                button?.onClick.RemoveAllListeners();
                var target = option.GetTargetDialogueAndSegment();
                button?.onClick.AddListener(delegate { BeginAt(target.Item1, target.Item2); });
                button.GetComponentInChildren<TextMeshProUGUI>().text = option.OptionName;
            }

            return true;
        }

        private void SetDialogueBoxDisplay() {
        }
        #endregion
    }
}