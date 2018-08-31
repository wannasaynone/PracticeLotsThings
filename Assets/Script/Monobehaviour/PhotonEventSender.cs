﻿using UnityEngine;
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
        if(photonView == null)
        {
            return;
        }
        photonView.RPC("PhotonEventSender_CreateActor", PhotonTargets.AllBuffered, actorID, position, angle, photonViewID);
    }

    public static void Fire(ShooterActor shooterActor, Vector3 firePosition)
    {
        if (photonView == null)
        {
            return;
        }
        photonView.RPC("PhotonEventSender_Fire", PhotonTargets.All, Engine.ActorManager.GetPhotonView(shooterActor).viewID, firePosition);
    }

    public static void Attack(ZombieActor zombieActor)
    {
        if (photonView == null)
        {
            return;
        }
        photonView.RPC("PhotonEventSender_Attack", PhotonTargets.All, Engine.ActorManager.GetPhotonView(zombieActor).viewID);
    }

    public static void ShowTransformedFromOthers(ZombieActor zombieActor)
    {
        if(photonView == null)
        {
            return;
        }
        photonView.RPC("PhotonEventSender_ShowTransformedFromOthers", PhotonTargets.All, Engine.ActorManager.GetPhotonView(zombieActor).viewID);
    }

    public static void DestroyActor(Actor actor)
    {
        if (photonView == null)
        {
            return;
        }
        photonView.RPC("PhotonEventSender_DestroyActor", PhotonTargets.All, Engine.ActorManager.GetPhotonView(actor).viewID);
    }

    public static void StartGame()
    {
        if (photonView == null)
        {
            return;
        }

        photonView.RPC("PhotonEventSender_StartGame", PhotonTargets.AllBuffered);
    }

    public static void EndGame(ActorFilter.ActorType wonActorType)
    {
        photonView.RPC("PhotonEventSender_EndGame", PhotonTargets.All, (int)wonActorType);
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

    [PunRPC]
    private void PhotonEventSender_Attack(int zombieActorPhotonViewID)
    {
        ((ZombieActor)Engine.ActorManager.GetPhotonActor(zombieActorPhotonViewID)).SyncAttack();
    }

    [PunRPC]
    private void PhotonEventSender_ShowTransformedFromOthers(int zombieActorPhotonViewID)
    {
        ((ZombieActor)Engine.ActorManager.GetPhotonActor(zombieActorPhotonViewID)).SyncIsTransformedFromOthers();
    }

    [PunRPC]
    private void PhotonEventSender_DestroyActor(int actorPhotonViewID)
    {
        Engine.ActorManager.SyncDestroyActor(Engine.ActorManager.GetPhotonActor(actorPhotonViewID));
    }

    [PunRPC]
    private void PhotonEventSender_StartGame()
    {
        Engine.GameManager.SyncGameStart();
    }

    [PunRPC]
    private void PhotonEventSender_EndGame(int wonActorType)
    {
        Engine.GameManager.SyncGameOver((ActorFilter.ActorType)wonActorType);
    }

}
