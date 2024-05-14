using UnityEngine;

namespace ParentHouse {
    public class AnimationDirectionDemo : MonoBehaviour {
        [SerializeField] private CharacterCustomizer customizer;
        [SerializeField] private Vector2 direction;

        public void SetDirection() {
            customizer.SetCharacterFacingDirection(direction);
        }
    }
}