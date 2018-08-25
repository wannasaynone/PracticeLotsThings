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
    private static List<Actor> m_allActors = new List<Actor>();
    private ActorPrefabManager m_actorPrefabManager = null;

    public ActorManager(ActorPrefabManager actorPrefabManager)
    {
        m_actorPrefabManager = actorPrefabManager;
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
        }
        return _actor;
    }

    private void RemoveActorFromList(Actor actor)
    {
        m_allActors.Remove(actor);
    }

}
