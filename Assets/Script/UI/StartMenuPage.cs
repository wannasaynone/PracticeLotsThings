using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuPage : View {

	public void StartGame()
    {
        Engine.Instance.StartGame();
    }

}
