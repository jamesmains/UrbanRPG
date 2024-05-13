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
        [SerializeField] private RectTransform cursorRect;
        [SerializeField] private Animator animator;
        [SerializeField] private Image heldItemImage;

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        private void Awake()
        {
            UnityEngine.Cursor.visible = false;
        }

        void Update()
        {
            cursorRect.position = Input.mousePosition;
        }

        public void TryClick()
        {
            animator.SetTrigger("OnClick");
        }

        public void DragItem(Item incomingItem)
        {
            heldItemImage.gameObject.SetActive(true);
            heldItemImage.sprite = incomingItem.Sprite;
            animator.SetBool("IsHolding",true);
            animator.SetTrigger("OnHold");
        }

        public void ReleaseItem()
        {
            heldItemImage.sprite = null;
            heldItemImage.gameObject.SetActive(false);
            animator.SetBool("IsHolding",false);
        }
    }
}

