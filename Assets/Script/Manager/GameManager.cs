using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager {

    public const string GAME_VERSION = "V0.31";

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

    private int m_playerActorPhotonViewID = -1;

    public GameManager(ActorManager actorManager, GameSetting gameSetting)
    {
        m_actorManager = actorManager;
        m_gameSetting = gameSetting;
        m_gameState = GameState.None;
    }

    public void InitGame(NewGameSetting newGameSetting)
    {
        Debug.Log("newGameSetting.gameType=" + newGameSetting.gameType + ", newGameSetting.startAs=" + newGameSetting.startAs);
        Debug.Log("PhotonNetwork.isMasterClient=" + PhotonNetwork.isMasterClient);
        m_currentNewGameSetting = newGameSetting;

        PhotonEventSender.OnActorCreated += OnPlayerActorCreated;

        int _prefabID = 0;

        if (m_currentNewGameSetting.startAs == ActorFilter.ActorType.Shooter)
        {
            _prefabID = GameSetting.ShooterActorPrefabID;
        }
        else
        {
            _prefabID = GameSetting.ZombieActorPrefabID;
        }
        m_playerActorPhotonViewID = PhotonNetwork.AllocateViewID();
        PhotonEventSender.CreateActor(_prefabID, Engine.GetRamdomPosition(), Vector3.zero, m_playerActorPhotonViewID);
    }

    private void OnPlayerActorCreated(PhotonView photonView, Actor actor)
    {
        if(photonView.viewID == m_playerActorPhotonViewID)
        {
            PhotonEventSender.OnActorCreated -= OnPlayerActorCreated;
            photonView.TransferOwnership(PhotonNetwork.player.ID);
            actor.EnableAI(false);
            CameraController.MainCameraController.Track(actor.gameObject);
        }
    }

    private void CreateShooter(bool isPlayer)
    {

    }

    private void CreateZombie(bool isPlayer)
    {

    }

    private void CreateNormal(int number)
    {

    }

}
