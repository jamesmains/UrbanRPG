using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using I302.Manu;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ParentHouse.UI
{
    public class Cursor : MonoBehaviour
{
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private IntVariable playerLockVariable;
    [SerializeField] private BoolVariable isMouseOverUserInterface;
    [SerializeField] private GameEvent onMoveOrAddItem;
    [SerializeField] [FoldoutGroup("Cursor GFX")] private Image defaultCursor;
    [SerializeField] [FoldoutGroup("Cursor GFX")] private Sprite defaultCursorIcon;
    [SerializeField] [FoldoutGroup("Cursor GFX")] private Sprite listCursorIcon;
    [SerializeField] [FoldoutGroup("Cursor GFX")] private Sprite emptyActivityHoverIcon;
    
    [SerializeField] [FoldoutGroup("Action List")] private LayerMask activityLayerMask;
    [SerializeField] [FoldoutGroup("Action List")] private RectTransform actionListContainer;
    [SerializeField] [FoldoutGroup("Action List")] private GameObject actionListObject;
    
    [SerializeField] [FoldoutGroup("Debug")] private CursorState CursorState;
    [SerializeField] [FoldoutGroup("Debug")] private float scrollSpeed;

    private List<Activity> currentHoveringActivities = new List<Activity>();
    private List<UnityAction> activityActions = new List<UnityAction>();
    public List<CursorActionListObject> existingActionListObjects = new List<CursorActionListObject>();
    private CursorActionListObject currentlyHighlightedListObject;
    private Transform player;
    private Vector3 originalListPosition;
    private Vector3 offsetListPosition;
    private float horizontalListPositionOffset = 24f;
    private int listActionIndex;
    private bool lockCursorState;

    private void Awake()
    {
        if(GameObject.FindWithTag("Player") != null)
            player = GameObject.FindWithTag("Player").transform;
        
        if(player == null)
            Debug.LogError("Player is null");
        
        //UnityEngine.Cursor.visible = false;
        originalListPosition = actionListContainer.localPosition;
        offsetListPosition = originalListPosition;
        offsetListPosition.x -= horizontalListPositionOffset;
        ClearDisplayActionList();
        DisplayCursor(defaultCursorIcon);
    }

    void Update()
    {
        cursorTransform.position = Input.mousePosition;
        scrollSpeed = Input.GetAxis("Mouse ScrollWheel");
        if (CursorState == CursorState.Default)
        {
            
        }
        else if (CursorState == CursorState.DisplayList)
        {
            CheckScrollInput();

            if (Input.GetMouseButtonDown(0) && activityActions.Count > 0) // todo send message if player is out of range & fix invoke
            {
                activityActions[listActionIndex].Invoke();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMouseOverUserInterface.Value == false && !lockCursorState && playerLockVariable.Value == 0)
        {
            HandleHoverOverActivities();
            CheckActivityRanges();
        }
        else ClearAll();
    }

    public void HandleHoverOverActivities()
    {
        var origin = Camera.main.transform.position;
        var mousePos = Input.mousePosition;
        mousePos.z = 1000;
        var dir = Camera.main.ScreenToWorldPoint(mousePos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin, dir, Mathf.Infinity,activityLayerMask);
        Debug.DrawRay(origin,dir,Color.cyan,3f); // todo add debug flag
        
        bool needRefresh = false;
        List<Activity> tempActivityList = new List<Activity>();
        
        for (int i = 0; i < hits.Length; i++)
        {
            if(!hits[i].transform.gameObject.GetComponent<Activity>()) continue;

            Activity activity = hits[i].transform.gameObject.GetComponent<Activity>();
            if (!CheckRange(hits[i].transform.position, activity.rangeRequirement)) return;
            tempActivityList.Add(activity);
            int matchCount = 0;
            for (int j = 0; j < currentHoveringActivities.Count; j++)
            {
                if (currentHoveringActivities[j].Identity == activity.Identity)
                    matchCount++;
            }

            if (matchCount == 0)
                needRefresh = true;
        }

        if (needRefresh || hits.Length == 0 || tempActivityList.Count != currentHoveringActivities.Count)
        {
            currentHoveringActivities.Clear();
            currentHoveringActivities = tempActivityList;
            PopulateActionList();
        }
        CursorState = CursorState.DisplayList;
    }

    private void CheckActivityRanges()
    {
        List<Activity> tempActivityList = new List<Activity>(currentHoveringActivities);
        foreach (var activity in tempActivityList.ToList())
        {
            if (!CheckRange(activity.gameObject.transform.position, activity.rangeRequirement))
            {
                tempActivityList.Remove(activity);
            }
        }

        if (tempActivityList.Count != currentHoveringActivities.Count)
        {
            currentHoveringActivities.Clear();
            currentHoveringActivities = tempActivityList;
            PopulateActionList();
        }
            
    }
    
    private bool CheckRange(Vector3 origin, float rangeRequirement)
    {
        if (player == null) return true;
        return Vector3.Distance(player.position, origin) < rangeRequirement;    
    }

    private void CheckScrollInput()
    {
        int actionCount = activityActions.Count;
        if (actionCount <= 1) return;
        
        int moveIncrement = scrollSpeed == 0 ? 0 : scrollSpeed > 0 ? -1 : 1;
        int newIndex = listActionIndex; 
        newIndex += moveIncrement;
        newIndex = newIndex >= actionCount ? 0 :
            newIndex < 0 ? actionCount - 1 : newIndex;
        
        if (listActionIndex != newIndex)
        {
            listActionIndex = newIndex;
            existingActionListObjects[listActionIndex].ToggleHighlight(true);
        }
    }

    private void PopulateActionList()
    {
        ClearDisplayActionList();
        if (currentHoveringActivities.Count == 0)
        {
            defaultCursor.sprite = defaultCursorIcon;
            return;
        }
        
        if (currentHoveringActivities.Count == 1 && currentHoveringActivities[0].ActivityActions.Count == 1)
        {
            if (SpawnActionListObject(currentHoveringActivities[0].ActivityActions[0], false))
            {
                defaultCursor.sprite = currentHoveringActivities[0].ActivityActions[0].signature.ActionIcon;
            }
        }
        else
        {
            defaultCursor.sprite = listCursorIcon;
            foreach (var activity in currentHoveringActivities)
            {
                foreach (var t in activity.ActivityActions)
                {
                    SpawnActionListObject(t);
                }
            }
        }
        listActionIndex = listActionIndex >= activityActions.Count ? activityActions.Count -1 : listActionIndex < 0 ? 0 : listActionIndex;
        if(existingActionListObjects.Count > 0)
            existingActionListObjects[listActionIndex].ToggleHighlight(true);
    }

    private bool SpawnActionListObject(ActivityAction action, bool useIcon = true)
    {
        // todo send message that player is missing required item
        if (!playerInventory.HasItem(action.signature.RequiredItem) && action.signature.RequiredItem != null)
        {
            return false;
        }
        else if (playerInventory.HasItem(action.signature.RequiredItem) && action.signature.RequiredItem != null)
        {
            if (playerInventory.ItemList[action.signature.RequiredItem] < action.signature.RequiredItemQuantity)
            {
                return false;
            }
        }

        
        
        var listObject = Instantiate(actionListObject, actionListContainer);
        var objectInit = listObject.GetComponent<CursorActionListObject>();
        
        objectInit.Setup(action, useIcon:useIcon);
        activityActions.Add(delegate
        {
            action.eventChannel.Invoke();
            if (action.signature.RequiredItem != null)
            {
                playerInventory.TryUseItem(action.signature.RequiredItem,action.signature.RequiredItemQuantity);
                onMoveOrAddItem.Raise();
            }
        });
        
        existingActionListObjects.Add(objectInit);
        actionListContainer.localPosition = offsetListPosition;
        return true;
    }

    public void ClearAll()
    {
        if(currentHoveringActivities.Count > 0)
            currentHoveringActivities.Clear();
        ClearDisplayActionList();
    }
    
    public void ClearDisplayActionList()
    {
        if(activityActions.Count == 0 && existingActionListObjects.Count == 0) return;
        activityActions.Clear();
        if (existingActionListObjects.Count > 0)
        {
            foreach (var listObject in existingActionListObjects)
            {
                Destroy(listObject.gameObject);
            }
            existingActionListObjects.Clear();
        }
        DisplayCursor(defaultCursorIcon);
    }

    public void DisplayCursor(Sprite incomingCursorIcon)
    {
        CursorState = CursorState.Default;
        defaultCursor.sprite = incomingCursorIcon;
    }

    public void DragItem(InventorySlot incomingItem)
    {
        DisplayCursor(incomingItem.heldItem.Sprite);
        lockCursorState = true;
    }

    public void ReleaseItem()
    {
        lockCursorState = false;
        DisplayCursor(defaultCursorIcon);
    }
}
}

