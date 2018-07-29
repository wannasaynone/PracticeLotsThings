using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineStarter : MonoBehaviour {

    [SerializeField] private ActorPrefabManager m_actors = null;

    private ActorManager m_actorManager = null;

    private void Start()
    {
        m_actorManager = new ActorManager(m_actors);
    }

    private void Update()
    {
        TimerManager.Tick(Time.deltaTime);
    }

}
