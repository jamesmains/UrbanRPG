using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHoverHighlighter : MonoBehaviour
{
    [SerializeField] private Transform decalTopRight;
    [SerializeField] private Transform decalBotRight;
    [SerializeField] private Transform decalBotLeft;
    [SerializeField] private Transform decalTopLeft;
    [SerializeField] private GameObject[] decals;
    private SpriteRenderer _currentSpriteRenderer;

    private void Awake()
    {
        Clear();
    }

    public void HoverOver(SpriteRenderer incomingSpriteRenderer)
    {
        if (incomingSpriteRenderer == null)
            Clear();
        if (_currentSpriteRenderer != incomingSpriteRenderer)
        {
            _currentSpriteRenderer = incomingSpriteRenderer;
            transform.position = incomingSpriteRenderer.transform.position;
            var baseRot = incomingSpriteRenderer.transform.rotation.eulerAngles;
            this.transform.rotation = Quaternion.Euler(baseRot);
            foreach (var decal in decals)
            {
                decal.SetActive(true);
                var rot = incomingSpriteRenderer.transform.rotation.eulerAngles;
                decal.transform.rotation = Quaternion.Euler(rot);
            }
            
            
            var bounds = _currentSpriteRenderer.sprite.bounds;
            
            decalTopRight.position = _currentSpriteRenderer.transform.TransformPoint(new Vector3(bounds.max.x,bounds.max.y, 0));
            decalBotLeft.position = _currentSpriteRenderer.transform.TransformPoint(new Vector3(bounds.min.x,bounds.min.y, 0));
            
            decalBotRight.position = _currentSpriteRenderer.transform.TransformPoint(new Vector3(bounds.max.x,bounds.min.y, 0));
            decalTopLeft.position = _currentSpriteRenderer.transform.TransformPoint(new Vector3(bounds.min.x, bounds.max.y, 0));
            
            var finalRotation = Vector3.zero;
            finalRotation.x = 90f;
            this.transform.rotation = Quaternion.Euler(finalRotation);            
        }
        else return;
    }

    private void Clear()
    {
        _currentSpriteRenderer = null;
        foreach(var decal in decals)
            decal.SetActive(false);
    }
}
