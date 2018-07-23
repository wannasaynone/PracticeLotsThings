using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineStarter : MonoBehaviour {


    private void Update()
    {
        TimerManager.Tick(Time.deltaTime);
    }

}
