using Sirenix.OdinInspector;
using UnityEngine;

public class InteractableSign : MonoBehaviour, Interactable {
    [SerializeField] [BoxGroup("Settings")]
    private string TitleTextToDisplay;

    [SerializeField] [BoxGroup("Settings")] [TextArea]
    private string TextToDisplay;

    [SerializeField] [BoxGroup("Settings")]
    private int InteractionPriority = 10;

    [SerializeField] [BoxGroup("Status")] private bool CanInteract = true;

    public int Priority => InteractionPriority;

    public void Interact() {
        if (!CanInteract) return;
        CanInteract = false;
        SignTextDisplay.OnShowSignText.Invoke(TitleTextToDisplay, TextToDisplay);
    }

    public void ExitInteraction() {
        CanInteract = true;
        SignTextDisplay.OnCloseSign.Invoke();
    }
}