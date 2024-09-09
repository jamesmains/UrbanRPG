using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SignTextDisplay : MonoBehaviour {
    [SerializeField] [BoxGroup("Dependencies")]
    private TextMeshProUGUI SignTitle;
    
    [SerializeField] [BoxGroup("Dependencies")]
    private TextMeshProUGUI SignText;
    
    [SerializeField] [FoldoutGroup("Events")]
    private UnityEvent OnOpenSignMenu;
    
    [SerializeField] [FoldoutGroup("Events")]
    private UnityEvent OnCloseSignMenu;
    
    public static readonly UnityEvent<string,string> OnShowSignText = new();
    public static readonly UnityEvent OnCloseSign = new ();

    private void Awake() {
        OnShowSignText.AddListener(ShowSignText);
        OnCloseSign.AddListener(CloseSign);
    }

    private void ShowSignText(string title, string text) {
        SignTitle.text = title;
        SignText.text = text;
        OnOpenSignMenu.Invoke();
    }

    private void CloseSign() {
        OnCloseSignMenu.Invoke();
    }
}
