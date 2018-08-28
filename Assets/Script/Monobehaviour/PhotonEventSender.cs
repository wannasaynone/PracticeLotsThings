using UnityEngine;
using System;

public class PhotonEventSender : MonoBehaviour {

    public static event Action OnPhotonEventSenderCreated = null;
    public static event Action<Actor> OnActorCreated = null;
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

    public static void Fire(PhotonView shooterActorPhotonView, Vector3 firePosition)
    {
        photonView.RPC("PhotonEventSender_Fire", PhotonTargets.All, shooterActorPhotonView.viewID, firePosition);
    }

    [PunRPC]
    private void PhotonEventSender_CreateActor(int actorID, Vector3 position, Vector3 angle, int photonViewID)
    {
        Actor _actor = Engine.ActorManager.CreateActor(actorID, position, angle);
        _actor.PhotonView.viewID = photonViewID;
        if (OnActorCreated != null)
        {
            OnActorCreated(_actor);
        }
    }

    [PunRPC]
    private void PhotonEventSender_Fire(int shooterActorPhotonViewID, Vector3 fireEndPosition)
    {
        ((ShooterActor)Engine.ActorManager.GetPhotonActor(shooterActorPhotonViewID)).SyncAttack(fireEndPosition);
    }

}
