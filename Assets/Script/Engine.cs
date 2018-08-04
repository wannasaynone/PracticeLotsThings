using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour {

    [SerializeField] private ActorPrefabManager m_actors = null;

    public static ActorFilter ActorFilter { get { return m_actorFilter; } }
    public static ActorManager ActorManager { get { return m_actorManager; } }

    private static GameManager m_gameManager = null;
    private static ActorManager m_actorManager = null;
    private static ActorFilter m_actorFilter = null;

    private void Awake()
    {
        GameDataManager.LoadGameData<CharacterStatus>("Data/CharacterStatus");
    }

    private void Start()
    {
        m_actorManager = new ActorManager(m_actors);
        m_gameManager = new GameManager(m_actorManager);
        m_actorFilter = new ActorFilter(m_actorManager);

        m_gameManager.StartGame();
    }
}
