using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actor Prefab Manager")]
public class ActorPrefabManager : ScriptableObject {

    [SerializeField] private Actor[] m_actors = null;

    public Actor GetActorPrefab(int id)
    {
        if (id < m_actors.Length)
        {
            return m_actors[id];
        }
        else
        {
            Debug.LogError("Actor Prefab ID " + id + " not exists");
            return null;
        }
    }

}
