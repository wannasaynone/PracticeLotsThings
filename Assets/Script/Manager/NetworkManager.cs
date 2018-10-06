using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System;
using JsonFx.Json;
using PracticeLotsThings.Data;
using PracticeLotsThings.MainGameMonoBehaviour;

namespace PracticeLotsThings.Manager
{
    public class NetworkManager : Manager
    {
        public static bool IsOffline { get; private set; }

        public NetworkManager(bool isLocalhost)
        {
            IsOffline = true;
            m_isLocalhost = isLocalhost;
        }

        private bool m_isLocalhost = false;

        private string host = "ec2-13-58-220-134.us-east-2.compute.amazonaws.com";
        private string localhost = "localhost";
        private int port = 3000;
        private Socket m_socket = null;
        private NetworkStream m_netStream;
        private StreamWriter m_writer;
        private StreamReader m_reader;
        private char[] m_readMsgBuffer = new char[2048];

        private Action m_onConnected = null;
        private Action m_onDisconnected = null;
        private Action m_onGameJoined = null;

        public void Connect(Action onConnected)
        {
            if (!PhotonEventReceiver.Exist)
            {
                GameObject gameObject = new GameObject("[PhotonEventReceiver]");
                gameObject.AddComponent<PhotonEventReceiver>();
            }

            if (m_onConnected != null || PhotonNetwork.connected)
            {
                return;
            }

            PhotonEventReceiver.OnConnected += OnConnectedToPhoton;
            PhotonNetwork.ConnectUsingSettings(Engine.GameVersion);

            m_onConnected = onConnected;
        }

        private void OnConnectedToPhoton()
        {
            PhotonEventReceiver.OnConnected -= OnConnectedToPhoton;
            ConnectToPairServer();
        }

        private void ConnectToPairServer()
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (m_isLocalhost)
            {
                Debug.Log("ConnectToPairServer:localhost");
                m_socket.Connect(localhost, port);
            }
            else
            {
                Debug.Log("ConnectToPairServer:" + host + ":" + port);
                m_socket.Connect(host, port);
            }

            m_netStream = new NetworkStream(m_socket);
            m_writer = new StreamWriter(m_netStream);
            m_reader = new StreamReader(m_netStream);
        }

        private void OnConnectedToPairServer()
        {
            IsOffline = false;
            if (m_onConnected != null)
            {
                m_onConnected();
            }

            m_onConnected = null;
        }

        public void Disconnect(Action onDisconnected)
        {
            if (m_onDisconnected != null || !PhotonNetwork.connected)
            {
                return;
            }
            PhotonNetwork.Disconnect();
            PhotonEventReceiver.OnDisconnected += OnDisconnectedFromPhoton;
            m_onDisconnected = onDisconnected;
        }

        private void OnDisconnectedFromPhoton()
        {
            PhotonEventReceiver.OnDisconnected -= OnDisconnectedFromPhoton;
            m_reader.Close();
            m_writer.Close();
            m_netStream.Close();
            m_reader.Dispose();
            m_writer.Dispose();
            m_netStream.Dispose();
            m_reader = null;
            m_writer = null;
            m_netStream = null;

            m_socket.Shutdown(SocketShutdown.Both);
            m_socket.Close();
            m_socket = null;

            if (m_onDisconnected != null)
            {
                m_onDisconnected();
            }

            m_onDisconnected = null;
        }

        public void Update()
        {
            if (m_socket == null || m_netStream == null || m_writer == null || m_reader == null)
            {
                return;
            }

            Receive();
        }

        public void EnterGame(NewGameSetting newGameSetting, Action onGameJoined)
        {
            if (m_onGameJoined != null)
            {
                return;
            }
            NewGameSettingData _newGameSettingData = new NewGameSettingData(newGameSetting.GetGameTypeString(), newGameSetting.GetActorString());
            string jsonString = JsonWriter.Serialize(_newGameSettingData);
            m_onGameJoined = onGameJoined;
            Send(jsonString);
        }

        private void Receive()
        {
            int _bytesRead = 0;
            while (m_netStream.DataAvailable)
            {
                _bytesRead += m_reader.Read(m_readMsgBuffer, _bytesRead, m_readMsgBuffer.Length);
                if (_bytesRead > 0)
                {
                    string _msg = new string(m_readMsgBuffer, 0, _bytesRead);
                    Debug.Log("Receive:" + _msg);
                    if (_msg == "1")
                    {
                        OnConnectedToPairServer();
                    }
                    else
                    {
                        DeserializeServerData(_msg, delegate (RoomData roomData)
                        {
                            if (!string.IsNullOrEmpty(roomData.RoomName))
                            {
                                if (roomData.IsMasterClient)
                                {
                                    PhotonNetwork.CreateRoom(roomData.RoomName);
                                    PhotonEventReceiver.OnRoomCreated += OnCreatedRoom;
                                }
                            }
                        });
                        DeserializeServerData(_msg, delegate (JoinRoomData joinRoomData)
                        {
                            if (!string.IsNullOrEmpty(joinRoomData.JoinRoomName))
                            {
                                PhotonNetwork.JoinRoom(joinRoomData.JoinRoomName);
                                PhotonEventReceiver.OnRoomJoined += OnJoinedRoom;
                            }
                        });
                    }
                }
            }
        }

        private void DeserializeServerData<T>(string jsonString, Action<T> onDeserialized)
        {
            try
            {
                T _data = JsonReader.Deserialize<T>(jsonString);
                if (onDeserialized != null)
                {
                    onDeserialized(_data);
                }
            }
            catch { }
        }

        private void OnCreatedRoom()
        {
            PhotonEventReceiver.OnRoomCreated -= OnCreatedRoom;
            PhotonEventReceiver.OnRoomJoined += OnJoinedRoom;
        }

        private void OnJoinedRoom()
        {
            PhotonEventReceiver.OnRoomJoined -= OnJoinedRoom;

            if (PhotonNetwork.isMasterClient)
            {
                PhotonEventSender.OnPhotonEventSenderCreated += MasterClinet_OnPhotonEventSenderCreated;
                PhotonNetwork.Instantiate("PhotonEventSender", Vector3.zero, Quaternion.identity, 0);
            }
            else
            {
                PhotonEventSender.OnPhotonEventSenderCreated += NotMasterClient_OnPhotonEventSenderCreated;
            }
        }

        private void MasterClinet_OnPhotonEventSenderCreated()
        {
            Debug.Log("MasterClinet_OnPhotonEventSenderCreated");
            PhotonEventSender.OnPhotonEventSenderCreated -= MasterClinet_OnPhotonEventSenderCreated;

            string jsonString = JsonWriter.Serialize(new CreatedRoomData(PhotonNetwork.room.Name));
            Send(jsonString);

            if (m_onGameJoined != null)
            {
                m_onGameJoined();
            }
            m_onGameJoined = null;
        }

        private void NotMasterClient_OnPhotonEventSenderCreated()
        {
            Debug.Log("NotMasterClient_OnPhotonEventSenderCreated");
            PhotonEventSender.OnPhotonEventSenderCreated -= NotMasterClient_OnPhotonEventSenderCreated;
            if (m_onGameJoined != null)
            {
                m_onGameJoined();
            }
            m_onGameJoined = null;
        }

        private void Send(string message)
        {
            Debug.Log("Send:" + message);
            m_writer.Write(message);
            m_writer.Flush();
        }
    }
}
