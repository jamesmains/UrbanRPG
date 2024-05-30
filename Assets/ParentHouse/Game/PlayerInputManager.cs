using System;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ParentHouse.Game {
    public class PlayerInputManager : SerializedMonoBehaviour {
        [SerializeField] [FoldoutGroup("Settings")] [Tooltip("Default InputController")]
        private InputController Input;

        private void OnEnable() {
            ItemLookupTable.GetItem("beccd8d1-8b93-4f2d-b681-71d05a623f70");
            GameEvents.GameStateEntered.AddListener(OnEnterNewGameState);
            // Debug
            GameManager.EnterState(new BaseGameplayState());
        }

        private void OnDisable() {
            GameEvents.GameStateEntered.RemoveListener(OnEnterNewGameState);
        }

        private void OnEnterNewGameState(GameState NewState) {
            Input = NewState.Controller;
        }

        #region Input Callbacks

        public void OnMove(InputValue value) {
            Input.Move(value);
        }

        public void OnNavigate(InputValue value) {
            Input.Navigate(value);
        }

        #endregion
    }

    /// <summary>
    /// Base Input Class, hold functions for all input options.
    /// </summary>
    [Serializable]
    public abstract class InputController {
        public virtual void Move(InputValue Value) {
        }

        public virtual void Navigate(InputValue Value) {
        }
    }

    /// <summary>
    /// Generic Player Controller, let's player navigate 3d world during standard play.
    /// </summary>
    public class PlayerInputController : InputController {
        public override void Move(InputValue Value) {
            Debug.Log($"Controlling Player Via Broadcast: {Value.Get<Vector2>()}");
        }
    }

    /// <summary>
    /// Allows Ui navigation.
    /// </summary>
    public class GenericUserInterfaceController : InputController {
        public override void Navigate(InputValue Value) {
            Debug.Log($"Navigating User Interface: {Value.Get<Vector2>()}");
        }
    }
}