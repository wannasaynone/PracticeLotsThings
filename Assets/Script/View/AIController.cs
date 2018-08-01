using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIController : View {

    [SerializeField] protected Actor m_actor = null;
    [SerializeField] protected AIBehaviour m_currentRunningBehaviour = null;
    [SerializeField] protected ActorAniamtorController m_actorAnimatorController = null;

    protected Vector3 m_goal = default(Vector3);
    protected bool m_isMoving = false; // TODO: use enum to handle this
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

    public void MoveTo(Vector3 position)
    {
        m_goal = position;
        m_isMoving = true;
    }

    public abstract void StartAttack();
    public abstract void StopAttack();

    public virtual void ResetAI()
    {
        m_isMoving = false;
        m_currentRunningBehaviour.Init();
    }

    protected virtual void Update()
    {
        UpdateAIBehaviour();
    }

    private void FixedUpdate()
    {
        Move();
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

    private void Move()
    {
        if (Vector3.Distance(transform.position, m_goal) > 0.5f && m_isMoving)
        {
            Vector3 _dir = new Vector3((m_goal.x - transform.position.x), 0f, (m_goal.z - transform.position.z));
            transform.position += _dir.normalized * m_actor.MoveSpeed;

            if (!m_isAttacking)
            {
                m_actor.FaceTo(m_goal);
            }

            if (Vector3.Distance(transform.position, m_goal) < 0.5f)
            {
                m_isMoving = false;
                m_actorAnimatorController.SetMovementAniamtion(0f, 0f, 0f);
            }
            else
            {
                m_actorAnimatorController.SetMovementAniamtion(1f, 1f, 1f);
            }
        }
    }
}
