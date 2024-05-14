using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ParentHouse {
    public class MouseInteractionEffects : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler {
        [ShowInInspector] [OdinSerialize] public List<HoverEffect> Effects { get; set; } = new();

        private void Awake() {
            foreach (var effect in Effects) effect.Init();
        }

        public void OnPointerDown(PointerEventData eventData) {
            foreach (var effect in Effects) effect.OnMouseDown();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            foreach (var effect in Effects) effect.OnMouseEnter();
        }

        public void OnPointerExit(PointerEventData eventData) {
            foreach (var effect in Effects) effect.OnMouseExit();
        }

        public void OnPointerUp(PointerEventData eventData) {
            foreach (var effect in Effects) effect.OnMouseUp();
        }
    }

    [Serializable]
    public abstract class HoverEffect {
        public abstract void Init();
        public abstract void OnMouseUp();
        public abstract void OnMouseDown();
        public abstract void OnMouseEnter();
        public abstract void OnMouseExit();
    }

    public class PopEffect : HoverEffect {
        protected GameObject cachedGameObject;

        [SerializeField] [FoldoutGroup("Debug")] [ReadOnly]
        private Vector2 cachedStartPosition;

        [SerializeField] private Vector2 imagePopDistance;

        [SerializeField] [FoldoutGroup("Debug")] [ReadOnly]
        private bool isMouseOver;

        [SerializeField] private RectTransform targetImage;

        public override void Init() {
            cachedGameObject = targetImage.gameObject;
            cachedStartPosition = targetImage.anchoredPosition;
        }

        public override void OnMouseUp() {
            if (!isMouseOver) return;
            targetImage.anchoredPosition = cachedStartPosition + imagePopDistance;
        }

        public override void OnMouseDown() {
            if (!isMouseOver) return;
            targetImage.anchoredPosition = cachedStartPosition;
        }

        public override void OnMouseEnter() {
            isMouseOver = true;
            targetImage.anchoredPosition = cachedStartPosition + imagePopDistance;
        }

        public override void OnMouseExit() {
            isMouseOver = false;
            targetImage.anchoredPosition = cachedStartPosition;
        }
    }

    public class ShadowPopEffect : PopEffect {
        [SerializeField] [FoldoutGroup("Debug")] [ReadOnly]
        private Shadow shadow;

        [SerializeField] private Vector2 shadowPopDistance;

        public override void Init() {
            base.Init();
            if (cachedGameObject.GetComponent<Shadow>() == null) cachedGameObject.AddComponent<Shadow>();

            shadow = cachedGameObject.GetComponent<Shadow>();
            shadow.effectDistance = Vector2.zero;
        }

        public override void OnMouseUp() {
            base.OnMouseUp();
            shadow.effectDistance = shadowPopDistance;
        }

        public override void OnMouseDown() {
            base.OnMouseDown();
            shadow.effectDistance = Vector2.zero;
        }

        public override void OnMouseEnter() {
            base.OnMouseEnter();
            shadow.effectDistance = shadowPopDistance;
        }

        public override void OnMouseExit() {
            base.OnMouseExit();
            shadow.effectDistance = Vector2.zero;
        }
    }

    public class ChangeTextEffect : HoverEffect {
        [SerializeField] private string message;

        [SerializeField] private TextMeshProUGUI targetTextObject;
        [SerializeField] private string targetTextObjectTag;

        public ChangeTextEffect(TextMeshProUGUI display, string incomingMessage) {
            if (display == null) {
                Debug.LogError("No Display for ChangeTextEffect");
                return;
            }

            display.text = "";
            targetTextObject = display;
            message = incomingMessage;
        }

        public override void Init() {
            if (targetTextObject == null && !string.IsNullOrEmpty(targetTextObjectTag))
                targetTextObject = GameObject.FindWithTag(targetTextObjectTag).GetComponent<TextMeshProUGUI>();
            targetTextObject.text = "";
        }

        public override void OnMouseUp() {
        }

        public override void OnMouseDown() {
        }

        public override void OnMouseEnter() {
            targetTextObject.text = message;
        }

        public override void OnMouseExit() {
            targetTextObject.text = "";
        }
    }

    public class ImageToggleEngableEffect : HoverEffect {
        [SerializeField] private Image image;

        public override void Init() {
            image.enabled = false;
        }

        public override void OnMouseUp() {
        }

        public override void OnMouseDown() {
        }

        public override void OnMouseEnter() {
            image.enabled = true;
        }

        public override void OnMouseExit() {
            image.enabled = false;
        }
    }
}