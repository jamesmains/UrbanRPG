using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Todo: Make more readable -- pretty hard to sort through
// |-> Either individual files or group by category

namespace Parent_House_Framework.Cores {
    public class JuicyCore : MonoBehaviour {
    }

    [Serializable]
    public abstract class ObjectEffect {
        protected GameObject AssociatedObject;

        public virtual void Initialize(GameObject selfObject) {
            AssociatedObject = selfObject;
        }

        public virtual void HandleState(bool state) {
        }
    }

    [Serializable]
    public class AnimationBoolObjectEffect : ObjectEffect {
        private Animator Anim;
        public string BoolName;

        public override void Initialize(GameObject parentObject) {
            base.Initialize(parentObject);
            Anim = AssociatedObject.GetComponent<Animator>();
        }

        public override void HandleState(bool state) {
            base.HandleState(state);
            Anim.SetBool(BoolName, state);
        }
    }

    [Serializable]
    public class GameObjectActiveObjectEffect : ObjectEffect {

        public override void HandleState(bool state) {
            base.HandleState(state);
            AssociatedObject.SetActive(state);
        }
    }

    [Serializable]
    public class ImageSpriteSwapEffect : ObjectEffect {
        [SerializeField, FoldoutGroup("Settings")]
        private Sprite OnSprite;

        [SerializeField, FoldoutGroup("Settings")]
        private Sprite OffSprite;

        private Image m_Image;

        public override void Initialize(GameObject parentObject) {
            base.Initialize(parentObject);
            m_Image = AssociatedObject.GetComponent<Image>();
        }

        public override void HandleState(bool state) {
            base.HandleState(state);
            m_Image.sprite = state ? OnSprite : OffSprite;
        }
    }

    [Serializable]
    public class SetTextGUIMessageEffect : ObjectEffect {
        [SerializeField, FoldoutGroup("Settings")]
        private string OnMessage;

        [SerializeField, FoldoutGroup("Settings")]
        private string OffMessage;

        private TextMeshProUGUI m_TextMeshProUGUI;

        public override void Initialize(GameObject parentObject) {
            base.Initialize(parentObject);
            m_TextMeshProUGUI = AssociatedObject.GetComponent<TextMeshProUGUI>();
        }

        public override void HandleState(bool state) {
            base.HandleState(state);
            m_TextMeshProUGUI.text = state ? OnMessage : OffMessage;
        }
    }

    [Serializable]
    public class EventTriggerEffect : ObjectEffect {
        [SerializeField, FoldoutGroup("Settings")]
        private UnityEvent OnEvent = new();

        [SerializeField, FoldoutGroup("Settings")]
        private UnityEvent OffEvent = new();

        public override void HandleState(bool state) {
            if (state) {
                OnEvent?.Invoke();
            }
            else {
                OffEvent?.Invoke();
            }
        }
    }

    [Serializable]
    public class CanvasFadeEffect : ObjectEffect {
        [SerializeField, FoldoutGroup("Settings")]
        private Vector2 OpacityRange;

        [SerializeField, FoldoutGroup("Settings")]
        private float Duration;

        private CanvasGroup m_CanvasGroup;

        [Button]
        private void SetAlpha(float alpha) {
            m_CanvasGroup.alpha = alpha;
        }

        public override void Initialize(GameObject parentObject) {
            base.Initialize(parentObject);
            m_CanvasGroup = AssociatedObject.GetComponent<CanvasGroup>();
        }

        public override void HandleState(bool state) {
            base.HandleState(state);
            m_CanvasGroup.DOFade(state ? OpacityRange.y : OpacityRange.x, Duration);
        }
    }

    [Serializable]
    public abstract class TweenEffect : ObjectEffect {
        [SerializeField, FoldoutGroup("Settings")]
        protected Ease EaseIn = Ease.InOutSine;

        [SerializeField, FoldoutGroup("Settings")]
        protected Ease EaseOut = Ease.InOutSine;

        [SerializeField, FoldoutGroup("Settings")]
        protected float Duration;

        [SerializeField, FoldoutGroup("Settings")]
        protected float Overshoot;
    }
    
    [Serializable]
    public class TransformMoveEffect : TweenEffect {
        [SerializeField, FoldoutGroup("Settings")]
        private Vector3 StartPosition;
        
        [SerializeField, FoldoutGroup("Settings")]
        private Vector3 EndPosition;
        
#if UNITY_EDITOR
        [Button]
        public void SetStartPos() {
            StartPosition = AssociatedObject.transform.localPosition;
        }

        [Button]
        public void SetEndPos() {
            EndPosition = AssociatedObject.transform.localPosition;
        }
#endif

        public override void HandleState(bool state) {
            base.HandleState(state);
            AssociatedObject.transform.DOLocalMove(state ? EndPosition : StartPosition, Duration)
                .SetEase(state ? EaseIn : EaseOut, Overshoot);
        }
    }
    
    [Serializable]
    public class TransformRotateEffect : TweenEffect {
        [SerializeField, FoldoutGroup("Settings")]
        private RotateMode RotateMode;

        [SerializeField, FoldoutGroup("Settings")]
        private Vector3 StartRotation = Vector3.zero;

        [SerializeField, FoldoutGroup("Settings")]
        private Vector3 EndRotation = Vector3.one;

#if UNITY_EDITOR
        [Button]
        public void SetStartRotation() {
            // Todo: Test that this gets the actual value needed
            StartRotation = AssociatedObject.transform.eulerAngles;
        }

        [Button]
        public void SetEndRotation() {
            // Todo: Test that this gets the actual value needed
            EndRotation = AssociatedObject.transform.eulerAngles;
        }
#endif
        
        public override void HandleState(bool state) {
            base.HandleState(state);
            AssociatedObject.transform.DORotate(state ? EndRotation : StartRotation, Duration, RotateMode)
                .SetEase(state ? EaseIn : EaseOut, Overshoot);
        }
    }
    
    [Serializable]
    public class TransformScaleEffect : TweenEffect {
        [SerializeField, FoldoutGroup("Settings")]
        private Vector3 MinSize = Vector3.zero;

        [SerializeField, FoldoutGroup("Settings")]
        private Vector3 MaxSize = Vector3.one;
#if UNITY_EDITOR
        [Button]
        public void SetMinSize() {
            MinSize = AssociatedObject.transform.localScale;
        }

        [Button]
        public void SetMaxSize() {
            MaxSize = AssociatedObject.transform.localScale;
        }
#endif
        public override void HandleState(bool state) {
            base.HandleState(state);
            AssociatedObject.transform.DOScale(state ? MaxSize : MinSize, Duration)
                .SetEase(state ? EaseIn : EaseOut, Overshoot);
        }
    }

    
    [Serializable]
    public abstract class RectTweenEffect : TweenEffect {
        protected RectTransform m_RectTransform;

        public override void Initialize(GameObject parentObject) {
            base.Initialize(parentObject);
            m_RectTransform = AssociatedObject.GetComponent<RectTransform>();
        }
    }

    [Serializable]
    public class RectTransformScaleEffect : RectTweenEffect {
        [SerializeField, FoldoutGroup("Settings")]
        private Vector3 MinSize = Vector3.zero;

        [SerializeField, FoldoutGroup("Settings")]
        private Vector3 MaxSize = Vector3.one;
#if UNITY_EDITOR
        [Button]
        public void SetMinSize() {
            MinSize = m_RectTransform.localScale;
        }

        [Button]
        public void SetMaxSize() {
            MaxSize = m_RectTransform.localScale;
        }
#endif
        public override void HandleState(bool state) {
            base.HandleState(state);
            m_RectTransform.DOScale(state ? MaxSize : MinSize, Duration)
                .SetEase(state ? EaseIn : EaseOut, Overshoot);
        }
    }

    [Serializable]
    public class RectTransformRotateEffect : RectTweenEffect {
        [SerializeField, FoldoutGroup("Settings")]
        private RotateMode RotateMode;

        [SerializeField, FoldoutGroup("Settings")]
        private Vector3 StartRotation = Vector3.zero;

        [SerializeField, FoldoutGroup("Settings")]
        private Vector3 EndRotation = Vector3.one;

#if UNITY_EDITOR
        [Button]
        public void SetStartRotation() {
            // Todo: Test that this gets the actual value needed
            StartRotation = m_RectTransform.eulerAngles;
        }

        [Button]
        public void SetEndRotation() {
            // Todo: Test that this gets the actual value needed
            EndRotation = m_RectTransform.eulerAngles;
        }
#endif
        
        public override void HandleState(bool state) {
            base.HandleState(state);
            m_RectTransform.DORotate(state ? EndRotation : StartRotation, Duration, RotateMode)
                .SetEase(state ? EaseIn : EaseOut, Overshoot);
        }
    }

    [Serializable]
    public class RectTransformMoveEffect : RectTweenEffect {
        [SerializeField, FoldoutGroup("Settings")]
        private Vector3 StartPosition;
        
        [SerializeField, FoldoutGroup("Settings")]
        private Vector3 EndPosition;
        
#if UNITY_EDITOR
        [Button]
        public void SetStartPos() {
            StartPosition = m_RectTransform.anchoredPosition;
        }

        [Button]
        public void SetEndPos() {
            EndPosition = m_RectTransform.anchoredPosition;
        }
#endif

        public override void HandleState(bool state) {
            base.HandleState(state);
            m_RectTransform.DOAnchorPos3D(state ? EndPosition : StartPosition, Duration)
                .SetEase(state ? EaseIn : EaseOut, Overshoot);
        }
    }
}