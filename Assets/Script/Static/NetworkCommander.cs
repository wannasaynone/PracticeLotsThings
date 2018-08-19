using System;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public static class NetworkCommander {

    private const string GAME_TYPE_PVP = "pvp_";
    private const string GAME_TYPE_PVE = "pve_";
    private const string GAME_TYPE_1V1 = "1v1_";
    private const string GAME_TYPE_2V2 = "2v2_";

    private const string HASHKEY_ACTOR_TYPE = "Actor Type";
    private const string HASHKEY_SHOOTER = "Shooter";
    private const string HASHKEY_ZOMBIE = "Zombie";

    public static ConnectionState CurrentState { get { return PhotonNetwork.connectionState; } }
    public static string CurrentRoomName { get { return m_currentRoomName; } }
    public static NewGameSetting CurrentNewGameSetting { get { return m_currentNewGameSetting; } }

    private static string m_currentRoomName = "";
    private static NewGameSetting m_currentNewGameSetting = null;

	public static void Connect()
    {
        if(PhotonNetwork.connected)
        {
            return;
        }

        if(!PhotonEventReceiver.Exist)
        {
            UnityEngine.GameObject _eventReceiver = new UnityEngine.GameObject("PhotonEventReceiver");
            _eventReceiver.transform.position = UnityEngine.Vector3.zero;
            _eventReceiver.AddComponent<PhotonEventReceiver>();
        }

        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.ConnectUsingSettings(GameManager.GAME_VERSION);

        if (String.IsNullOrEmpty(PhotonNetwork.playerName))
        {
            PhotonNetwork.playerName = "Pl" + UnityEngine.Random.Range(1, 9999);
        }
    }

    public static void Disconnect()
    {
        if (!PhotonNetwork.connected)
        {
            return;
        }

        PhotonNetwork.Disconnect();
    }

    public static void UpdatePlayerProperties(NewGameSetting gameSetting)
    {
        if (CurrentState != ConnectionState.Connected)
        {
            UnityEngine.Debug.LogError("CurrentState != ConnectionState.Connected");
            return;
        }

        m_currentNewGameSetting = gameSetting;
        PhotonNetwork.player.SetCustomProperties(new Hashtable() { { HASHKEY_ACTOR_TYPE, gameSetting.startAs == ActorFilter.ActorType.Shooter ? HASHKEY_SHOOTER : HASHKEY_ZOMBIE } });
    }

    public static void CreateRoom(Action onCantCreateRoom)
    {
        if(!IsConnectionInited())
        {
            return;
        }

        if (!PhotonNetwork.insideLobby || PhotonNetwork.inRoom)
        {
            if (onCantCreateRoom != null)
            {
                onCantCreateRoom();
            }
            return;
        }

        int _roomUID = CreateRommUID();
        if (_roomUID == -1)
        {
            if(onCantCreateRoom != null)
            {
                onCantCreateRoom();
            }
            return;
        }
        else
        {
            string _roomName = GetGameTypeString(m_currentNewGameSetting) + GetPlayerNumberString(m_currentNewGameSetting) + _roomUID;

            RoomOptions _roomOption = new RoomOptions();

            switch (m_currentNewGameSetting.gameType)
            {
                case NewGameSetting.GameType.PvE_1v1:
                    {
                        _roomOption.MaxPlayers = 1;
                        break;
                    }
                case NewGameSetting.GameType.PvE_2v2:
                case NewGameSetting.GameType.PvP_1v1:
                    {
                        _roomOption.MaxPlayers = 2;
                        break;
                    }
                case NewGameSetting.GameType.PvP_2v2:
                    {
                        _roomOption.MaxPlayers = 4;
                        break;
                    }
            }

            _roomOption.CustomRoomProperties = 
                new Hashtable()
                {
                    { HASHKEY_SHOOTER, m_currentNewGameSetting.startAs == ActorFilter.ActorType.Shooter ? 1 : 0 },
                    { HASHKEY_ZOMBIE, m_currentNewGameSetting.startAs == ActorFilter.ActorType.Zombie ? 1 : 0 },
                };
            _roomOption.CustomRoomPropertiesForLobby = new string[] { HASHKEY_SHOOTER, HASHKEY_ZOMBIE };

            PhotonNetwork.CreateRoom(_roomName, _roomOption,  null);
            
            m_currentRoomName = _roomName;
        }
    }

    public static void JoinRoom(Action onCantJoinRoom)
    {
        if (!IsConnectionInited())
        {
            return;
        }

        if (!PhotonNetwork.insideLobby || PhotonNetwork.inRoom)
        {
            if (onCantJoinRoom != null)
            {
                onCantJoinRoom();
            }
            return;
        }

        RoomInfo[] _roomInfos = PhotonNetwork.GetRoomList();
        if(_roomInfos == null || _roomInfos.Length == 0)
        {
            if (onCantJoinRoom != null)
            {
                onCantJoinRoom();
            }
            return;
        }

        for(int i = 0; i < _roomInfos.Length; i++)
        {
            if (_roomInfos[i].Name.Contains(GetGameTypeString(m_currentNewGameSetting)) && _roomInfos[i].Name.Contains(GetPlayerNumberString(m_currentNewGameSetting)))
            {
                string _checkActorType = m_currentNewGameSetting.startAs == ActorFilter.ActorType.Shooter ? HASHKEY_SHOOTER : HASHKEY_ZOMBIE;
                int _checkNumber = 0;

                switch(m_currentNewGameSetting.gameType)
                {
                    case NewGameSetting.GameType.PvE_2v2:
                    case NewGameSetting.GameType.PvP_2v2:
                        {
                            _checkNumber = 2;
                            break;
                        }
                    case NewGameSetting.GameType.PvE_1v1:
                    case NewGameSetting.GameType.PvP_1v1:
                        {
                            _checkNumber = 1;
                            break;
                        }
                }

                if((int)_roomInfos[i].CustomProperties[_checkActorType] >= _checkNumber)
                {
                    continue;
                }

                if (PhotonNetwork.JoinRoom(_roomInfos[i].Name))
                {
                    m_currentRoomName = _roomInfos[i].Name;
                    return;
                }
                else
                {
                    continue;
                }
            }
        }

        if(onCantJoinRoom != null)
        {
            onCantJoinRoom();
        }
    }

    private static bool IsConnectionInited()
    {
        if (CurrentState != ConnectionState.Connected)
        {
            UnityEngine.Debug.LogError("CurrentState != ConnectionState.Connected");
            return false;
        }

        if (m_currentNewGameSetting == null)
        {
            UnityEngine.Debug.LogError("m_currentNewGameSetting == null");
            return false;
        }

        return true;
    }

    private static string GetGameTypeString(NewGameSetting gameSetting)
    {
        switch (gameSetting.gameType)
        {
            case NewGameSetting.GameType.PvE_1v1:
            case NewGameSetting.GameType.PvE_2v2:
                {
                    return GAME_TYPE_PVE;
                }
            case NewGameSetting.GameType.PvP_1v1:
            case NewGameSetting.GameType.PvP_2v2:
                {
                    return GAME_TYPE_PVP;
                }
        }
        UnityEngine.Debug.LogError("Unknow Game Type:" + gameSetting.gameType);
        return "";
    }

    private static string GetPlayerNumberString(NewGameSetting gameSetting)
    {
        switch (gameSetting.gameType)
        {
            case NewGameSetting.GameType.PvP_2v2:
            case NewGameSetting.GameType.PvE_2v2:
                {
                    return GAME_TYPE_2V2;
                }
            case NewGameSetting.GameType.PvP_1v1:
            case NewGameSetting.GameType.PvE_1v1:
                {
                    return GAME_TYPE_1V1;
                }
        }
        UnityEngine.Debug.LogError("Unknow Game Type:" + gameSetting.gameType);
        return "";
    }

    private static int CreateRommUID()
    {
        RoomInfo[] _roomInfos = PhotonNetwork.GetRoomList();
        List<int> _allRoomUIDs = new List<int>();
        for(int i = 0; i < _roomInfos.Length; i++)
        {
            int _tryParse = 0;
            if(int.TryParse(_roomInfos[i].Name.Replace(GAME_TYPE_1V1,"").Replace(GAME_TYPE_2V2,"").Replace(GAME_TYPE_PVE,"").Replace(GAME_TYPE_PVP,""),out _tryParse))
            {
                _allRoomUIDs.Add(_tryParse);
            }
        }

        int _maxTriedTimes = 10000;
        Random _random = new Random();

        for(int i = 0; i < _maxTriedTimes; i++)
        {
            int _roll = _random.Next(int.MaxValue - 1);
            if(!_allRoomUIDs.Contains(_roll))
            {
                return _roll;
            }
        }

        return -1;
    }
}
