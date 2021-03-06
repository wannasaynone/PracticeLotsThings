﻿using UnityEngine;
using PracticeLotsThings.AI;

namespace PracticeLotsThings.View.Controller
{
    public class AIController : View
    {
        public bool IsLocked { get { return m_lockDetect; } }

        [SerializeField] protected Actor.Actor m_actor = null;
        [SerializeField] protected AIStateBase m_currentRunningState = null;

        private bool m_lockDetect = false;

        public void Lock()
        {
            m_lockDetect = true;
        }

        public void Unlock()
        {
            m_lockDetect = false;
        }

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
            if (m_currentRunningState == null || m_lockDetect)
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
}
