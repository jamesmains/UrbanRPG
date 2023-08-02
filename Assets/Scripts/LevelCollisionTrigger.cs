using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCollisionTrigger : CollisionTrigger
{
    private void OnEnable()
    {
        onEnter.AddListener(GameEvents.OnLoadNextScene.Raise);
    }

    private void OnDisable()
    {
        onEnter.RemoveListener(GameEvents.OnLoadNextScene.Raise);
    }
}
