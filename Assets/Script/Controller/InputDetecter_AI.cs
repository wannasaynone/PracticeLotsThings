using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDetecter_AI : InputDetecter
{
    public enum AIState
    {
        Idle,
        Move,
        Run,
        Attack
    }

    public AIState CurrentAIState { get { return m_currentAIState; } }
    private AIState m_currentAIState = AIState.Idle;
    private Vector3 m_start = default(Vector3);
    private Vector3 m_end = default(Vector3);

    public override void DetectInput()
    {
        switch(CurrentAIState)
        {
            case AIState.Idle:
                {
                    break;
                }
            case AIState.Move:
                {
                    Move();
                    break;
                }
            case AIState.Run:
                {
                    Move();
                    break;
                }
        }
    }

    private void Move()
    {
        if (m_start.x > m_end.x)
        {
            m_leftKey_horizontal = CurrentAIState == AIState.Move ? -1f : -2f;
        }
        else
        {
            m_leftKey_horizontal = CurrentAIState == AIState.Move ? 1f : 2f;
        }

        if (m_start.z > m_end.z)
        {
            m_leftKey_vertical = CurrentAIState == AIState.Move ? -1f : -2f;
        }
        else
        {
            m_leftKey_vertical = CurrentAIState == AIState.Move ? 1f : 2f;
        }
    }

    public void SetMoveTo(Vector3 start, Vector3 end)
    {
        m_currentAIState = AIState.Run;
        m_start = start;
        m_end = end;
        KeyCPressed = false;
    }

    public void SetIdle()
    {
        m_currentAIState = AIState.Idle;
        m_leftKey_horizontal = 0f;
        m_leftKey_vertical = 0f;
        KeyCPressed = false;
    }

    public void SetAttack()
    {
        m_currentAIState = AIState.Attack;
        KeyCPressed = true;
    }
    
}
