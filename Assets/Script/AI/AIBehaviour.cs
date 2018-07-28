using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI Behaviour")]
public class AIBehaviour : ScriptableObject {

    [SerializeField] private AIStateBase m_state = null;
    [SerializeField] private List<AIConditionBase> m_conditions = new List<AIConditionBase>();
    [SerializeField] private AIBehaviour m_nextBehaviour = null;

    public AIBehaviour NextBehaviour { get { return m_nextBehaviour; } }
    public bool IsCanGoNext { get { return CheckCanGoNext(); } }

    public void Init()
    {
        m_state.Init();

        for(int i = 0; i < m_conditions.Count; i++)
        {
            m_conditions[i].Init();
        }
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
        if (m_conditions == null || m_conditions.Count == 0)
        {
            return false;
        }

        int _count = 0;
        for (int i = 0; i < m_conditions.Count; i++)
        {
            if (m_conditions[i].CheckPass())
            {
                _count++;
            }
        }

        return _count == m_conditions.Count;
    }

}
