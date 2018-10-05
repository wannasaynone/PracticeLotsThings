using System.Collections.Generic;
using UnityEngine;
using PracticeLotsThings.Data;
using PracticeLotsThings.View.Actor;
using PracticeLotsThings.MainGameMonoBehaviour;
using PracticeLotsThings.View.UI;

namespace PracticeLotsThings.Manager
{
    public class GameManager : Manager
    {
        public const string GAME_VERSION = "V0.50";

        public enum GameState
        {
            None,
            Initing,
            Running,
            AppandingToMasterClient,
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

        public GameState CurrentGameState { get { return m_gameState; } }

        private ObjectPrefabManager m_objectPrefabManager = null;
        private static GameSetting m_gameSetting = null;
        private static Actor m_player = null;
        private NewGameSetting m_currentNewGameSetting = null;

        private GameState m_gameState = GameState.None;
        private IGameStartCondition m_gameStartCondition = null;
        private IGameOverCondition m_gameOverCondition = null;
        private IGameLogic m_runningGameLogic = null;

        private List<int> m_otherAIObjectInstanceID = new List<int>();

        public GameManager(ObjectPrefabManager objectPrefabManager)
        {
            m_gameState = GameState.None;
            m_objectPrefabManager = objectPrefabManager;
        }

        public void InitGame(NewGameSetting newGameSetting)
        {
            if (m_gameState != GameState.None)
            {
                return;
            }

            Debug.Log("init game, newGameSetting.gameType=" + newGameSetting.gameType + ", newGameSetting.startAs=" + newGameSetting.startAs);
            Debug.Log("NetworkManager.IsOffline=" + NetworkManager.IsOffline + ", PhotonNetwork.isMasterClient=" + PhotonNetwork.isMasterClient);

            if (NetworkManager.IsOffline || (!NetworkManager.IsOffline && PhotonNetwork.isMasterClient))
            {
                for (int i = 0; i < GameSetting.SceneObjectNumber; i++)
                {
                    int _id = Random.Range(0, m_objectPrefabManager.Length);
                    Engine.Instance.CreateObject(_id, null, Engine.GetRamdomPosition(), m_objectPrefabManager.GetPrefab(_id).transform.eulerAngles);
                }
            }

            m_currentNewGameSetting = newGameSetting;
            CreatePlayerActor();

            if (NetworkManager.IsOffline || (!NetworkManager.IsOffline && PhotonNetwork.isMasterClient))
            {
                m_otherAIObjectInstanceID = new List<int>();

                int _needActorNumber = m_currentNewGameSetting.totalActorNumber == NewGameSetting.ActorNumber._1v1 ? 1 : 2;
                m_gameStartCondition = new MainGameStartCondition(_needActorNumber, _needActorNumber, m_currentNewGameSetting.normalActorNumber);
                m_gameState = GameState.Initing;

                CreateNormal(m_currentNewGameSetting.normalActorNumber);
                CreateOtherAI();
            }
            else
            {
                m_gameState = GameState.AppandingToMasterClient;
            }
        }

        private void StartGame()
        {
            if (NetworkManager.IsOffline || (!NetworkManager.IsOffline && PhotonNetwork.isMasterClient))
            {
                List<Actor> _allActors = ActorManager.AllActors;
                for (int i = 0; i < _allActors.Count; i++)
                {
                    if (m_otherAIObjectInstanceID.Contains(_allActors[i].gameObject.GetInstanceID()))
                    {
                        _allActors[i].EnableAI(true);
                    }
                    else if (_allActors[i] == m_player)
                    {
                        _allActors[i].EnableAI(false);
                    }
                }
                m_runningGameLogic = new MainGameLogic(50f, 3f);
                m_gameState = GameState.Running;
                m_gameOverCondition = new GameOverCondition_AllActorDied(m_currentNewGameSetting.startAs);
                if (!NetworkManager.IsOffline)
                {
                    PhotonEventSender.StartGame();
                }
            }
        }

        public void SyncGameStart()
        {
            m_player.EnableAI(false);
        }

        public void UpdateGame()
        {
            switch (m_gameState)
            {
                case GameState.None:
                case GameState.AppandingToMasterClient:
                    {
                        break;
                    }
                case GameState.Initing:
                    {
                        if (m_gameStartCondition.IsGameCanStart())
                        {
                            StartGame();
                        }
                        break;
                    }
                case GameState.Running:
                    {
                        m_runningGameLogic.Tick();
                        if (m_gameOverCondition.IsGameOver(OnPlayerWin, OnPlayerLose))
                        {
                            m_gameOverCondition = null;
                            m_gameState = GameState.End;
                        }
                        break;
                    }
            }
        }

        private void OnGameEnded()
        {
            m_gameState = GameState.None;
        }

        public void SyncGameOver(ActorFilter.ActorType wonActorType)
        {
            if (PhotonNetwork.isMasterClient)
            {
                return;
            }

            if (wonActorType == m_currentNewGameSetting.startAs)
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
            if (!NetworkManager.IsOffline)
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

            Engine.ActorManager.CreateActor(_playerPrefabID,
                delegate (Actor actor)
                {
                    m_player = actor;
                    CameraController.MainCameraController.Track(actor.gameObject);
                },
                Engine.GetRamdomPosition());
        }

        private void CreateNormal(int number)
        {
            for (int i = 0; i < number; i++)
            {
                Engine.ActorManager.CreateActor(GameSetting.NormalActorPrefabID,
                delegate (Actor actor)
                {
                    m_otherAIObjectInstanceID.Add(actor.gameObject.GetInstanceID());
                },
                Engine.GetRamdomPosition(), Vector3.zero);
            }
        }

        private void CreateOtherAI()
        {
            switch (m_currentNewGameSetting.gameType)
            {
                case NewGameSetting.GameType.PvE:
                    {
                        switch (m_currentNewGameSetting.totalActorNumber)
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
                                    if (m_currentNewGameSetting.startAs == ActorFilter.ActorType.Shooter)
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
                                    if (NetworkManager.IsOffline)
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
            Engine.ActorManager.CreateActor(actorID,
            delegate (Actor actor)
            {
                m_otherAIObjectInstanceID.Add(actor.gameObject.GetInstanceID());
            },
            Engine.GetRamdomPosition());
        }
    }
}