using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actor Prefab Manager")]
public class ActorPrefabManager : ScriptableObject {

    [SerializeField] private Actor[] m_actors = null;

    public Actor GetActorPrefab(int id)
    {
        for(int i = 0; i < m_actors.Length; i++)
        {
            if(m_actors[i].ID == id)
            {
                return m_actors[i];
            }
        }

        return null;
    }

}
