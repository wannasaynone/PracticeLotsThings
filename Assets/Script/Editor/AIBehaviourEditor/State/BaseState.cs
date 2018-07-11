using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : ScriptableObject {

    public abstract void Init();
    public abstract void Tick();
	
}
