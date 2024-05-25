using ParentHouse.Utils;

namespace ParentHouse.Game {
    public static class GameManager {
        public static GameState CurrentGameState;

        public static void InitGame() {
            CurrentGameState = new BaseUserInterfaceState();
        }

        public static void EnterState(GameState NewState) {
            CurrentGameState = NewState;
            CurrentGameState.EnterGameState();
        }
    }

    /// <summary>
    /// Game State classes
    /// </summary>
    public abstract class GameState {
        public InputController Controller;

        public virtual void EnterGameState() {
            GameEvents.GameStateEntered.Invoke(this);
        }

        public virtual void ExitGameState() {
            GameEvents.GameStateExited.Invoke(this);
        }
    }

    public class BaseUserInterfaceState : GameState {
    }

    public class BaseGameplayState : GameState {
        public override void EnterGameState() {
            Controller = new PlayerInputController();
            base.EnterGameState();
        }
    }

    public class TransitionToGameplayState : GameState {
        public override void ExitGameState() {
            base.ExitGameState();
            GameManager.CurrentGameState = new BaseGameplayState();
        }
    }
}