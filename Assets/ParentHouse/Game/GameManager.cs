using UnityEngine;

public static class GameManager {
    public static GameState CurrentGameState;

    public static void InitGame() {
        CurrentGameState = new BaseUserInterfaceState();
    }
}


public abstract class GameState {
    public bool AllowTimeToProgress;
    public bool AllowPlayerWorldInput;
    public bool AllowPlayerUiInput;

    public virtual void EnterGameState() {
    }

    public virtual void ExitGameState() {
    }
}

public class BaseUserInterfaceState : GameState {
    public override void EnterGameState() {
        base.EnterGameState();
        AllowTimeToProgress = true;
        AllowPlayerWorldInput = false;
        AllowPlayerUiInput = true;
    }
}

public class BaseGameplayState : GameState {
    public override void EnterGameState() {
        base.EnterGameState();
        AllowTimeToProgress = true;
        AllowPlayerWorldInput = true;
        AllowPlayerUiInput = true;
    }
}

public class TransitionToGameplayState : GameState {
    public override void EnterGameState() {
        base.EnterGameState();
        AllowTimeToProgress = false;
        AllowPlayerWorldInput = false;
        AllowPlayerUiInput = false;
    }

    public override void ExitGameState() {
        base.ExitGameState();
        GameManager.CurrentGameState = new BaseGameplayState();
    }
}