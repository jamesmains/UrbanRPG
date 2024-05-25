using System;
using ParentHouse.Game;
using UnityEngine;
using UnityEngine.Events;

namespace ParentHouse.Utils {
    public class GameEvents : MonoBehaviour {
        // Game State Events
        public static UnityEvent<GameState> GameStateEntered = new();        
        public static UnityEvent<GameState> GameStateExited = new();        
        // UI Events
        public static UnityEvent ShowPlayerHud = new();
        public static UnityEvent HidePlayerHud = new();

        // Activity Events
        public static UnityEvent OpenCalendarDayDetails = new();
        public static UnityEvent<ActivityTrigger> OnOpenActivityWheel = new();
        public static UnityEvent<float, UnityEvent, Sprite> OnStartActivity = new();
        public static UnityEvent OnCancelActivity = new();
        public static UnityEvent OnEndActivity = new();
        public static UnityEvent OnCloseActivityWheel = new();

        // Systems
        public static UnityEvent<SceneTransition> OnLoadNextScene = new();
        public static UnityEvent<string> OnShowTooltip = new();
        public static UnityEvent OnHideTooltip = new();

        // Time
        public static UnityEvent OnChangeTime = new();
        public static UnityEvent OnNewDay = new();

        // Inventory and items
        public static UnityEvent OnMouseExitInventorySlot = new();
        public static UnityEvent OnUpdateInventory = new();
        public static UnityEvent OnUpdateMoneyDisplay = new();
        public static UnityEvent OnPickupItem = new();
        public static UnityEvent OnDespawnItem = new();
        public static UnityEvent OnMoveOrAddItem = new();
        public static UnityEvent<Item> OnItemMove = new();
        public static UnityEvent OnItemRelease = new();
        public static UnityEvent OnChangeRide = new();

        // POPUP MESSAGES
        public static UnityEvent<Actor> OnSendReputationChangeMessage = new();
        public static UnityEvent<string> OnSendGenericMessage = new();
        public static UnityEvent<Sprite, string> OnCreateImageStringMessage = new();
        public static UnityEvent<Sprite, string> OnCreateSpriteStringPopup = new();

        // Quests
        public static UnityEvent<Quest> OnAcceptQuest = new();
        public static UnityEvent<Quest> OnMakeQuestProgress = new();
        public static UnityEvent<Quest> OnReadyToComplete = new();
        public static UnityEvent<Quest> OnCompleteQuest = new();
        public static UnityEvent OnUpdateQuests = new();

        // Skills & Needs
        public static UnityEvent OnLevelUp = new();
        public static UnityEvent OnGainExperience = new();
        public static UnityEvent OnPassout = new();
        public static UnityEvent<Sprite> OnNeedDecayTrigger = new();

        // Dialogue
        public static UnityEvent<Dialogue> StartDialogueEvent = new();

        // Shop
        public static UnityEvent<Shop> OnOpenShop = new();
        public static UnityEvent OnCartQuantityChange = new();

        // Unsorted
        public static UnityEvent OnPlayerMoved = new();
        public static UnityEvent<UnityAction> OnCreateMessageBox = new();

        // Dressing Room
        public static UnityEvent<Actor> OnUpdateOutfit = new();
    }
}