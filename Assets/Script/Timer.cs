using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

	// Update is called once per frame
	private void Update () {

        TimerManager.Tick(Time.deltaTime);
		
	}
}
