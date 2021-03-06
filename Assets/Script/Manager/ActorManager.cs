﻿using System.Collections.Generic;
using UnityEngine;
using PracticeLotsThings.View.Actor;
using PracticeLotsThings.MainGameMonoBehaviour;

namespace PracticeLotsThings.Manager
{
    public class ActorManager : Manager
    {
        public static List<Actor> AllActors
        {
            get
            {
                return new List<Actor>(m_allActors);
            }
        }
        public static List<PhotonView> AllPhotonView
        {
            get
            {
                return new List<PhotonView>(m_allPhotonView);
            }
        }
        private static List<Actor> m_allActors = new List<Actor>();
        private static List<PhotonView> m_allPhotonView = new List<PhotonView>();
        private static Dictionary<Actor, PhotonView> m_actorToPhotonView = new Dictionary<Actor, PhotonView>();
        private static Dictionary<PhotonView, Actor> m_photonViewToActor = new Dictionary<PhotonView, Actor>();
        private ActorPrefabManager m_actorPrefabManager = null;

        private Dictionary<int, System.Action<Actor>> m_photonIdToOnActorCreated = new Dictionary<int, System.Action<Actor>>();

        public ActorManager(ActorPrefabManager actorPrefabManager)
        {
            m_actorPrefabManager = actorPrefabManager;
            PhotonEventSender.OnActorCreated += OnActorCreated;
        }

        public static bool IsMyActor(Actor actor)
        {
            if (m_actorToPhotonView.ContainsKey(actor))
            {
                return m_actorToPhotonView[actor].owner == PhotonNetwork.player || NetworkManager.IsOffline;
            }
            else
            {
                return false;
            }
        }

        public void CreateActor(int id, System.Action<Actor> onActorCreated, Vector3 bornPosition = default(Vector3), Vector3 bornAngle = default(Vector3))
        {
            if (NetworkManager.IsOffline)
            {
                SyncCreateActor(id, onActorCreated, bornPosition, bornAngle);
            }
            else
            {
                int _photonID = PhotonNetwork.AllocateViewID();
                m_photonIdToOnActorCreated.Add(_photonID, onActorCreated);
                PhotonEventSender.CreateActor(id, bornPosition, bornAngle, _photonID);
            }
        }

        private void OnActorCreated(Actor actor)
        {
            if (m_photonIdToOnActorCreated.ContainsKey(actor.PhotonView.viewID))
            {
                if (m_photonIdToOnActorCreated[actor.PhotonView.viewID] != null)
                {
                    m_photonIdToOnActorCreated[actor.PhotonView.viewID](actor);
                }
                m_photonIdToOnActorCreated.Remove(actor.PhotonView.viewID);
            }
        }

        public void SyncCreateActor(int id, System.Action<Actor> onActorCreated, Vector3 bornPosition = default(Vector3), Vector3 bornAngle = default(Vector3))
        {
            Actor _actor = m_actorPrefabManager.GetActorPrefab(id);
            if (_actor != null)
            {
                _actor = Object.Instantiate(_actor);
                _actor.transform.position = bornPosition;
                _actor.transform.eulerAngles = bornAngle;
                _actor.OnActorDestroyed += RemoveActorFromList;
                m_allActors.Add(_actor);
                if (_actor.PhotonView != null)
                {
                    m_allPhotonView.Add(_actor.PhotonView);
                    m_actorToPhotonView.Add(_actor, _actor.PhotonView);
                    m_photonViewToActor.Add(_actor.PhotonView, _actor);
                }
                if (onActorCreated != null)
                {
                    onActorCreated(_actor);
                }
            }
        }

        public Actor GetPhotonActor(int photonViewID)
        {
            for (int i = 0; i < m_allPhotonView.Count; i++)
            {
                if (m_allPhotonView[i].viewID == photonViewID)
                {
                    return m_photonViewToActor[m_allPhotonView[i]];
                }
            }
            return null;
        }

        public PhotonView GetPhotonView(Actor actor)
        {
            if (m_actorToPhotonView.ContainsKey(actor))
            {
                return m_actorToPhotonView[actor];
            }
            else
            {
                return null;
            }
        }

        public static void DestroyActor(Actor actor)
        {
            Object.Destroy(actor.gameObject);
            if (!NetworkManager.IsOffline)
            {
                PhotonEventSender.DestroyActor(actor);
            }
        }

        public void SyncDestroyActor(Actor actor)
        {
            if (actor.gameObject == null)
            {
                return;
            }
            Object.Destroy(actor.gameObject);
        }

        private void RemoveActorFromList(Actor actor)
        {
            m_allActors.Remove(actor);
            if (m_actorToPhotonView.ContainsKey(actor))
            {
                m_allPhotonView.Remove(m_actorToPhotonView[actor]);
                m_actorToPhotonView.Remove(actor);
            }
            if (m_photonViewToActor.ContainsValue(actor))
            {
                m_photonViewToActor.Remove(actor.PhotonView);
            }
        }
    }
}