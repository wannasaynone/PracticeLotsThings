using UnityEngine;
using System;

public class PhotonEventSender : MonoBehaviour {

    public static event Action OnGameObjectCreated = null;
    private static PhotonView photonView = null;

    [SerializeField] private PhotonView m_photonView = null;

    private void Awake()
    {
        if(photonView != null)
        {
            Destroy(gameObject);
            return;
        }

        photonView = m_photonView;
        if(OnGameObjectCreated != null)
        {
            OnGameObjectCreated();
        }
    }

    public static void CreateActor(int actorID, Vector3 position, Vector3 angle, int photonViewID)
    {
        photonView.RPC("PhotonEventSender_CreateActor", PhotonTargets.AllBuffered, actorID, position, angle, photonViewID);
    }

    [PunRPC]
    private void PhotonEventSender_CreateActor(int actorID, Vector3 position, Vector3 angle, int photonViewID)
    {
        Engine.ActorManager.CreateActor(actorID, position, angle).GetComponent<PhotonView>().viewID = photonViewID;
    }

}
