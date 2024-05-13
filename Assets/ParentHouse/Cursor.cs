using UnityEngine;
using UnityEngine.UI;

namespace ParentHouse
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

