using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private PlayerSaveSlot playerSaveSlot;
    [SerializeField] private GameEvent onLoadNextScene;
    [SerializeField] private GameObject saveSlotListObject;
    [SerializeField] private GameObject emptySaveSlotListObject;
    [SerializeField] private Transform saveSlotListObjectContainer;
    [SerializeField] private int totalSaveSlotCount;
    [SerializeField] private UnityActionGameEvent messageBoxEvent;
    private void Awake()
    {
        UpdateSaveSlotDisplay();
    }

    public void UpdateSaveSlotDisplay()
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
                .Setup(tempData.characterName,tempData.cityName,Actions);
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
                    messageBoxEvent.Raise(CityNameMessageBox);
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
                    print("ready to goto next");
                }
            }

            void ExitButton()
            {
                messageBoxEvent.Raise(CharacteNameMessageBox);
            }

            MessageBox.NewLine();
            MessageBox.AddButton("Confirm", ConfirmButton);
            MessageBox.AddButton("Back", ExitButton);
        }
        
        messageBoxEvent.Raise(CharacteNameMessageBox);
    }

    private void LoadGame(string saveName)
    {
        Debug.Log(saveName);
        playerSaveSlot.saveSlot = saveName;
        playerSaveSlot.LoadData();
        onLoadNextScene.Raise();
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
