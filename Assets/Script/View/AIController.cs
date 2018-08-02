using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIController : View {

    [SerializeField] protected Actor m_actor = null;
    [SerializeField] protected AIBehaviour m_currentRunningBehaviour = null;

    protected bool m_isAttacking = false;

    protected virtual void Start()
    {
        if (m_currentRunningBehaviour != null)
        {
            m_currentRunningBehaviour.Init();
        }
        else
        {
            m_currentRunningBehaviour = ScriptableObject.CreateInstance<AIBehaviour>();
        }

        ResetAI();
    }

    public abstract void StartAttack();
    public abstract void StopAttack();

    public virtual void ResetAI()
    {
        m_currentRunningBehaviour.Init();
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
