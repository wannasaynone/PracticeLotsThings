using System.Collections.Generic;
using UnityEngine;

public class ActorManager : Manager {

    private static Dictionary<Actor, CharacterStatus> m_actorToCharacterStatus = new Dictionary<Actor, CharacterStatus>();
    private ActorPrefabManager m_actorPrefabManager = null;

    public ActorManager(ActorPrefabManager actorPrefabManager)
    {
        m_actorPrefabManager = actorPrefabManager;
    }

    public Actor CreateActor(int id, Vector3 bornPosition = default(Vector3))
    {
        Actor _actor = m_actorPrefabManager.GetActorPrefab(id);
        if(_actor != null)
        {
            _actor = Object.Instantiate(_actor);
            _actor.transform.position = bornPosition;
            m_actorToCharacterStatus.Add(_actor, new CharacterStatus(GameDataManager.GetGameData<CharacterStatus>(_actor.StatusID)));
        }
        return _actor;
    }

    public CharacterStatus GetCharacterStatus(Actor actor)
    {
        if(!m_actorToCharacterStatus.ContainsKey(actor))
        {
            m_actorToCharacterStatus.Add(actor, new CharacterStatus(GameDataManager.GetGameData<CharacterStatus>(actor.StatusID)));
        }

        return m_actorToCharacterStatus[actor];
    }

}
