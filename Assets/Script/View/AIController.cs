using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : View {

    [SerializeField] protected Actor m_actor = null;
    [SerializeField] protected AIBehaviour m_currentRunningBehaviour = null;

    protected bool m_isAttacking = false;

    protected virtual void Start()
    {
        if (m_currentRunningBehaviour != null)
        {
            ResetAI();
        }
        else
        {
            m_currentRunningBehaviour = ScriptableObject.CreateInstance<AIBehaviour>();
        }
    }

    public virtual void ResetAI()
    {
        m_currentRunningBehaviour = Engine.GetInstance<AIBehaviour>(m_currentRunningBehaviour);
        m_currentRunningBehaviour.Init(m_actor);
    }

    protected virtual void Update()
    {
        UpdateAIBehaviour();
    }

    private void UpdateAIBehaviour()
    {
        if (m_currentRunningBehaviour == null)
        {
            return;
        }

        m_currentRunningBehaviour.Update();

        if (m_currentRunningBehaviour.IsCanGoNext)
        {
            m_currentRunningBehaviour = m_currentRunningBehaviour.NextBehaviour;
            ResetAI();
        }
    }

}
