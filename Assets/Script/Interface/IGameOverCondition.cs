using System;

namespace PracticeLotsThings.Manager
{
    public interface IGameOverCondition
    {
        bool IsGameOver(Action onIsPlayerWin, Action onIsPlayerLose);
    }
}
