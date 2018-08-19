using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class NetworkManager {

    private Action m_onRoomCreated = null;
    private Action m_onRoomJoined = null;

    public void StartCreateRoom(NewGameSetting gameSetting, Action onRoomCreated)
    {
        if(m_onRoomCreated != null)
        {
            UnityEngine.Debug.LogError("Previous creating room flow is not complete yet");
            return;
        }
        UnityEngine.Debug.Log("StartCreateRoom");
        PhotonEventReceiver.OnPlayerPropertiesChanged += OnPlayerDataUpdated_CreateRoom;
        NetworkCommander.UpdatePlayerProperties(gameSetting);
        m_onRoomCreated = onRoomCreated;
    }

    public void StartJoinRoom(NewGameSetting gameSetting, Action onRoomJoined)
    {
        if (m_onRoomJoined != null)
        {
            UnityEngine.Debug.LogError("Previous joining room flow is not complete yet");
            return;
        }

        PhotonEventReceiver.OnPlayerPropertiesChanged += OnPlayerDataUpdated_JoinRoom;
        NetworkCommander.UpdatePlayerProperties(gameSetting);

        m_onRoomJoined = onRoomJoined;
    }   
    
    private void OnPlayerDataUpdated_CreateRoom(Dictionary<PhotonPlayer, Hashtable> keyValuePairs)
    {
        UnityEngine.Debug.Log("OnPlayerDataUpdated_CreateRoom");
        PhotonEventReceiver.OnPlayerPropertiesChanged -= OnPlayerDataUpdated_CreateRoom;

        PhotonEventReceiver.OnRoomCreated += OnRoomCreated;
        NetworkCommander.CreateRoom(null);
    }

    private void OnPlayerDataUpdated_JoinRoom(Dictionary<PhotonPlayer, Hashtable> keyValuePairs)
    {
        PhotonEventReceiver.OnPlayerPropertiesChanged -= OnPlayerDataUpdated_JoinRoom;

        PhotonEventReceiver.OnRoomJoined += OnRoomJoined;
        NetworkCommander.JoinRoom(null);
    }

    private void OnRoomCreated()
    {
        UnityEngine.Debug.Log("OnRoomCreated");
        PhotonEventReceiver.OnRoomCreated -= OnRoomCreated;
        if(m_onRoomCreated != null)
        {
            m_onRoomCreated();
        }
        m_onRoomCreated = null;
    }

    private void OnRoomJoined()
    {
        PhotonEventReceiver.OnRoomJoined -= OnRoomJoined;
        if (m_onRoomJoined != null)
        {
            m_onRoomJoined();
        }
        m_onRoomJoined = null;
    }

    // ===當有其他玩家加入 & 自己是MASTER時===
    // 檢查成員構成，如果不合規則 => 最後加入的人很可能是因為連線延遲導致加入的 => 踢走
    // 根據成員構成更新房間參數 => 令房間不可視，直到更新完成
    // ===流程結束===

    // ===當有其他玩家離開 & 自己是MASTER時===
    // 檢查成員構成，如果不合規則 => 最後加入的人很可能是因為連線延遲導致加入的 => 踢走
    // 根據成員構成更新房間參數 => 令房間不可視，直到更新完成
    // ===流程結束===

}
