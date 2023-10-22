using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDirectionDemo : MonoBehaviour
{
    [SerializeField] private CharacterCustomizer customizer;
    [SerializeField] private Vector2 direction;

    public void SetDirection()
    {
        customizer.SetCharacterFacingDirection(direction);
    }
}
