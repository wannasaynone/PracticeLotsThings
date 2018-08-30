using System;

public interface IGameOverCondition {

    bool IsGameOver(Action onIsPlayerWin, Action onIsPlayerLose);

}
