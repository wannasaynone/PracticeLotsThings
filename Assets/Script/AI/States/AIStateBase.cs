using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NextAIState
{
    public List<AIConditionBase> conditions = new List<AIConditionBase>();
    public AIStateBase nextState = null;
}

public abstract class AIStateBase : ScriptableObject {

    [SerializeField] private NextAIState[] m_nextAIStates;

    protected Actor m_aiActor = null;
    public AIStateBase NextState { get; private set; }
    public bool IsCanGoNext { get { return CheckCanGoNext(); } }

    public virtual void Init(Actor ai)
    {
        m_aiActor = ai;
        InitConditions();
        NextState = null;
    }

    public abstract void Update();
    public abstract void OnExit();

    private void InitConditions()
    {
        if (m_nextAIStates != null && m_nextAIStates.Length > 0)
        {
            for (int _behaviourIndex = 0; _behaviourIndex < m_nextAIStates.Length; _behaviourIndex++)
            {
                for (int _conditionIndex = 0; _conditionIndex < m_nextAIStates[_behaviourIndex].conditions.Count; _conditionIndex++)
                {
                    if (m_nextAIStates[_behaviourIndex].conditions[_conditionIndex] != null)
                    {
                        m_nextAIStates[_behaviourIndex].conditions[_conditionIndex] = Instantiate(m_nextAIStates[_behaviourIndex].conditions[_conditionIndex]);
                        m_nextAIStates[_behaviourIndex].conditions[_conditionIndex].Init(m_aiActor);
                    }
                }
            }
        }
    }

    private void DestroyConditionClones()
    {
        if (m_nextAIStates != null && m_nextAIStates.Length > 0)
        {
            for (int _behaviourIndex = 0; _behaviourIndex < m_nextAIStates.Length; _behaviourIndex++)
            {
                for (int _conditionIndex = 0; _conditionIndex < m_nextAIStates[_behaviourIndex].conditions.Count; _conditionIndex++)
                {
                    if (m_nextAIStates[_behaviourIndex].conditions[_conditionIndex] != null)
                    {
                        Destroy(m_nextAIStates[_behaviourIndex].conditions[_conditionIndex]);
                    }
                }
            }
        }
    }

    protected void ForceGoTo(AIStateBase next)
    {
        NextState = next;
        DestroyConditionClones();
    }

    private bool CheckCanGoNext()
    {
        if (NextState != null)
        {
            return true;
        }

        if (m_nextAIStates == null || m_nextAIStates.Length == 0)
        {
            return false;
        }

        List<AIStateBase> _tempPassStates = new List<AIStateBase>();

        for (int _stateIndex = 0; _stateIndex < m_nextAIStates.Length; _stateIndex++)
        {
            if (m_nextAIStates[_stateIndex].conditions == null || m_nextAIStates[_stateIndex].conditions.Count == 0)
            {
                continue;
            }

            int _count = 0;
            for (int _conditionIndex = 0; _conditionIndex < m_nextAIStates[_stateIndex].conditions.Count; _conditionIndex++)
            {
                if (m_nextAIStates[_stateIndex].conditions[_conditionIndex].CheckPass())
                {
                    _count++;
                }
                
                if (_count == m_nextAIStates[_stateIndex].conditions.Count)
                {
                    _tempPassStates.Add(m_nextAIStates[_stateIndex].nextState);
                }
            }
        }

        if (_tempPassStates.Count > 0)
        {
            ForceGoTo(_tempPassStates[Random.Range(0, _tempPassStates.Count)]);
            return true;
        }
        else
        {
            return false;
        }
    }

}
