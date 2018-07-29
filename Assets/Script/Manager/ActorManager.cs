using System.Collections.Generic;
using UnityEngine;

public class ActorManager : Manager {

    private static List<Actor> m_actors = new List<Actor>();
    private static Dictionary<Actor, CharacterStatus> m_actorToCharacterStatus = new Dictionary<Actor, CharacterStatus>();
    private static ActorPrefabManager m_actorPrefabManager = null;

    public ActorManager(ActorPrefabManager actorPrefabManager)
    {
        m_actorPrefabManager = actorPrefabManager;
    }

    public static Actor CreateActor(int id, Vector3 bornPosition = default(Vector3))
    {
        Actor _actor = m_actorPrefabManager.GetActorPrefab(id);
        if(_actor != null)
        {
            _actor = Object.Instantiate(_actor);
            _actor.transform.position = bornPosition;
            m_actors.Add(_actor);
            // TODO: get character status from game data manager
            m_actorToCharacterStatus.Add(_actor, new CharacterStatus() { HP = 100 });
        }
        return _actor;
    }

	public static Actor GetActor(int id)
    {
        return m_actors.Find(x => x.ID == id);
    }

    public static CharacterStatus GetCharacterStatus(int id)
    {
        return m_actorToCharacterStatus[m_actors.Find(x => x.ID == id)];
    }

    public static CharacterStatus GetCharacterStatus(Actor actor)
    {
        return m_actorToCharacterStatus[actor];
    }

}
