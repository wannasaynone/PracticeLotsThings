using UnityEngine;
using UnityEngine.UI;
using PracticeLotsThings.Manager;
using PracticeLotsThings.MainGameMonoBehaviour;
using PracticeLotsThings.Data;

namespace PracticeLotsThings.View.UI
{
    public class SelectStartGameSettingPage : View
    {

        private enum PlayAs
        {
            Shooter,
            Zombie,
            Random
        }

        private enum GameType
        {
            PVP,
            PVE,
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
            m_onlineStartGameMenuRoot.SetActive(!NetworkManager.IsOffline);
            m_localStartGameMenuRoot.SetActive(NetworkManager.IsOffline);
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
            Engine.NetworkManager.Connect(StartCreateOnlineGame);
            m_connectServerButton.interactable = false;
            m_startLocalGameButton.interactable = false;
        }

        public void Button_StartGame()
        {
            HideAll();
            NewGameSetting _newGameSetting = CreateNewGameSetting();
            Engine.Instance.LoadScene("Test",
                delegate
                {
                    if (NetworkManager.IsOffline)
                    {
                        Engine.GameManager.InitGame(_newGameSetting);
                    }
                    else
                    {
                        Engine.NetworkManager.EnterGame(_newGameSetting, delegate { Engine.GameManager.InitGame(_newGameSetting); });
                    }
                });
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
                _gameSetting.gameType = NewGameSetting.GameType.PvE;
            }
            else
            {
                _gameSetting.gameType = NewGameSetting.GameType.PvP;
            }

            if (m_playerNumber == PlayerNumber._1V1)
            {
                _gameSetting.totalActorNumber = NewGameSetting.ActorNumber._1v1;
            }
            else
            {
                _gameSetting.totalActorNumber = NewGameSetting.ActorNumber._2v2;
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
}