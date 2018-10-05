using UnityEngine;

namespace PracticeLotsThings.MainGameMonoBehaviour
{
    public class DDOL : MonoBehaviour
    {
        // Use this for initialization
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

