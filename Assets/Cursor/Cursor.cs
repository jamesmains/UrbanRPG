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
    [SerializeField] [FoldoutGroup("Data")] private RectTransform cursorTransform;
    [SerializeField] [FoldoutGroup("Data")] private IntVariable playerLockVariable;
    [SerializeField] [FoldoutGroup("Data")] private BoolVariable isMouseOverUserInterface;
    [SerializeField] [FoldoutGroup("Data")] private VectorVariable playerPositionVariable;
    [SerializeField] [FoldoutGroup("Data")] private CursorState CursorState;
    [SerializeField] [FoldoutGroup("Data")] private float scrollSpeed;
    
    [SerializeField] [FoldoutGroup("Details")] private Image cursorDisplay;
    [SerializeField] [FoldoutGroup("Details")] private Sprite defaultCursorIcon;
    [SerializeField] [FoldoutGroup("Details")] private Sprite listCursorIcon;

    [SerializeField] [FoldoutGroup("Action List")] private LayerMask activityLayerMask;
    [SerializeField] [FoldoutGroup("Action List")] private RectTransform actionListContainer;
    [SerializeField] [FoldoutGroup("Action List")] private GameObject actionListObject;

    [SerializeField] [FoldoutGroup("Events")] private GameEvent onMoveOrAddItem;

    #region Local Variables
    
    private List<Activity> currentHoveringActivities = new List<Activity>();
    private List<UnityAction> activityActions = new List<UnityAction>();
    public List<CursorActionListObject> existingActionListObjects = new List<CursorActionListObject>();
    private CursorActionListObject currentlyHighlightedListObject;
    private Vector3 originalListPosition;
    private Vector3 offsetListPosition;
    private float horizontalListPositionOffset = 24f;
    private int listActionIndex;
    private bool lockCursorState;
    
    #endregion

    private void Awake()
    {
        UnityEngine.Cursor.visible = false;
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

        if (hits.Length == 0)
        {
            cursorDisplay.sprite = defaultCursorIcon;
            return;
        }
        
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

        if (currentHoveringActivities.Count > 0)
        {
            CursorState = CursorState.DisplayList;
        }
        else
        {
            CursorState = CursorState.Default;
        }
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
        if (playerPositionVariable == null) return true;
        return Vector3.Distance(playerPositionVariable.Value, origin) < rangeRequirement;    
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
            cursorDisplay.sprite = defaultCursorIcon;
            return;
        }
        
        if (currentHoveringActivities.Count == 1 && currentHoveringActivities[0].ActivityActions.Count == 1)
        {
            if (SpawnActionListObject(currentHoveringActivities[0].ActivityActions[0], false))
            {
                cursorDisplay.sprite = currentHoveringActivities[0].ActivityActions[0].signature.ActionIcon;
            }
        }
        else
        {
            cursorDisplay.sprite = listCursorIcon;
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
        else
        {
            CursorState = CursorState.Default;
            DisplayCursor(defaultCursorIcon);
        }
    }

    private bool SpawnActionListObject(ActivityAction action, bool useIcon = true)
    {
        if (!action.signature.IsConditionMet()) return false;

        var listObject = Instantiate(actionListObject, actionListContainer);
        var objectInit = listObject.GetComponent<CursorActionListObject>();
        
        objectInit.Setup(action, useIcon:useIcon);
        activityActions.Add(delegate
        {
            action.eventChannel.Invoke();
            action.signature.TryUseItems();
            onMoveOrAddItem.Raise();
            PopulateActionList();
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
        cursorDisplay.sprite = incomingCursorIcon;
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

