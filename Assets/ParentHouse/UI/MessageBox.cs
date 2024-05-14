using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ParentHouse.UI {
    /// <summary>
    /// Handles creating a single message box that can be interacted by the player to perform actions.
    /// Currently cannot scale beyond a single box.
    /// </summary>
    public class MessageBox : MonoBehaviour {
        private static MessageBox Instance;

        [SerializeField] [FoldoutGroup("Dependencies")]
        private Transform MessageBoxTransform;

        [SerializeField] [FoldoutGroup("Dependencies")]
        private GameObject ClickProtection;

        [SerializeField] [FoldoutGroup("Dependencies/Prefabs")]
        private GameObject NewLineObject;

        [SerializeField] [FoldoutGroup("Dependencies/Prefabs")]
        private GameObject TextObject;

        [SerializeField] [FoldoutGroup("Dependencies/Prefabs")]
        private GameObject InputFieldObject;

        [SerializeField] [FoldoutGroup("Dependencies/Prefabs")]
        private GameObject ButtonObject;

        [SerializeField] [FoldoutGroup("Dependencies/Prefabs")]
        private GameObject HorizontalFiller;

        [SerializeField] [FoldoutGroup("Dependencies/Prefabs")]
        private GameObject VerticalFiller;

        [SerializeField] [FoldoutGroup("Status")]
        private Transform CurrentLine;

        private void Awake() {
            Instance = this;
            Clear();
        }

        public static void SetWindowScaleFactor(float scaleFactor = 1) {
            Instance.MessageBoxTransform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }

        public static void AddText(string newText, float fontSize = 36) {
            var obj = Instantiate(Instance.TextObject, Instance.CurrentLine);
            var textDisplay = obj.GetComponentInChildren<TextMeshProUGUI>();
            textDisplay.text = newText;
            textDisplay.fontSize = fontSize;
        }

        public static TMP_InputField AddInputField(string placeholderText, float displayLength = 250f,
            UnityAction onSubmit = null, UnityAction onTextChanged = null) {
            var obj = Instantiate(Instance.InputFieldObject, Instance.CurrentLine);
            var inputField = obj.GetComponentInChildren<TMP_InputField>();
            inputField.GetComponent<RectTransform>().sizeDelta = new Vector2(displayLength, 52f);
            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = placeholderText;
            return inputField;
            // inputField.onEndEdit.AddListener(onSubmit);
        }

        public static void AddButton(string buttonText, UnityAction onClick = null) {
            var obj = Instantiate(Instance.ButtonObject, Instance.CurrentLine);
            obj.GetComponentInChildren<Button>().onClick.AddListener(onClick);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        }

        public static void HorizontalSpace(float space) {
            Instantiate(Instance.HorizontalFiller, Instance.CurrentLine).transform.GetChild(0)
                .GetComponent<RectTransform>().sizeDelta = new Vector2(space, 0);
        }

        public static void VerticalSpace(float space) {
            Instantiate(Instance.VerticalFiller, Instance.MessageBoxTransform).GetComponent<RectTransform>().sizeDelta =
                new Vector2(0, space);
            NewLine();
        }

        public static void NewLine() {
            Instance.CurrentLine = Instantiate(Instance.NewLineObject, Instance.MessageBoxTransform).transform;
        }

        public static void Clear() {
            var oldItems = Instance.MessageBoxTransform.GetComponentsInChildren<Transform>();
            foreach (var item in oldItems)
                if (item != Instance.MessageBoxTransform)
                    Destroy(item.gameObject);
            Instance.MessageBoxTransform.gameObject.SetActive(false);
            Instance.ClickProtection.SetActive(false);
        }

        // Accessed Directly
        public static void Show(UnityAction actions) {
            Instance.Setup(actions);
        }

        // Accessed Via Unity Action Event Listener
        public void Setup(UnityAction actions) {
            CurrentLine = null;
            if (MessageBoxTransform.childCount > 0)
                Clear();
            MessageBoxTransform.gameObject.SetActive(true);
            Instance.ClickProtection.SetActive(true);
            SetWindowScaleFactor();
            actions.Invoke();
            VerticalSpace(32);
        }
    }
}