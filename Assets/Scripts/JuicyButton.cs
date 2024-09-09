using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JuicyButton : SerializedMonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler,
    IPointerExitHandler {
#if UNITY_EDITOR

    [SerializeField] [FoldoutGroup("Settings")]
    private bool UseLabelToRenameObject;

    [SerializeField] [FoldoutGroup("Settings")]
    private string Label;

    private void OnValidate() {
        if (UseLabelToRenameObject && gameObject.name != $"Btn_{Label}") {
            gameObject.name = $"Btn_{Label}";
        }
    }

#endif

    [SerializeField] [FoldoutGroup("Settings")]
    private bool CanToggle;

    [SerializeField] [FoldoutGroup("Settings")]
    private bool StartOn;

    [SerializeField] [FoldoutGroup("Settings")]
    private bool LockIfOn;

    [SerializeField] [FoldoutGroup("Settings")]
    private bool AllowReleaseEventsOnClick;

    [SerializeField] [FoldoutGroup("Settings")]
    private bool PlayReleaseEffectsOnClick;

    [SerializeField] [FoldoutGroup("Settings")]
    private bool RequireHoldTime;

    [SerializeField] [FoldoutGroup("Settings")]
    private float HoldTime;

    [SerializeField] [FoldoutGroup("Settings")]
    private bool ForceReleaseWhenHeldTimeIsMet;

    [SerializeField] [FoldoutGroup("Settings")]
    private List<JuicyButtonEffect> PointerUpEffects = new();

    [SerializeField] [FoldoutGroup("Settings")]
    private List<JuicyButtonEffect> PointerDownEffects = new();

    [SerializeField] [FoldoutGroup("Settings")]
    private List<JuicyButtonEffect> PointerClickEffects = new();

    [SerializeField] [FoldoutGroup("Settings")]
    private List<JuicyButtonEffect> PointerExitEffects = new();

    [SerializeField] [FoldoutGroup("Settings")]
    private List<JuicyButtonEffect> ToggleOnEffects = new();

    [SerializeField] [FoldoutGroup("Settings")]
    private List<JuicyButtonEffect> ToggleOffEffects = new();

    [SerializeField] [FoldoutGroup("Events")]
    public UnityEvent OnButtonUp;

    [SerializeField] [FoldoutGroup("Events")]
    public UnityEvent OnButtonDown;

    [SerializeField] [FoldoutGroup("Events")]
    public UnityEvent OnButtonClick;

    [SerializeField] [FoldoutGroup("Events")]
    public UnityEvent OnButtonExit;

    [SerializeField] [FoldoutGroup("Events")]
    private UnityEvent OnToggleOn = new();

    [SerializeField] [FoldoutGroup("Events")]
    private UnityEvent OnToggleOff = new();

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private bool IsOn;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private bool IsMouseOver;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private float StartHoldTime;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private bool IsHolding;

    private void OnEnable() {
        if (!RequireHoldTime) {
            ForceReleaseWhenHeldTimeIsMet = false;
        }

        if (StartOn) {
            ForceToggleState(true);
        }
    }

    private Coroutine ForceReleaseCoroutine;

    public void OnPointerEnter(PointerEventData eventData) {
        print("Mouse Enter");
        IsMouseOver = true;
    }

    public void OnPointerDown(PointerEventData eventData) {
        print("Mouse Down");
        OnButtonDown.Invoke();
        IsHolding = true;
        if (RequireHoldTime) {
            StartHoldTime = Time.time;
        }

        if (ForceReleaseWhenHeldTimeIsMet) {
            ForceReleaseCoroutine = StartCoroutine(ForceReleaseDelay());

            IEnumerator ForceReleaseDelay() {
                yield return new WaitForSeconds(HoldTime);
                Release();
            }
        }

        foreach (var downEffect in PointerDownEffects) {
            downEffect.Play();
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (IsHolding)
            Release();
    }

    private void Release() {
        IsHolding = false;

        if (ForceReleaseCoroutine != null) {
            StopCoroutine(ForceReleaseCoroutine);
            ForceReleaseCoroutine = null;
        }

        if (!IsMouseOver || (IsMouseOver && AllowReleaseEventsOnClick))
            OnButtonUp.Invoke();
        if (!IsMouseOver || (IsMouseOver && PlayReleaseEffectsOnClick))
            foreach (var upEffect in PointerUpEffects) {
                upEffect.Play();
            }

        if (!IsMouseOver) return;
        if (RequireHoldTime && Time.time < StartHoldTime + HoldTime) return;

        Click();
    }

    private void Click() {
        if (LockIfOn && IsOn) return;
        if (CanToggle) {
            ForceToggleState(!IsOn);
        }

        OnButtonClick.Invoke();
        foreach (var clickEffect in PointerClickEffects) {
            clickEffect.Play();
        }
    }

    public void ForceToggleState(bool state) {
        IsOn = state;
        if (IsOn) {
            OnToggleOn.Invoke();
            foreach (var onEffect in ToggleOnEffects) {
                onEffect.Play();
            }
        }
        else {
            OnToggleOff.Invoke();
            foreach (var onEffect in ToggleOffEffects) {
                onEffect.Play();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        IsMouseOver = false;
        OnButtonExit.Invoke();
        foreach (var exitEffect in PointerExitEffects) {
            exitEffect.Play();
        }
    }
}

[Serializable]
public abstract class JuicyButtonEffect {
    public virtual void Play() {
    }
}

public class SpriteChangeJuciyButtonChangeEffect : JuicyButtonEffect {
    [SerializeField] private Sprite EffectSprite;
    [SerializeField] private Image TargetImage;

    public override void Play() {
        TargetImage.sprite = EffectSprite;
    }
}

public abstract class TweenJuicyButtonEffect : JuicyButtonEffect {
    [SerializeField] [FoldoutGroup("Settings")]
    protected Ease EaseType = Ease.InOutSine;

    [SerializeField] [FoldoutGroup("Settings")]
    protected float Speed = 1;

    [SerializeField] [FoldoutGroup("Settings")]
    protected float Overshoot;

    [SerializeField] [FoldoutGroup("Dependencies")]
    protected RectTransform Rect;
}

public class PulseJuicyButtonEffect : TweenJuicyButtonEffect {
    [SerializeField] [FoldoutGroup("Settings")]
    private Vector3 PulseEndSize;

    public override void Play() {
        Rect.DOScale(PulseEndSize, Speed)
            .SetEase(EaseType, Overshoot);
    }
}

public class RotateJuicyButtonEffect : TweenJuicyButtonEffect {
    [SerializeField] [FoldoutGroup("Settings")]
    private RotateMode RotateMode;

    [SerializeField] [FoldoutGroup("Settings")]
    private Vector3 TargetAngle;

    public override void Play() {
        Rect.DORotate(TargetAngle, Speed, RotateMode)
            .SetEase(EaseType, Overshoot);
    }
}

public class MoveJuicyButtonEffect : TweenJuicyButtonEffect {
    [SerializeField] [FoldoutGroup("Settings")]
    private Vector3 TargetPosition;

    public override void Play() {
        Rect.DOAnchorPos3D(TargetPosition, Speed)
            .SetEase(EaseType, Overshoot);
    }
}