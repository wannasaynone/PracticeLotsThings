using UnityEngine;
using System;

public class PhotonEventSender : MonoBehaviour {

    public static event Action OnPhotonEventSenderCreated = null;
    public static event Action<PhotonView, Actor> OnActorCreated = null;
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
        if(OnPhotonEventSenderCreated != null)
        {
            OnPhotonEventSenderCreated();
        }
    }

    public static void CreateActor(int actorID, Vector3 position, Vector3 angle, int photonViewID)
    {
        photonView.RPC("PhotonEventSender_CreateActor", PhotonTargets.AllBuffered, actorID, position, angle, photonViewID);
    }

    [PunRPC]
    private void PhotonEventSender_CreateActor(int actorID, Vector3 position, Vector3 angle, int photonViewID)
    {
        Actor _actor = Engine.ActorManager.CreateActor(actorID, position, angle);
        PhotonView _photonView = _actor.GetComponent<PhotonView>();
        _photonView.viewID = photonViewID;
        if (OnActorCreated != null)
        {
            OnActorCreated(_photonView, _actor);
        }
    }

}
