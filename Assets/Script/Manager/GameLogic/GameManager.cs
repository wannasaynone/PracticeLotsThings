using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager {

    public const string GAME_VERSION = "V0.33";

    public enum GameState
    {
        None,
        WaitingOtherPlayerJoin,
        Running,
        End
    }

    public static Actor Player { get { return m_player; } }
    public static GameSetting GameSetting
    {
        get
        {
            if (m_gameSetting == null)
            {
                GameDataManager.LoadGameData<GameSetting>("Data/GameSetting");
                m_gameSetting = GameDataManager.GetGameData<GameSetting>(0);
            }
            return m_gameSetting;
        }
    }

    private static GameSetting m_gameSetting = null;
    private static Actor m_player = null;
    private NewGameSetting m_currentNewGameSetting = null;

    private GameState m_gameState = GameState.None;
    private IGameOverCondition m_gameOverCondition;

    private List<int> m_allAIActorPhotonViewID = new List<int>();
    private int m_playerActorPhotonViewID = -1;
    private int m_needActorNumber = 0;

    public GameManager()
    {

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
        if(NetworkManager.IsOffline)
        {
            CreateNormal(m_currentNewGameSetting.normalActorNumber);
            CreateOtherAI();
            m_gameOverCondition = new GameOverCondition_AllActorDied(m_currentNewGameSetting.startAs);
            m_gameState = GameState.Running;
        }
        else
        {
            if(PhotonNetwork.isMasterClient)
            {
                m_gameState = GameState.WaitingOtherPlayerJoin;
                int _needActorNumber = m_currentNewGameSetting.totalActorNumber == NewGameSetting.ActorNumber._1v1 ? 2 : 4;
                m_needActorNumber = newGameSetting.normalActorNumber + _needActorNumber;
                CreateNormal(m_currentNewGameSetting.normalActorNumber);
                CreateOtherAI();
            }
        }
    }

    private void StartGame()
    {
        if(PhotonNetwork.isMasterClient)
        {
            List<Actor> _allActor = ActorManager.AllActors;

            for (int i = 0; i < _allActor.Count; i++)
            {
                if (m_allAIActorPhotonViewID.Contains(Engine.ActorManager.GetPhotonView(_allActor[i]).viewID))
                {
                    _allActor[i].EnableAI(true);
                }
            }
            Engine.ActorManager.GetPhotonActor(m_playerActorPhotonViewID).EnableAI(false);
            m_gameState = GameState.Running;
            m_gameOverCondition = new GameOverCondition_AllActorDied(m_currentNewGameSetting.startAs);
            PhotonEventSender.StartGame();
        }
    }

    public void SyncGameStart()
    {
        Engine.ActorManager.GetPhotonActor(m_playerActorPhotonViewID).EnableAI(false);
    }

    public void UpdateGame()
    {
        if(!NetworkManager.IsOffline && !PhotonNetwork.isMasterClient)
        {
            return;
        }

        if (m_gameState != GameState.Running || m_gameOverCondition == null)
        {
            return;
        }

        if(m_gameOverCondition.IsGameOver(OnPlayerWin, OnPlayerLose))
        {
            m_gameOverCondition = null;
            m_gameState = GameState.End;
        }
    }

    private void OnGameEnded()
    {
        m_gameState = GameState.None;
    }

    public void SyncGameOver(ActorFilter.ActorType wonActorType)
    {
        if(PhotonNetwork.isMasterClient)
        {
            return;
        }

        if(wonActorType == m_currentNewGameSetting.startAs)
        {
            ((GameCommonUIPage)GetViews<GameCommonUIPage>()[0]).ShowGameOverMenu("You Win", OnGameEnded);
        }
        else
        {
            ((GameCommonUIPage)GetViews<GameCommonUIPage>()[0]).ShowGameOverMenu("You Lose", OnGameEnded);
        }
    }

    private void OnPlayerWin()
    {
        if(!NetworkManager.IsOffline)
        {
            PhotonEventSender.EndGame(m_currentNewGameSetting.startAs);
        }

        ((GameCommonUIPage)GetViews<GameCommonUIPage>()[0]).ShowGameOverMenu("You Win", OnGameEnded);
    }

    private void OnPlayerLose()
    {
        if (!NetworkManager.IsOffline)
        {
            PhotonEventSender.EndGame(m_currentNewGameSetting.startAs == ActorFilter.ActorType.Shooter ? ActorFilter.ActorType.Zombie : ActorFilter.ActorType.Shooter);
        }

        ((GameCommonUIPage)GetViews<GameCommonUIPage>()[0]).ShowGameOverMenu("You Lose", OnGameEnded);
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
                int _photonViewID = PhotonNetwork.AllocateViewID();
                m_allAIActorPhotonViewID.Add(_photonViewID);
                PhotonEventSender.CreateActor(GameSetting.NormalActorPrefabID, Engine.GetRamdomPosition(), Vector3.zero, _photonViewID);
            }
            else if(NetworkManager.IsOffline)
            {
                Engine.ActorManager.CreateActor(GameSetting.NormalActorPrefabID, Engine.GetRamdomPosition(), Vector3.zero).EnableAI(true);
            }
        }
    }

    private void OnActorCreated(Actor actor)
    {
        if(Engine.ActorManager.GetPhotonView(actor).viewID == m_playerActorPhotonViewID)
        {
            CameraController.MainCameraController.Track(actor.gameObject);
        }

        if (PhotonNetwork.isMasterClient && m_gameState == GameState.WaitingOtherPlayerJoin)
        {
            if (m_needActorNumber == ActorManager.AllActors.Count)
            {
                StartGame();
            }
        }
    }

    private void CreateOtherAI()
    {
        switch(m_currentNewGameSetting.gameType)
        {
            case NewGameSetting.GameType.PvE:
                {
                    switch(m_currentNewGameSetting.totalActorNumber)
                    {
                        case NewGameSetting.ActorNumber._1v1:
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
                        case NewGameSetting.ActorNumber._2v2:
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
                    switch (m_currentNewGameSetting.totalActorNumber)
                    {
                        case NewGameSetting.ActorNumber._1v1:
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
                        case NewGameSetting.ActorNumber._2v2:
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
            m_allAIActorPhotonViewID.Add(_photonViewID);
            PhotonEventSender.CreateActor(actorID, Engine.GetRamdomPosition(), Vector3.zero, _photonViewID);
        }
        else if (NetworkManager.IsOffline)
        {
            Engine.ActorManager.CreateActor(actorID, Engine.GetRamdomPosition(), Vector3.zero).EnableAI(true);
        }
    }

}
