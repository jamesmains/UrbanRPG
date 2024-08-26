using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Menu : MonoBehaviour {

    [SerializeField] [FoldoutGroup("Settings")]
    private Ease EaseType = Ease.OutQuint;

    [SerializeField] [FoldoutGroup("Settings")]
    private float Speed = 0.3f;

    [SerializeField] [FoldoutGroup("Dependencies")] [ReadOnly]
    private CanvasGroup CanvasGroup;

    [SerializeField] [FoldoutGroup("Dependencies")] [ReadOnly]
    private RectTransform Rect;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private Vector2 TargetPosition;

    private void Awake() {
        CanvasGroup = GetComponent<CanvasGroup>();
        Rect = GetComponent<RectTransform>();
    }

    public void Open() {
        
        TargetPosition = Vector2.zero;
        StartCoroutine(DoOpen());
        IEnumerator DoOpen() {
            Tween menuTween = Rect.DOAnchorPos(TargetPosition, Speed)
                .SetEase(EaseType);
            yield return menuTween.WaitForCompletion();
            Activate();
        }
    }

    public void CloseHorizontal(int direction) {
        float destination = Screen.width * (direction > 0 ? 1 : -1);
        print(
            $"All Size Information: Menu Rect: {Rect.sizeDelta}, Screen Width: {Screen.width}, Direction: {direction}, Total:{destination}");
        TargetPosition = new Vector2(destination,0);
        Deactivate();
    }
    
    public void CloseVertical(int direction) {
        float destination = Screen.height * (direction > 0 ? 1 : -1);
        print(
            $"All Size Information: Menu Rect: {Rect.sizeDelta}, Screen Height: {Screen.height}, Direction: {direction}, Total:{destination}");
        TargetPosition = new Vector2(0,destination);
        Deactivate();
    }

    private void Activate() {
        CanvasGroup.blocksRaycasts = true;
    }

    private void Deactivate() {
        CanvasGroup.blocksRaycasts = false;
        StartCoroutine(DoClose());
        IEnumerator DoClose() {
            Tween menuTween = Rect.DOAnchorPos(TargetPosition, Speed)
                .SetEase(EaseType);
            yield return menuTween.WaitForCompletion();
        }
    }
}