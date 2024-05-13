using UnityEngine;
using UnityEngine.UI;

namespace ParentHouse.UI {
    public class FoldoutDisplay : MonoBehaviour
    {
        [SerializeField] protected RectTransform displayRect;
        [SerializeField] protected RectTransform foldoutRect;
        [SerializeField] protected float openSpeed;
        protected bool isOpen;
        protected Vector2 openSize;
        protected Vector2 closedSize;
        protected static FoldoutDisplay openedDisplay;
    
    
        protected virtual void Update()
        {
            if (isOpen && displayRect.sizeDelta != openSize)
            {
                var size = displayRect.sizeDelta;
                size.y = Mathf.Lerp(size.y, openSize.y, openSpeed * Time.deltaTime);
                displayRect.sizeDelta = size;
            }
            else if (!isOpen && displayRect.sizeDelta != closedSize)
            {
                var size = displayRect.sizeDelta;
                size.y = Mathf.Lerp(size.y, closedSize.y, openSpeed * Time.deltaTime);
                displayRect.sizeDelta = size;
            }
        }

        protected virtual void SetOpenSize()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(foldoutRect);
            openSize = new Vector2(displayRect.rect.height, 58 + foldoutRect.sizeDelta.y);
        }
    
        public virtual void ToggleDisplay()
        {
            if(openedDisplay == null)
                openedDisplay = this;
        
            if(openedDisplay != null && openedDisplay != this)
            {
                openedDisplay.CloseFoldout();
            }
        
            openedDisplay = this;
        
            if (isOpen)
            {
                CloseFoldout();
            }
            else
            {
                OpenFoldout();
            }
        }

        public virtual void OpenFoldout()
        {
            SetOpenSize();
            isOpen = true;
        }

        public virtual void CloseFoldout()
        {
            isOpen = false;
        }
    }
}
