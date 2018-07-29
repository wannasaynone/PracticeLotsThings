using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineStarter : MonoBehaviour {

    [SerializeField] private ActorPrefabManager m_actors = null;

    private ActorManager m_actorManager = null;

    private void Start()
    {
        m_actorManager = new ActorManager(m_actors);
        // TESTING
        ActorManager.CreateActor(0, Vector3.zero);
        ActorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0, Random.Range(-40f, 40f)));
    }

    private void Update()
    {
        TimerManager.Tick(Time.deltaTime);
    }

}
