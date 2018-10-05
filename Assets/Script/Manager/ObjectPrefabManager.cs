using UnityEngine;

namespace PracticeLotsThings.Manager
{
    [CreateAssetMenu(menuName = "Object Prefab Manager")]
    public class ObjectPrefabManager : ScriptableObject
    {
        public int Length { get { return m_objects.Length; } }

        [SerializeField] private GameObject[] m_objects = null;

        public GameObject GetPrefab(int prefabID)
        {
            if (prefabID < m_objects.Length && m_objects[prefabID] != null)
            {
                return m_objects[prefabID];
            }
            else
            {
                Debug.LogError("Object Prefab ID " + prefabID + " not exists");
                return null;
            }
        }

        public GameObject GetRandomObject()
        {
            return m_objects[Random.Range(0, m_objects.Length)];
        }
    }
}
