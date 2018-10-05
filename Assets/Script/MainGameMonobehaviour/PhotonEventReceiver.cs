using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon;

namespace PracticeLotsThings.MainGameMonoBehaviour
{
    public class PhotonEventReceiver : PunBehaviour
    {
        public static bool Exist { get { return m_instance != null; } }
        private static PhotonEventReceiver m_instance = null;

        public static event Action OnConnected = null;
        public static event Action OnDisconnected = null;
        public static event Action<DisconnectCause> OnConnectFail = null;
        public static event Action OnLobbyJoined = null;
        public static event Action OnRoomCreated = null;
        public static event Action OnRoomJoined = null;
        public static event Action<PhotonMessageInfo> OnPhotonViewCreated = null;
        public static event Action OnRoomLeft = null;
        public static event Action<Hashtable> OnRoomPropertiesChanged = null;
        public static event Action<Dictionary<PhotonPlayer, Hashtable>> OnPlayerPropertiesChanged = null;
        public static event Action<PhotonPlayer> OnOtherPlayerConnected = null;
        public static event Action<PhotonPlayer> OnOtherPlayerDisconnected = null;

        private void Awake()
        {
            if (m_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                m_instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public override void OnConnectedToMaster()
        {
            UnityEngine.Debug.Log("PhotonEvent:OnConnectedToMaster");
            if (OnConnected != null)
            {
                OnConnected();
            }
        }

        public override void OnDisconnectedFromPhoton()
        {
            UnityEngine.Debug.Log("PhotonEvent:OnDisconnectedFromPhoton");
            if (OnDisconnected != null)
            {
                OnDisconnected();
            }
        }

        public override void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            UnityEngine.Debug.Log("PhotonEvent:OnFailedToConnectToPhoton");
            if (OnConnectFail != null)
            {
                OnConnectFail(cause);
            }
        }

        public override void OnJoinedLobby()
        {
            UnityEngine.Debug.Log("PhotonEvent:OnJoinedLobby");
            if (OnLobbyJoined != null)
            {
                OnLobbyJoined();
            }
        }

        public override void OnCreatedRoom()
        {
            UnityEngine.Debug.Log("PhotonEvent:OnCreatedRoom");
            if (OnRoomCreated != null)
            {
                OnRoomCreated();
            }
        }

        public override void OnJoinedRoom()
        {
            UnityEngine.Debug.Log("PhotonEvent:OnJoinedRoom");
            if (OnRoomJoined != null)
            {
                OnRoomJoined();
            }
        }

        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            UnityEngine.Debug.Log("PhotonEvent:OnPhotonInstantiate");
            if (OnPhotonViewCreated != null)
            {
                OnPhotonViewCreated(info);
            }
        }

        public override void OnLeftRoom()
        {
            UnityEngine.Debug.Log("PhotonEvent:OnLeftRoom");
            if (OnRoomLeft != null)
            {
                OnRoomLeft();
            }
        }

        public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            UnityEngine.Debug.Log("PhotonEvent:OnPhotonPlayerPropertiesChanged");
            if (OnPlayerPropertiesChanged != null)
            {
                Dictionary<PhotonPlayer, Hashtable> _playerToHashtable = new Dictionary<PhotonPlayer, Hashtable>();
                for (int i = 0; i < playerAndUpdatedProps.Length; i += 2)
                {
                    if (i + 1 < playerAndUpdatedProps.Length)
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
            UnityEngine.Debug.Log("PhotonEvent:OnPhotonCustomRoomPropertiesChanged");
            if (OnRoomPropertiesChanged != null)
            {
                OnRoomPropertiesChanged(propertiesThatChanged);
            }
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            UnityEngine.Debug.Log("PhotonEvent:OnPhotonPlayerConnected");
            if (OnOtherPlayerConnected != null)
            {
                OnOtherPlayerConnected(newPlayer);
            }
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            UnityEngine.Debug.Log("PhotonEvent:OnPhotonPlayerDisconnected");
            if (OnOtherPlayerDisconnected != null)
            {
                OnOtherPlayerDisconnected(otherPlayer);
            }
        }
    }
}
