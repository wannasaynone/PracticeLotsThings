using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIConditionBase : ScriptableObject {

    public enum CompareCondition
    {
        More,
        Less
    }

    public enum StatusType
    {
        HP
    }

    public abstract void Init();
    public abstract bool CheckPass();

}
