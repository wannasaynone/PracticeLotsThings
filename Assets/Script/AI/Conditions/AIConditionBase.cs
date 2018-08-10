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

    protected Actor m_aiActor = null;
    public virtual void Init(Actor ai)
    {
        m_aiActor = ai;
    }

    public abstract bool CheckPass();
}
