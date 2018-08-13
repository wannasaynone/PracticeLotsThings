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
        AllZombieDied,
        AllShooterDied
    }

    public enum LosingCondition
    {
        AllShooterDied,
        AllZombieDied
    }

    public static Actor Player { get { return m_player; } }
    private static Actor m_player = null;

    public GameSetting GameSetting { get { return m_gameSetting; } }

    private GameSetting m_gameSetting = null;
    private NewGameSetting m_currentNewGameSetting = null;
    private ActorManager m_actorManager = null;

    private WinningCondition m_winningCondition = WinningCondition.AllZombieDied;
    private LosingCondition m_losingCondition = LosingCondition.AllShooterDied;
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
        m_currentNewGameSetting = newGameSetting;
        switch (newGameSetting.startAs)
        {
            case ActorFilter.ActorType.Shooter:
                {
                    m_player = m_actorManager.CreateActor(m_gameSetting.ShooterActorPrefabID);
                    m_winningCondition = WinningCondition.AllZombieDied;
                    m_losingCondition = LosingCondition.AllShooterDied;
                    break;
                }
            case ActorFilter.ActorType.Zombie:
                {
                    m_player = m_actorManager.CreateActor(m_gameSetting.ZombieActorPrefabID);
                    m_winningCondition = WinningCondition.AllShooterDied;
                    m_losingCondition = LosingCondition.AllZombieDied;
                    break;
                }
        }

        m_player.EnableAI(false);
        m_player.OnActorDied += delegate
        {
            Actor _empty = Engine.ActorManager.CreateActor(m_gameSetting.EmptyActorPrefabID, m_player.transform.position);
            CameraController.MainCameraController.Track(_empty.gameObject);
        };
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

        ((GameCommonUIPage)GetViews<GameCommonUIPage>()[0]).Hide();
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
            if(_normal[i].GetCharacterStatus().HP > 0)
            {
                m_normalActorCount++;
            }
        }

        for (int i = 0; i < _shooter.Count; i++)
        {
            if (_shooter[i].GetCharacterStatus().HP > 0)
            {
                m_shooterActorCount++;
            }
        }

        for (int i = 0; i < _zombie.Count; i++)
        {
            if (_zombie[i].GetCharacterStatus().HP > 0)
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
            case WinningCondition.AllShooterDied:
                {
                    if(m_shooterActorCount <= 0)
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
            case LosingCondition.AllShooterDied:
                {
                    if (m_shooterActorCount <= 0)
                    {
                        EndGame(false);
                    }
                    break;
                }
            case LosingCondition.AllZombieDied:
                {
                    if (m_zombieActorCount <= 0)
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
        string _winText = m_winningCondition == WinningCondition.AllShooterDied ? "All Shooter Died\n" : "All Zombie Died\n";
        string _loseText = m_losingCondition == LosingCondition.AllShooterDied ? "All Shooter Died\n" : "All Zombie Died\n";
        string _content = playerWin ? "Game Over\n" + _winText + "You Win" : "Game Over\n" + _loseText + "You Lose";
        ((GameCommonUIPage)GetViews<GameCommonUIPage>()[0]).Show(_content);
    }

}
