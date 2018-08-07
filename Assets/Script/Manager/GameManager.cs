using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager {

    public enum GameState
    {
        None,
        Running,
        End
    }

    public enum WinningCondition
    {
        AllZombieDied
    }

    public enum LosingCondition
    {
        AllShooterDieds
    }

    public static Actor Player { get { return m_player; } }
    private static Actor m_player = null;

    private GameSetting m_gameSetting = null;
    private ActorManager m_actorManager = null;

    private WinningCondition m_winningCondition = WinningCondition.AllZombieDied;
    private LosingCondition m_losingCondition = LosingCondition.AllShooterDieds;
    private int m_normalActorCount = 0;
    private int m_shooterActorCount = 0;
    private int m_zombieActorCount = 0;
    private GameState m_gameState = GameState.None;

    public GameManager(ActorManager actorManager, GameSetting gameSetting)
    {
        m_actorManager = actorManager;
        m_gameSetting = gameSetting;
        m_gameState = GameState.None;
    }

    public void InitGame(NewGameSetting newGameSetting)
    {
        switch(newGameSetting.startAs)
        {
            case ActorFilter.ActorType.Shooter:
                {
                    m_player = m_actorManager.CreateActor(m_gameSetting.ShooterActorPrefabID);
                    break;
                }
            case ActorFilter.ActorType.Zombie:
                {
                    m_player = m_actorManager.CreateActor(m_gameSetting.ZombieActorPrefabID);
                    break;
                }
        }

        m_player.EnableAI(false);
        CameraController.MainCameraController.Track(m_player.gameObject);

        // TODO: network...
        Actor _actor = null;
        switch (newGameSetting.gameType)
        {
            case NewGameSetting.GameType.OneVsOne:
                {
                    switch (newGameSetting.startAs)
                    {
                        case ActorFilter.ActorType.Shooter:
                            {
                                _actor = m_actorManager.CreateActor(m_gameSetting.ZombieActorPrefabID, Engine.GetRamdomPosition());
                                _actor.EnableAI(true);
                                break;
                            }
                        case ActorFilter.ActorType.Zombie:
                            {
                                _actor = m_actorManager.CreateActor(m_gameSetting.ShooterActorPrefabID, Engine.GetRamdomPosition());
                                _actor.EnableAI(true);
                                break;
                            }
                    }
                    break;
                }
                // TODO: AI setting when 2V2 (need to write conditions...)
            case NewGameSetting.GameType.TwoVsTwo:
                {
                    switch (newGameSetting.startAs)
                    {
                        case ActorFilter.ActorType.Shooter:
                            {
                                _actor = m_actorManager.CreateActor(m_gameSetting.ZombieActorPrefabID, Engine.GetRamdomPosition());
                                _actor.EnableAI(true);

                                _actor = m_actorManager.CreateActor(m_gameSetting.ZombieActorPrefabID, Engine.GetRamdomPosition());
                                _actor.EnableAI(true);

                                _actor = m_actorManager.CreateActor(m_gameSetting.ShooterActorPrefabID, Engine.GetRamdomPosition());
                                _actor.EnableAI(true);
                                break;
                            }
                        case ActorFilter.ActorType.Zombie:
                            {
                                _actor = m_actorManager.CreateActor(m_gameSetting.ShooterActorPrefabID, Engine.GetRamdomPosition());
                                _actor.EnableAI(true);

                                _actor = m_actorManager.CreateActor(m_gameSetting.ShooterActorPrefabID, Engine.GetRamdomPosition());
                                _actor.EnableAI(true);

                                _actor = m_actorManager.CreateActor(m_gameSetting.ZombieActorPrefabID, Engine.GetRamdomPosition());
                                _actor.EnableAI(true);
                                break;
                            }
                    }
                    break;
                }
        }

        for(int i = 0; i < newGameSetting.normalActorNumber; i++)
        {
            Actor _normalActor = m_actorManager.CreateActor(m_gameSetting.NormalActorPrefabID, Engine.GetRamdomPosition());
            TimerManager.Schedule(Random.Range(0f, 0.5f) * i, delegate { _normalActor.EnableAI(true); });
        }

        m_gameState = GameState.Running;
    }

    public void CheckGame()
    {
        if (m_gameState != GameState.Running)
        {
            return;
        }

        m_normalActorCount = 0;
        m_shooterActorCount = 0;
        m_zombieActorCount = 0;

        List<Actor> _normal = Engine.ActorFilter.GetActors(new ActorFilter.FilteCondition() { filteBy = ActorFilter.FilteBy.Type, compareCondition = ActorFilter.CompareCondition.Is, actorType = ActorFilter.ActorType.Normal });
        List<Actor> _shooter = Engine.ActorFilter.GetActors(new ActorFilter.FilteCondition() { filteBy = ActorFilter.FilteBy.Type, compareCondition = ActorFilter.CompareCondition.Is, actorType = ActorFilter.ActorType.Shooter });
        List<Actor> _zombie = Engine.ActorFilter.GetActors(new ActorFilter.FilteCondition() { filteBy = ActorFilter.FilteBy.Type, compareCondition = ActorFilter.CompareCondition.Is, actorType = ActorFilter.ActorType.Zombie });

        for(int i = 0; i < _normal.Count; i++)
        {
            if(Engine.ActorManager.GetCharacterStatus(_normal[i]).HP > 0)
            {
                m_normalActorCount++;
            }
        }

        for (int i = 0; i < _shooter.Count; i++)
        {
            if (Engine.ActorManager.GetCharacterStatus(_shooter[i]).HP > 0)
            {
                m_shooterActorCount++;
            }
        }

        for (int i = 0; i < _zombie.Count; i++)
        {
            if (Engine.ActorManager.GetCharacterStatus(_zombie[i]).HP > 0)
            {
                m_zombieActorCount++;
            }
        }

        CheckWinningCondition();
        CheckLosingConiditon();
    }

    private void CheckWinningCondition()
    {
        switch(m_winningCondition)
        {
            case WinningCondition.AllZombieDied:
                {
                    if(m_zombieActorCount <= 0)
                    {
                        EndGame(true);
                    }
                    break;
                }
        }
    }

    private void CheckLosingConiditon()
    {
        switch (m_losingCondition)
        {
            case LosingCondition.AllShooterDieds:
                {
                    if (m_shooterActorCount <= 0)
                    {
                        EndGame(false);
                    }
                    break;
                }
        }
    }

    private void EndGame(bool playerWin)
    {
        if (m_gameState == GameState.End)
        {
            return;
        }

        m_gameState = GameState.End;
        string _content = playerWin ? "Game Over\nYou Win\nScore:" + m_normalActorCount : "Game Over\nYou Lose";
        ((GameCommonUIPage)GetViews<GameCommonUIPage>()[0]).Show(_content);
    }

}
