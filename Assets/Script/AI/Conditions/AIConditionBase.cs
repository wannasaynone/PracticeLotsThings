using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIConditionBase : ScriptableObject {

    public abstract void Init();
    public abstract bool CheckPass();

}
