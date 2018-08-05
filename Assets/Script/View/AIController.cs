using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : View {

    [SerializeField] protected Actor m_actor = null;
    [SerializeField] protected AIStateBase m_currentRunningState = null;

    protected virtual void Start()
    {
        if (m_currentRunningState != null)
        {
            m_currentRunningState = Instantiate(m_currentRunningState);
            m_currentRunningState.Init(m_actor);
        }
    }

    protected virtual void Update()
    {
        UpdateAIState();
    }

    private void UpdateAIState()
    {
        if (m_currentRunningState == null)
        {
            return;
        }

        m_currentRunningState.Update();

        if (m_currentRunningState.IsCanGoNext)
        {
            ChangeToNextState();
        }
    }

    private void ChangeToNextState()
    {
        m_currentRunningState.OnExit();

        ScriptableObject _waitForDestroy = m_currentRunningState;
        m_currentRunningState = Instantiate(m_currentRunningState.NextState);
        Destroy(_waitForDestroy);
        m_currentRunningState.Init(m_actor);
    }

}
