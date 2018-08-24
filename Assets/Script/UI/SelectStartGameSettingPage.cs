using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStartGameSettingPage : View {

    private enum PlayAs
    {
        Shooter,
        Zombie,
        Random
    }

    private enum GameType
    {
        PVE,
        PVP,
        Random
    }

    private enum PlayerNumber
    {
        _1V1,
        _2V2,
        Random
    }

    [SerializeField] private GameObject m_mainGameMenuRoot = null;
    [SerializeField] private GameObject m_gameSetting_charSetMenuRoot = null;

    [SerializeField] private Button m_connectServerButton = null;
    [SerializeField] private Button m_startLocalGameButton = null;

    [SerializeField] private GameObject m_onlineStartGameMenuRoot = null;
    [SerializeField] private GameObject m_localStartGameMenuRoot = null;

    [Header("Data")]
    [SerializeField] private Dropdown m_playAsDropdown = null;
    [SerializeField] private Dropdown m_playerNumberDropdown = null;
    [SerializeField] private Dropdown m_gameTypeDropdown = null;

    private GameType m_gameType = GameType.PVE;
    private PlayerNumber m_playerNumber = PlayerNumber._1V1;
    private PlayAs m_playAs = PlayAs.Shooter;

    private void Start()
    {
        PhotonEventReceiver.OnLobbyJoined += ShowSetCharacterMenu;
        PhotonEventReceiver.OnConnectFail += OnConnectFailed;
        PhotonEventReceiver.OnRoomLeft += OnRoomLeft;
    }

    public void HideAll()
    {
        m_mainGameMenuRoot.SetActive(false);
        m_gameSetting_charSetMenuRoot.SetActive(false);
    }

    public void ShowMainGameMenu()
    {
        HideAll();
        m_mainGameMenuRoot.SetActive(true);
    }

    public void ShowSetCharacterMenu()
    {
        HideAll();
        m_onlineStartGameMenuRoot.SetActive(PhotonNetwork.connected);
        m_localStartGameMenuRoot.SetActive(!PhotonNetwork.connected);
        m_gameSetting_charSetMenuRoot.SetActive(true);
    }

    public void StartCreateOnlineGame()
    {
        HideAll();
        ShowSetCharacterMenu();
    }

    public void Button_StartCreateLocalGame()
    {
        HideAll();
        ShowSetCharacterMenu();
    }

    public void Button_Connect()
    {
        NetworkManager.Instance.Connect(StartCreateOnlineGame);
        m_connectServerButton.interactable = false;
        m_startLocalGameButton.interactable = false;
    }

    public void Button_StartLocalGame()
    {
        
    }

    public void Button_StartOnlineGame()
    {
        HideAll();
        Engine.NewGameSetting = CreateNewGameSetting();
        NetworkManager.Instance.EnterGame(Engine.NewGameSetting, null);
    }

    private void OnRoomLeft()
    {

    }

    private void OnConnectFailed(DisconnectCause cause)
    {
        // TODO: message box
        m_connectServerButton.interactable = true;
        m_startLocalGameButton.interactable = true;
        ShowMainGameMenu();
    }

    private NewGameSetting CreateNewGameSetting()
    {
        NewGameSetting _gameSetting = ScriptableObject.CreateInstance<NewGameSetting>();

        m_gameType = (GameType)m_gameTypeDropdown.value;
        m_playerNumber = (PlayerNumber)m_playerNumberDropdown.value;
        m_playAs = (PlayAs)m_playAsDropdown.value;

        if (m_gameType == GameType.PVE)
        {
            if (m_playerNumber == PlayerNumber._1V1)
            {
                _gameSetting.gameType = NewGameSetting.GameType.PvE_1v1;
            }
            else
            {
                _gameSetting.gameType = NewGameSetting.GameType.PvE_2v2;
            }
        }
        else
        {
            if (m_playerNumber == PlayerNumber._1V1)
            {
                _gameSetting.gameType = NewGameSetting.GameType.PvP_1v1;
            }
            else
            {
                _gameSetting.gameType = NewGameSetting.GameType.PvP_2v2;
            }
        }

        switch (m_playAs)
        {
            case PlayAs.Shooter:
                {
                    _gameSetting.startAs = ActorFilter.ActorType.Shooter;
                    break;
                }
            case PlayAs.Zombie:
                {
                    _gameSetting.startAs = ActorFilter.ActorType.Zombie;
                    break;
                }
        }

        _gameSetting.normalActorNumber = 50;

        return _gameSetting;
    }

}
