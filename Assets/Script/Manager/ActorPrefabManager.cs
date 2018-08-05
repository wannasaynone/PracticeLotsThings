using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actor Prefab Manager")]
public class ActorPrefabManager : ScriptableObject {

    [SerializeField] private Actor[] m_actors = null;

    public Actor GetActorPrefab(int prefabID)
    {
        if (prefabID < m_actors.Length && m_actors[prefabID] != null)
        {
            return m_actors[prefabID];
        }
        else
        {
            Debug.LogError("Actor Prefab ID " + prefabID + " not exists");
            return null;
        }
    }

}
