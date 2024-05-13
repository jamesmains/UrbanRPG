using System.IO;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace ParentHouse.UI {
    public class MainMenuManager : MonoBehaviour
    {
        [FoldoutGroup("Data")][SerializeField] private GameObject saveSlotListObject;
        [FoldoutGroup("Data")][SerializeField] private GameObject emptySaveSlotListObject;
        [FoldoutGroup("Data")][SerializeField] private Transform saveSlotListObjectContainer;
        [FoldoutGroup("Data")][SerializeField] private PlayerSaveSlot playerSaveSlot;
        [FoldoutGroup("Data")][SerializeField] private int totalSaveSlotCount;
    
        private void Awake()
        {
            UpdateSaveSlotDisplay();
        }

        private void UpdateSaveSlotDisplay()
        {
            int remainingSaveSlots = totalSaveSlotCount;
            string saveDirectoryPath = $"{Application.persistentDataPath}/";
            var dirs = Directory.GetDirectories(saveDirectoryPath);
            foreach (var dir in dirs)
            {
                string slotName = dir.Substring(dir.LastIndexOf('/')+1);

                void Actions()
                {
                    LoadGame(slotName);
                }

                PlayerSaveSlot tempData = ScriptableObject.CreateInstance<PlayerSaveSlot>();
                tempData.LoadData(slotName);
                Instantiate(saveSlotListObject, saveSlotListObjectContainer)
                    .GetComponent<SaveSlotDisplay>()
                    .Setup(tempData.actorName,tempData.cityName,Actions);
                remainingSaveSlots--;
            }

            for (int i = 0; i < remainingSaveSlots; i++)
            {
                UnityAction actions = StartNewGame;
                Instantiate(emptySaveSlotListObject,saveSlotListObjectContainer)
                    .GetComponent<SaveSlotDisplay>()
                    .Setup(null,null,actions);
            }
        }
    
        private void StartNewGame()
        {
            bool ValidateInput(TMP_InputField input)
            {
                return !string.IsNullOrEmpty(input.text);
            }

            void CharacteNameMessageBox()
            {
                MessageBox.SetWindowScaleFactor(2);
                MessageBox.NewLine();
                MessageBox.AddText("What is your name?");
                MessageBox.VerticalSpace(16);
                MessageBox.NewLine();
                var inputField = MessageBox.AddInputField("Enter name...", 400);

                void ConfirmButton()
                {
                    if (ValidateInput(inputField))
                    {
                        GameEvents.OnCreateMessageBox.Invoke(CityNameMessageBox);
                    }
                }

                void ExitButton()
                {
                    MessageBox.Clear();
                }

                MessageBox.NewLine();
                MessageBox.AddButton("Confirm", ConfirmButton);
                MessageBox.AddButton("Back", ExitButton);
            }

            void CityNameMessageBox()
            {
                MessageBox.SetWindowScaleFactor(2);
                MessageBox.NewLine();
                MessageBox.AddText("Name your city:");
                MessageBox.VerticalSpace(16);
                MessageBox.NewLine();
            
                var inputField = MessageBox.AddInputField("Enter name...", 400);

                void ConfirmButton()
                {
                    if (ValidateInput(inputField))
                    {
                        MessageBox.Clear();
                    }
                }

                void ExitButton()
                {
                    GameEvents.OnCreateMessageBox.Invoke(CityNameMessageBox);
                }

                MessageBox.NewLine();
                MessageBox.AddButton("Confirm", ConfirmButton);
                MessageBox.AddButton("Back", ExitButton);
            }
        
            GameEvents.OnCreateMessageBox.Invoke(CityNameMessageBox);
        }

        private void LoadGame(string saveName)
        {
            playerSaveSlot.saveSlot = saveName;
            playerSaveSlot.LoadData();
            GameEvents.OnLoadNextScene.Invoke(playerSaveSlot.NextSceneTransition);
        }

        private void ExitGame(bool save = false)
        {
            if (save)
            {
                playerSaveSlot.SaveData();
            }
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
