using System;
using UnityEngine;

public class GameManager : Manager {

    public const string GAME_VERSION = "V0.31";

    public enum GameState
    {
        None,
        Running,
        End
    }

    public static Actor Player { get { return m_player; } }
    private static Actor m_player = null;

    public GameSetting GameSetting { get { return m_gameSetting; } }

    private GameSetting m_gameSetting = null;
    private NewGameSetting m_currentNewGameSetting = null;
    private ActorManager m_actorManager = null;

    private GameState m_gameState = GameState.None;
    private IGameOverCondition m_gameOverCondition;

    private int m_playerActorPhotonViewID = -1;

    public GameManager(ActorManager actorManager, GameSetting gameSetting)
    {
        m_actorManager = actorManager;
        m_gameSetting = gameSetting;
        m_gameState = GameState.None;
        PhotonEventSender.OnActorCreated += OnActorCreated;
    }

    public void InitGame(NewGameSetting newGameSetting)
    {
        if(m_gameState != GameState.None)
        {
            return;
        }
        Debug.Log("init game, newGameSetting.gameType=" + newGameSetting.gameType + ", newGameSetting.startAs=" + newGameSetting.startAs);
        Debug.Log("NetworkManager.IsOffline=" + NetworkManager.IsOffline + ", PhotonNetwork.isMasterClient=" + PhotonNetwork.isMasterClient);
        m_currentNewGameSetting = newGameSetting;
        CreatePlayerActor();
        CreateNormal(m_currentNewGameSetting.normalActorNumber);
        CreateOtherAI();
        m_gameOverCondition = new GameOverCondition_AllActorDied(m_currentNewGameSetting.startAs);
        m_gameState = GameState.Running;
    }

    public void UpdateGame(Action<Action> onGameEnd)
    {
        if(m_gameState != GameState.Running)
        {
            return;
        }
        if(m_gameOverCondition == null)
        {
            return;
        }
        if(m_gameOverCondition.IsGameOver(OnPlayerWin, OnPlayerLose))
        {
            m_gameOverCondition = null;
            m_gameState = GameState.End;
            if(onGameEnd != null)
            {
                onGameEnd(OnGameEnded);
            }
        }
    }

    private void OnGameEnded()
    {
        m_gameState = GameState.None;
    }

    private void OnPlayerWin()
    {
        Debug.Log("Player Win");
    }

    private void OnPlayerLose()
    {
        Debug.Log("Player Lose");
    }

    private void CreatePlayerActor()
    {
        int _playerPrefabID = 0;

        if (m_currentNewGameSetting.startAs == ActorFilter.ActorType.Shooter)
        {
            _playerPrefabID = GameSetting.ShooterActorPrefabID;
        }
        else
        {
            _playerPrefabID = GameSetting.ZombieActorPrefabID;
        }

        if (!NetworkManager.IsOffline)
        {
            m_playerActorPhotonViewID = PhotonNetwork.AllocateViewID();
            PhotonEventSender.CreateActor(_playerPrefabID, Engine.GetRamdomPosition(), Vector3.zero, m_playerActorPhotonViewID);
        }
        else
        {
            Actor _player = Engine.ActorManager.CreateActor(_playerPrefabID, Engine.GetRamdomPosition(), Vector3.zero);
            _player.EnableAI(false);
            CameraController.MainCameraController.Track(_player.gameObject);
        }
    }

    private void CreateNormal(int number)
    {
        for(int i = 0; i < number; i++)
        {
            if (!NetworkManager.IsOffline && PhotonNetwork.isMasterClient)
            {
                m_playerActorPhotonViewID = PhotonNetwork.AllocateViewID();
                PhotonEventSender.CreateActor(Engine.GameSetting.NormalActorPrefabID, Engine.GetRamdomPosition(), Vector3.zero, m_playerActorPhotonViewID);
            }
            else if(NetworkManager.IsOffline)
            {
                Engine.ActorManager.CreateActor(Engine.GameSetting.NormalActorPrefabID, Engine.GetRamdomPosition(), Vector3.zero).EnableAI(true);
            }
        }
    }

    private void OnActorCreated(Actor actor)
    {
        if (actor.PhotonView.viewID == m_playerActorPhotonViewID)
        {
            actor.EnableAI(false);
            CameraController.MainCameraController.Track(actor.gameObject);
        }
        else
        {
            actor.EnableAI(true);
        }
    }

    private void CreateOtherAI()
    {
        switch(m_currentNewGameSetting.gameType)
        {
            case NewGameSetting.GameType.PvE:
                {
                    switch(m_currentNewGameSetting.playerNumber)
                    {
                        case NewGameSetting.PlayerNumber._1v1:
                            {
                                if (m_currentNewGameSetting.startAs == ActorFilter.ActorType.Shooter)
                                {
                                    CreateAIActor(GameSetting.ZombieActorPrefabID);
                                }
                                else
                                {
                                    CreateAIActor(GameSetting.ShooterActorPrefabID);
                                }
                                break;
                            }
                        case NewGameSetting.PlayerNumber._2v2:
                            {
                                if(m_currentNewGameSetting.startAs == ActorFilter.ActorType.Shooter)
                                {
                                    CreateAIActor(GameSetting.ZombieActorPrefabID);
                                    CreateAIActor(GameSetting.ZombieActorPrefabID);
                                    if (NetworkManager.IsOffline)
                                    {
                                        CreateAIActor(GameSetting.ShooterActorPrefabID);
                                    }
                                }
                                else
                                {
                                    CreateAIActor(GameSetting.ShooterActorPrefabID);
                                    CreateAIActor(GameSetting.ShooterActorPrefabID);
                                    if (NetworkManager.IsOffline)
                                    {
                                        CreateAIActor(GameSetting.ZombieActorPrefabID);
                                    }
                                }
                                break;
                            }
                    }
                    break;
                }
            case NewGameSetting.GameType.PvP:
                {
                    switch (m_currentNewGameSetting.playerNumber)
                    {
                        case NewGameSetting.PlayerNumber._1v1:
                            {
                                if(NetworkManager.IsOffline)
                                {
                                    if (m_currentNewGameSetting.startAs == ActorFilter.ActorType.Shooter)
                                    {
                                        CreateAIActor(GameSetting.ZombieActorPrefabID);
                                    }
                                    else
                                    {
                                        CreateAIActor(GameSetting.ShooterActorPrefabID);
                                    }
                                }
                                break;
                            }
                        case NewGameSetting.PlayerNumber._2v2:
                            {
                                if (NetworkManager.IsOffline)
                                {
                                    if (m_currentNewGameSetting.startAs == ActorFilter.ActorType.Shooter)
                                    {
                                        CreateAIActor(GameSetting.ZombieActorPrefabID);
                                        CreateAIActor(GameSetting.ZombieActorPrefabID);
                                        CreateAIActor(GameSetting.ShooterActorPrefabID);
                                    }
                                    else
                                    {
                                        CreateAIActor(GameSetting.ShooterActorPrefabID);
                                        CreateAIActor(GameSetting.ShooterActorPrefabID);
                                        CreateAIActor(GameSetting.ZombieActorPrefabID);
                                    }
                                }
                                break;
                            }
                    }
                    break;
                }
        }
    }

    private void CreateAIActor(int actorID)
    {
        if (!NetworkManager.IsOffline && PhotonNetwork.isMasterClient)
        {
            int _photonViewID = PhotonNetwork.AllocateViewID();
            PhotonEventSender.CreateActor(actorID, Engine.GetRamdomPosition(), Vector3.zero, _photonViewID);
        }
        else if (NetworkManager.IsOffline)
        {
            Engine.ActorManager.CreateActor(actorID, Engine.GetRamdomPosition(), Vector3.zero).EnableAI(true);
        }
    }

}
