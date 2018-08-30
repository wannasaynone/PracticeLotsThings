using System.Collections.Generic;
using UnityEngine;

public class ActorManager : Manager {

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

    public ActorManager(ActorPrefabManager actorPrefabManager)
    {
        m_actorPrefabManager = actorPrefabManager;
    }

    public static bool IsMyActor(Actor actor)
    {
        return m_actorToPhotonView[actor].owner == PhotonNetwork.player || NetworkManager.IsOffline;
    }

    public Actor CreateActor(int id, Vector3 bornPosition = default(Vector3), Vector3 bornAngle = default(Vector3))
    {
        Actor _actor = m_actorPrefabManager.GetActorPrefab(id);
        if(_actor != null)
        {
            _actor = Object.Instantiate(_actor);
            _actor.transform.position = bornPosition;
            _actor.transform.eulerAngles = bornAngle;
            _actor.OnActorDestroyed += RemoveActorFromList;
            m_allActors.Add(_actor);
            PhotonView _photonView = _actor.PhotonView;
            if(_photonView != null)
            {
                m_allPhotonView.Add(_photonView);
                m_actorToPhotonView.Add(_actor, _photonView);
                m_photonViewToActor.Add(_photonView, _actor);
            }
        }
        return _actor;
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
        if(m_actorToPhotonView.ContainsKey(actor))
        {
            return m_actorToPhotonView[actor];
        }
        else
        {
            return null;
        }
    }

    public void DestroyActor(Actor actor)
    {
        Object.Destroy(actor.gameObject);
        if(!NetworkManager.IsOffline)
        {
            PhotonEventSender.DestroyActor(actor);
        }
    }

    public void SyncDestroyActor(Actor actor)
    {
        Object.Destroy(actor.gameObject);//
    }

    private void RemoveActorFromList(Actor actor)
    {
        m_allActors.Remove(actor);
        if(m_actorToPhotonView.ContainsKey(actor))
        {
            m_allPhotonView.Remove(m_actorToPhotonView[actor]);
            m_actorToPhotonView.Remove(actor);
        }
        if(m_photonViewToActor.ContainsValue(actor))
        {
            m_photonViewToActor.Remove(actor.PhotonView);
        }
    }

}
