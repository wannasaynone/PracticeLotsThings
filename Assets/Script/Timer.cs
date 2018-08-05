using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public static Timer Instance { get { return m_instance; } }
    private static Timer m_instance = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_instance = this;
    }

    // Update is called once per frame
    private void Update ()
    {
        TimerManager.Tick(Time.deltaTime);
	}
}
