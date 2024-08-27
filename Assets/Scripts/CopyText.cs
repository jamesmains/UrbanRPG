using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class CopyText : MonoBehaviour {
    [SerializeField] [FoldoutGroup("Dependencies")]
    private TextMeshProUGUI TargetText;

    public void Copy() {
        //UniClipboard.SetText(TargetText.text);
    }
}
