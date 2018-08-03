using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager {

    public static Actor Player = null;
    private ActorManager m_actorManager = null;

    public GameManager(ActorManager actorManager)
    {
        m_actorManager = actorManager;
    }

    public void StartGame(int playerActorId)
    {
        // TESTING // 
        Player = m_actorManager.CreateActor(playerActorId);
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
        m_actorManager.CreateActor(1, new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f)));
    }

}
