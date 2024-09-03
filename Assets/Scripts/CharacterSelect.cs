using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour {
    [SerializeField] [FoldoutGroup("Dependencies")]
    private Menu ThisMenu;
    
    [SerializeField] [FoldoutGroup("Dependencies")]
    private Menu CharacterCreateMenu;
    
    [SerializeField] [FoldoutGroup("Dependencies")]
    private GameObject NewCharacterButtonObject;
    
    [SerializeField] [FoldoutGroup("Dependencies")]
    private GameObject SelectCharacterButtonObject;
    
    [SerializeField] [FoldoutGroup("Dependencies")]
    private RectTransform Content;

    [SerializeField] [FoldoutGroup("Settings")] // This should be moved outside of UI Control
    private int CharacterSlots;
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private int CurrentTargetSlot;

    private void Awake() {
        LoadButtons();
    }

    private void LoadButtons() {
        foreach (Transform child in Content) {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < CharacterSlots; i++) {
            var path = SaveLoad.PlayerSavePath(i);
            if (SaveLoad.HasPath(path)) {
                AddSelectCharacterButton((PlayerData)SaveLoad.Load(SaveLoad.PlayerSavePath(i)));
            }
            else {
                AddCreateCharacterButton(i);
            }
        }
    }

    private void AddSelectCharacterButton(PlayerData associatedData) {
        var obj = Instantiate(SelectCharacterButtonObject, Content, false);
        var btn = obj.GetComponent<JuicyButton>();
        btn.OnButtonClick.AddListener(delegate { CurrentPlayerInfo.LoadPlayer(associatedData.SlotId); });
        btn.OnButtonClick.AddListener(StartWithSelectedCharacter);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{associatedData.PlayerName}";
    }

    private void AddCreateCharacterButton(int index) {
        var obj = Instantiate(NewCharacterButtonObject, Content, false);
        var btn = obj.GetComponent<JuicyButton>();
        btn.OnButtonClick.AddListener(CharacterCreateMenu.Open);
        btn.OnButtonClick.AddListener(delegate { CurrentTargetSlot = index; });
        btn.OnButtonClick.AddListener(ThisMenu.Close);
    }

    // This should be moved outside of UI Control
    public void StartWithSelectedCharacter() {
        if (CurrentPlayerInfo.Data != null) {
            SceneManager.LoadScene(1);
        }
    }

    public void CreateCharacter(TMP_InputField input) {
        PlayerData newData = new() {
            SlotId = CurrentTargetSlot,
            PlayerName = input.text,
            UniqueId = Guid.NewGuid().ToString()
        };
        SaveLoad.Save(newData,SaveLoad.PlayerSavePath(CurrentTargetSlot));
        LoadButtons();
    }
}
