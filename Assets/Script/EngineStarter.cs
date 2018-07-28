using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineStarter : MonoBehaviour {

    private ActorManage m_actorManager = null;

    private void Start()
    {
        m_actorManager = new ActorManage();
    }

    private void Update()
    {
        TimerManager.Tick(Time.deltaTime);
    }

}
