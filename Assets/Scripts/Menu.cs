using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public enum MenuState {
    Closed,
    Open
}

[RequireComponent(typeof(CanvasGroup))]
public class Menu : MonoBehaviour {
    [SerializeField] [FoldoutGroup("Settings")]
    private Vector2 OpenPosition;

    [SerializeField] [FoldoutGroup("Settings")]
    private Vector2 ClosePosition;
    
    [SerializeField] [FoldoutGroup("Settings")]
    private Ease EaseType = Ease.OutQuint;

    [SerializeField] [FoldoutGroup("Settings")]
    private float Speed = 0.3f;

    [SerializeField] [FoldoutGroup("Dependencies")] [ReadOnly]
    private CanvasGroup CanvasGroup;

    [SerializeField] [FoldoutGroup("Dependencies")] [ReadOnly]
    private RectTransform Rect;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    public MenuState State;
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private Vector2 TargetPosition;

    private void Awake() {
        CanvasGroup = GetComponent<CanvasGroup>();
        Rect = GetComponent<RectTransform>();
    }

    public void Open() {
        TargetPosition = OpenPosition;
        StartCoroutine(DoOpen());
        IEnumerator DoOpen() {
            Tween menuTween = Rect.DOAnchorPos(TargetPosition, Speed)
                .SetEase(EaseType);
            yield return menuTween.WaitForCompletion();
            Activate();
        }
    }

    public void Close() {
        TargetPosition = ClosePosition;
        Deactivate();
    }

    private void Activate() {
        CanvasGroup.blocksRaycasts = true;
        State = MenuState.Open;
    }

    private void Deactivate() {
        CanvasGroup.blocksRaycasts = false;
        State = MenuState.Closed;
        StartCoroutine(DoClose());
        IEnumerator DoClose() {
            Tween menuTween = Rect.DOAnchorPos(TargetPosition, Speed)
                .SetEase(EaseType);
            yield return menuTween.WaitForCompletion();
        }
    }
}