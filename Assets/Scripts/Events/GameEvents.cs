using System.Collections;
using System.Collections.Generic;
using i302.Utils.Events;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
#region Inputs
    // Primary Mouse Button Events
    public static i302Event OnPrimaryMouseButtonDown = new();
    public static i302Event OnPrimaryMouseButtonUp = new();
    public static i302Event OnPrimaryMouseButtonHeld = new();
    public static i302Event OnPrimaryMouseButtonReleased = new();
    
    // Alternate Mouse Button Events
    public static i302Event OnAltMouseButtonUp = new();
    public static i302Event OnAltMouseButtonDown = new();
    public static i302Event OnAltMouseButtonHeld = new();
    public static i302Event OnAltMouseButtonReleased = new();
    
    // Mouse Scroll Events
    public static i302Event<float> OnMouseScroll = new();
    
    // Move Left Button Events
    public static i302Event OnMoveLeftButtonDown = new();
    public static i302Event OnMoveLeftButtonUp = new();
    public static i302Event OnMoveLeftButtonHeld = new();
    public static i302Event OnMoveLeftButtonReleased = new();
    
    // Move Right Button Events
    public static i302Event OnMoveRightButtonDown = new();
    public static i302Event OnMoveRightButtonUp = new();
    public static i302Event OnMoveRightButtonHeld = new();
    public static i302Event OnMoveRightButtonReleased = new();
    
    // Move Up Button Events
    public static i302Event OnMoveUpButtonDown = new();
    public static i302Event OnMoveUpButtonUp = new();
    public static i302Event OnMoveUpButtonHeld = new();
    public static i302Event OnMoveUpButtonReleased = new();
    
    // Move Down Button Events
    public static i302Event OnMoveDownButtonDown = new();
    public static i302Event OnMoveDownButtonUp = new();
    public static i302Event OnMoveDownButtonHeld = new();
    public static i302Event OnMoveDownButtonReleased = new();
    
    // Interact Button Events
    public static i302Event OnInteractButtonDown = new();
    public static i302Event OnInteractButtonUp = new();
    public static i302Event OnInteractButtonHeld = new();
    public static i302Event OnInteractButtonReleased = new();
    
    // Ride Button Events
    public static i302Event OnRideButtonDown = new();
    public static i302Event OnRideButtonUp = new();
    public static i302Event OnRideButtonHeld = new();
    public static i302Event OnRideButtonReleased = new();
#endregion

    // UI Events
    public static i302Event OnMouseEnter = new();
    public static i302Event OnMouseExit = new();

    // Activity Events
    public static i302Event OpenCalendarDayDetails = new();
    public static i302Event<ActivityTrigger> OnOpenActivityWheel = new();
    public static i302Event<float,UnityEvent,Sprite> OnStartActivity = new();
    public static i302Event OnCancelActivity = new();
    public static i302Event OnEndActivity = new();
    public static i302Event OnCloseActivityWheel = new();

    // Systems
    public static i302Event<SceneTransition> OnLoadNextScene = new();
    public static i302Event<string> OnShowTooltip = new();
    public static i302Event OnHideTooltip = new();

    // Time
    public static i302Event OnChangeTime = new();
    public static i302Event OnNewDay = new();

    // Inventory and items
    public static i302Event OnMouseExitInventorySlot = new();
    public static i302Event OnUpdateInventory = new();
    public static i302Event OnUpdateMoneyDisplay = new();
    public static i302Event OnPickupItem = new();
    public static i302Event OnDespawnItem = new();
    public static i302Event OnMoveOrAddItem = new();
    public static i302Event<Item> OnItemMove = new();
    public static i302Event OnItemRelease = new();
    public static i302Event OnChangeRide = new();
    
    // POPUP MESSAGES
    public static i302Event<Actor> OnSendReputationChangeMessage = new();
    public static i302Event<string> OnSendGenericMessage = new();
    public static i302Event<Sprite, string> OnCreateImageStringMessage = new();
    public static i302Event<Sprite,string> OnCreateSpriteStringPopup = new();
    
    // Quests
    public static i302Event<Quest> OnAcceptQuest = new();
    public static i302Event<Quest> OnMakeQuestProgress = new();
    public static i302Event<Quest> OnReadyToComplete = new();
    public static i302Event<Quest> OnCompleteQuest = new();
    public static i302Event OnUpdateQuests = new();
    
    // Skills & Needs
    public static i302Event OnLevelUp = new();
    public static i302Event OnGainExperience = new();
    public static i302Event<Sprite> OnNeedDecayTrigger = new();
    
    // Dialogue
    public static i302Event<Dialogue> StartDialogueEvent = new();
    
    // Unsorted
    public static i302Event OnPlayerMoved = new();
    public static i302Event OnCartQuantityChange = new();
    public static i302Event<UnityAction> OnCreateMessageBox = new();
}
