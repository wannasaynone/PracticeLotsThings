using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Engine : MonoBehaviour {

    public static NewGameSetting NewGameSetting = null;

    [SerializeField] private ActorPrefabManager m_actors = null;
    [SerializeField] private GameSetting m_gameSetting = null;
    
    public static Engine Instance { get { return m_instance; } }
    public static ActorFilter ActorFilter { get { return m_actorFilter; } }
    public static ActorManager ActorManager { get { return m_actorManager; } }

    private static Engine m_instance = null;
    private static GameManager m_gameManager = null;
    private static ActorManager m_actorManager = null;
    private static ActorFilter m_actorFilter = null;

    public GameSetting GameSetting { get { return m_gameSetting; } }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_instance = this;
        GameDataManager.LoadGameData<CharacterStatus>("Data/CharacterStatus");
    }

    private void Start()
    {
        m_actorManager = new ActorManager(m_actors);
        m_gameManager = new GameManager(m_actorManager, m_gameSetting);
        m_actorFilter = new ActorFilter(m_actorManager);
    }

    public void StartGame()
    {
        if(NewGameSetting == null)
        {
            NewGameSetting = ScriptableObject.CreateInstance<NewGameSetting>();
            NewGameSetting.gameType = NewGameSetting.GameType.OneVsOne;
            NewGameSetting.startAs = ActorFilter.ActorType.Shooter;
            NewGameSetting.normalActorNumber = 50;
        }

        LoadScene("Test", delegate
        {
            m_gameManager.InitGame(NewGameSetting);
        });
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

}
