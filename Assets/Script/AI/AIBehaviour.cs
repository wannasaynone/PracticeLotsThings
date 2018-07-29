using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NextAIBehaviour
{
    public List<AIConditionBase> conditions = new List<AIConditionBase>();
    public AIBehaviour nextBehaviour = null;
}

[CreateAssetMenu(menuName = "AI Behaviour")]
public class AIBehaviour : ScriptableObject {

    [SerializeField] private AIStateBase m_state = null;
    [SerializeField] private NextAIBehaviour[] m_nextAIBehaviours;

    public AIBehaviour NextBehaviour { get; private set; }
    public bool IsCanGoNext { get { return CheckCanGoNext(); } }

    public void Init()
    {
        m_state.Init();

        for(int _behaviourIndex = 0; _behaviourIndex < m_nextAIBehaviours.Length; _behaviourIndex++)
        {
            for (int _conditionIndex = 0; _conditionIndex < m_nextAIBehaviours[_behaviourIndex].conditions.Count; _conditionIndex++)
            {
                m_nextAIBehaviours[_behaviourIndex].conditions[_conditionIndex].Init();
            }
        }

        NextBehaviour = null;
    }

    public void Update()
    {
        if(m_state != null)
        {
            m_state.Update();
        }
    }

    private bool CheckCanGoNext()
    {
        if(NextBehaviour != null)
        {
            return true;
        }

        List<AIBehaviour> _tempPassBehaviours = new List<AIBehaviour>();

        for (int _behaviourIndex = 0; _behaviourIndex < m_nextAIBehaviours.Length; _behaviourIndex++)
        {
            if (m_nextAIBehaviours[_behaviourIndex].conditions == null || m_nextAIBehaviours[_behaviourIndex].conditions.Count == 0)
            {
                continue;
            }

            for (int _conditionIndex = 0; _conditionIndex < m_nextAIBehaviours[_behaviourIndex].conditions.Count; _conditionIndex++)
            {
                int _count = 0;
                if (m_nextAIBehaviours[_behaviourIndex].conditions[_conditionIndex].CheckPass())
                {
                    _count++;
                }

                if(_count == m_nextAIBehaviours[_behaviourIndex].conditions.Count)
                {
                    _tempPassBehaviours.Add(m_nextAIBehaviours[_behaviourIndex].nextBehaviour);
                }
            }
        }

        if(_tempPassBehaviours.Count > 0)
        {
            NextBehaviour = _tempPassBehaviours[Random.Range(0, _tempPassBehaviours.Count)];
            return true;
        }
        else
        {
            return false;
        }
    }

}
