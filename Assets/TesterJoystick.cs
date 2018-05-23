using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterJoystick : MonoBehaviour {

	// Update is called once per frame
	void Update () {

        print("Square=" + Input.GetButton("Square") +",Cross=" + Input.GetButton("Cross") +",Circle=" + Input.GetButton("Circle") +",Triangle=" + Input.GetButton("Triangle"));

    }
}
