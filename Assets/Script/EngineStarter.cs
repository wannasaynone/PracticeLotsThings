using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineStarter : MonoBehaviour {

    GameManager gameManager = null;

    private void Start()
    {
        gameManager = new GameManager();
    }

    private void Update ()
    {
        gameManager.Update();
    }
}
