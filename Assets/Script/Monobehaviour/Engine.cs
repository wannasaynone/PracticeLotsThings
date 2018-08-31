using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Engine : MonoBehaviour {

    [SerializeField] private ActorPrefabManager m_actors = null;
    
    public static Engine Instance { get { return m_instance; } }
    public static ActorFilter ActorFilter { get { return m_actorFilter; } }
    public static ActorManager ActorManager { get { return m_actorManager; } }
    public static GameManager GameManager { get { return m_gameManager; } }
    public static GameSetting GameSetting { get { return m_gameManager.GameSetting; } }

    private static Engine m_instance = null;
    private static GameManager m_gameManager = null;
    private static ActorManager m_actorManager = null;
    private static ActorFilter m_actorFilter = null;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_instance = this;
        GameDataManager.LoadGameData<GameSetting>("Data/GameSetting");
        GameDataManager.LoadGameData<CharacterStatus>("Data/CharacterStatus");
    }

    private void Start()
    {
        m_actorManager = new ActorManager(m_actors);
        m_gameManager = new GameManager(GameDataManager.GetGameData<GameSetting>(0));
        m_actorFilter = new ActorFilter();
    }

    private void Update()
    {
        m_gameManager.UpdateGame();
    }

    public void LoadScene(string name, Action onSceneLoaded)
    {
        StartCoroutine(IELoadScene(name, onSceneLoaded));
    }

    private IEnumerator IELoadScene(string name, Action onSceneLoaded = null)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if(onSceneLoaded != null)
        {
            onSceneLoaded();
        }
    }

    public static Vector3 GetRamdomPosition()
    {
        return new Vector3(UnityEngine.Random.Range(-40f, 40f), 0f, UnityEngine.Random.Range(-40f, 40f));
    }

    public static bool IsOutOfRange(Vector3 position)
    {
        if(GameSetting == null)
        {
            return false;
        }

        return (position.x > GameSetting.Edge_MaxX
            || position.x < GameSetting.Edge_MinX
            || position.z > GameSetting.Edge_MaxZ
            || position.z < GameSetting.Edge_MinZ);
    }

}
