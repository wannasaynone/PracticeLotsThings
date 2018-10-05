using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using PracticeLotsThings.Manager;
using PracticeLotsThings.Data;

namespace PracticeLotsThings.MainGameMonoBehaviour
{
    public sealed class Engine : MonoBehaviour
    {
        public static event Action OnGameInited = null;

        [SerializeField] private ActorPrefabManager m_actors = null;
        [SerializeField] private ObjectPrefabManager m_objects = null;
        [SerializeField] private bool m_isLocalhost = true;

        public static Engine Instance
        {
            get
            {
                if (m_instance == null)
                {
                    Debug.LogError("Fail To Use Engine, Engine Game Object Not Existed or Not Inited");
                }
                return m_instance;
            }
        }

        public static NetworkManager NetworkManager
        {
            get
            {
                if (m_networkManager == null)
                {
                    m_networkManager = new NetworkManager(true);
                }
                return m_networkManager;
            }
        }

        public static ActorFilter ActorFilter
        {
            get
            {
                if (m_actorFilter == null)
                {
                    m_actorFilter = new ActorFilter();
                }
                return m_actorFilter;
            }
        }

        public static ActorManager ActorManager
        {
            get
            {
                if (m_actorManager == null)
                {
                    Debug.LogError("Fail To Use ActorManager, Engine Game Object Not Existed or Not Inited");
                }
                return m_actorManager;
            }
        }
        public static GameManager GameManager { get { return m_gameManager; } }

        private static Engine m_instance = null;
        private static GameManager m_gameManager = null;
        private static ActorManager m_actorManager = null;
        private static ActorFilter m_actorFilter = null;
        private static NetworkManager m_networkManager = null;

        private void Awake()
        {
            if (m_instance != null)
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
            m_gameManager = new GameManager(m_objects);
            m_actorFilter = new ActorFilter();
            m_networkManager = new NetworkManager(m_isLocalhost);

            if(OnGameInited != null)
            {
                OnGameInited();
            }
        }

        private void Update()
        {
            m_gameManager.UpdateGame();
            m_networkManager.Update();
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

            if (onSceneLoaded != null)
            {
                onSceneLoaded();
            }
        }

        public static Vector3 GetRamdomPosition()
        {
            return new Vector3(UnityEngine.Random.Range(GameManager.GameSetting.Edge_MinX, GameManager.GameSetting.Edge_MaxX), 0f, UnityEngine.Random.Range(GameManager.GameSetting.Edge_MinZ, GameManager.GameSetting.Edge_MaxZ));
        }

        public static bool IsOutOfRange(Vector3 position)
        {
            if (GameManager.GameSetting == null)
            {
                return false;
            }

            return (position.x > GameManager.GameSetting.Edge_MaxX
                || position.x < GameManager.GameSetting.Edge_MinX
                || position.z > GameManager.GameSetting.Edge_MaxZ
                || position.z < GameManager.GameSetting.Edge_MinZ);
        }

        public void CreateObject(int objectID, Action<GameObject> onObjectCreated, Vector3 position, Vector3 angle)
        {
            if (NetworkManager.IsOffline)
            {
                SyncCreateObject(objectID, onObjectCreated, position, angle);
            }
            else
            {
                int _photonID = PhotonNetwork.AllocateViewID();
                PhotonEventSender.CreateObject(objectID, position, angle, _photonID);
            }
        }

        public void SyncCreateObject(int objectID, Action<GameObject> onObjectCreated, Vector3 position, Vector3 angle)
        {
            GameObject _object = Instantiate(m_objects.GetPrefab(objectID));
            _object.transform.position = position;
            _object.transform.eulerAngles = angle;
            if (onObjectCreated != null)
            {
                onObjectCreated(_object);
            }
        }

        public void DestroyObject(GameObject obj)
        {
            if (NetworkManager.IsOffline)
            {
                Destroy(obj);
            }
            else
            {
                PhotonNetwork.Destroy(obj);
            }
        }
    }
}