using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ParentHouse.UI {
    public class MessageBox : MonoBehaviour {
        public static MessageBox Instance;
        [SerializeField] private Transform window;
        [SerializeField] private GameObject clickProtection;

        [SerializeField] [FoldoutGroup("Objects")]
        private GameObject newLineObject;

        [SerializeField] [FoldoutGroup("Objects")]
        private GameObject textObject;

        [SerializeField] [FoldoutGroup("Objects")]
        private GameObject inputFieldObject;

        [SerializeField] [FoldoutGroup("Objects")]
        private GameObject buttonObject;

        [SerializeField] [FoldoutGroup("Objects")]
        private GameObject horizontalFiller;

        [SerializeField] [FoldoutGroup("Objects")]
        private GameObject verticalFiller;

        private Transform currentLine;

        private void Awake() {
            Instance = this;
            Clear();
        }

        public static void SetWindowScaleFactor(float scaleFactor = 1) {
            Instance.window.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }

        public static void AddText(string newText, float fontSize = 36) {
            var obj = Instantiate(Instance.textObject, Instance.currentLine);
            var textDisplay = obj.GetComponentInChildren<TextMeshProUGUI>();
            textDisplay.text = newText;
            textDisplay.fontSize = fontSize;
        }

        public static TMP_InputField AddInputField(string placeholderText, float displayLength = 250f,
            UnityAction onSubmit = null, UnityAction onTextChanged = null) {
            var obj = Instantiate(Instance.inputFieldObject, Instance.currentLine);
            var inputField = obj.GetComponentInChildren<TMP_InputField>();
            inputField.GetComponent<RectTransform>().sizeDelta = new Vector2(displayLength, 52f);
            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = placeholderText;
            return inputField;
            // inputField.onEndEdit.AddListener(onSubmit);
        }

        public static void AddButton(string buttonText, UnityAction onClick = null) {
            var obj = Instantiate(Instance.buttonObject, Instance.currentLine);
            obj.GetComponentInChildren<Button>().onClick.AddListener(onClick);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        }

        public static void HorizontalSpace(float space) {
            Instantiate(Instance.horizontalFiller, Instance.currentLine).transform.GetChild(0)
                .GetComponent<RectTransform>().sizeDelta = new Vector2(space, 0);
        }

        public static void VerticalSpace(float space) {
            Instantiate(Instance.verticalFiller, Instance.window).GetComponent<RectTransform>().sizeDelta =
                new Vector2(0, space);
            NewLine();
        }

        public static void NewLine() {
            Instance.currentLine = Instantiate(Instance.newLineObject, Instance.window).transform;
        }

        public static void Clear() {
            var oldItems = Instance.window.GetComponentsInChildren<Transform>();
            foreach (var item in oldItems)
                if (item != Instance.window)
                    Destroy(item.gameObject);
            Instance.window.gameObject.SetActive(false);
            Instance.clickProtection.SetActive(false);
        }

        // Accessed Directly
        public static void Show(UnityAction actions) {
            Instance.Setup(actions);
        }

        // Accessed Via Unity Action Event Listener
        public void Setup(UnityAction actions) {
            currentLine = null;
            if (window.childCount > 0)
                Clear();
            window.gameObject.SetActive(true);
            Instance.clickProtection.SetActive(true);
            SetWindowScaleFactor();
            actions.Invoke();
            VerticalSpace(32);
        }
    }
}