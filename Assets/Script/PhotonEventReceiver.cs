using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon;

public class PhotonEventReceiver : PunBehaviour {

    public static bool Exist { get { return m_instance != null; } }
    private static PhotonEventReceiver m_instance = null;

    public static event Action<DisconnectCause> OnConnectFail = null;
    public static event Action OnLobbyJoined = null;
    public static event Action OnRoomCreated = null;
    public static event Action OnRoomJoined = null;
    public static event Action OnRoomLeft = null;
    public static event Action<Hashtable> OnRoomPropertiesChanged = null;
    public static event Action<Dictionary<PhotonPlayer, Hashtable>> OnPlayerPropertiesChanged = null;
    public static event Action<PhotonPlayer> OnOtherPlayerConnected = null;
    public static event Action<PhotonPlayer> OnOtherPlayerDisconnected = null;

    private void Awake()
    {
        if(m_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        if (OnConnectFail != null)
        {
            OnConnectFail(cause);
        }
    }

    public override void OnJoinedLobby()
    {
        if (OnLobbyJoined != null)
        {
            OnLobbyJoined();
        }
    }

    public override void OnCreatedRoom()
    {
        if (OnRoomCreated != null)
        {
            OnRoomCreated();
        }
    }

    public override void OnJoinedRoom()
    {
        if (OnRoomJoined != null)
        {
            OnRoomJoined();
        }
    }

    public override void OnLeftRoom()
    {
        if(OnRoomLeft != null)
        {
            OnRoomLeft();
        }
    }

    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        if (OnPlayerPropertiesChanged != null)
        {
            Dictionary<PhotonPlayer, Hashtable> _playerToHashtable = new Dictionary<PhotonPlayer, Hashtable>();
            for(int i = 0; i < playerAndUpdatedProps.Length; i+=2)
            {
                if(i + 1 < playerAndUpdatedProps.Length)
                {
                    PhotonPlayer _player = (PhotonPlayer)playerAndUpdatedProps[i];
                    Hashtable _hashtable = (Hashtable)playerAndUpdatedProps[i + 1];
                    _playerToHashtable.Add(_player, _hashtable);
                }
            }
            OnPlayerPropertiesChanged(_playerToHashtable);
        }
    }

    public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
    {
        if(OnRoomPropertiesChanged != null)
        {
            OnRoomPropertiesChanged(propertiesThatChanged);
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        if(OnOtherPlayerConnected != null)
        {
            OnOtherPlayerConnected(newPlayer);
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        if(OnOtherPlayerDisconnected != null)
        {
            OnOtherPlayerDisconnected(otherPlayer);
        }
    }

}
