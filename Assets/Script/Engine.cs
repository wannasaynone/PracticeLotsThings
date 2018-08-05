using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Engine : MonoBehaviour {

    [SerializeField] private ActorPrefabManager m_actors = null;
    [SerializeField] private GameSetting m_gameSetting = null;
    
    public static Engine Instance { get { return m_instance; } }
    public static ActorFilter ActorFilter { get { return m_actorFilter; } }
    public static ActorManager ActorManager { get { return m_actorManager; } }

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
        StartCoroutine(LoadScene("Test"));
    }

    private IEnumerator LoadScene(string name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        m_gameManager.InitGame(new NewGameSetting()
        {
            gameType = NewGameSetting.GameType.OneVsOne,
            startAs = ActorFilter.ActorType.Shooter,
            normalActorNumber = 50
        });
    }

    public static Vector3 GetRamdomPosition()
    {
        return new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f));
    }

}
