using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActor : Actor {

    public bool IsMoving { get { return m_isMoving; } }

    [SerializeField] private AIBehaviour m_currentRunningBehaviour = null;

    private Vector3 m_goal = default(Vector3);
    private bool m_isMoving = false; // TODO: use enum to handle this

    public void ResetAI()
    {
        m_isMoving = false;
        IsShooting = false;
        MotionCurve = 0f;
        HorizontalMotion = 0f;
        VerticalMotion = 0f;
        m_currentRunningBehaviour.Init();
    }

    public void SetMoveTo(Vector3 goal, bool walkAndShoot = false)
    {
        m_goal = goal;
        m_isMoving = true;
        IsShooting = walkAndShoot;
    }

    public void StartAttack()
    {
        m_shootCdTimer = m_gun.FireCdTime;
        IsShooting = true;
    }

    protected override void Start()
    {
        base.Start();

        m_inputDetecter = ScriptableObject.CreateInstance<InputDetecter_AI>();

        if(m_currentRunningBehaviour != null)
        {
            m_currentRunningBehaviour.Init();
        }

        ResetAI();
    }

    protected override void Update()
    {
        UpdateAIBehaviour();
        Shoot();
        m_actorAniamtorController.Update();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void UpdateAIBehaviour()
    {
        if(m_currentRunningBehaviour == null)
        {
            return;
        }

        m_currentRunningBehaviour.Update();

        if(m_currentRunningBehaviour.IsCanGoNext)
        {
            m_currentRunningBehaviour = m_currentRunningBehaviour.NextBehaviour;
            ResetAI();
        }
    }

    private void Shoot()
    {
        if (IsShooting)
        {
            m_shootCdTimer -= Time.deltaTime;
            if (m_shootCdTimer <= 0)
            {
                m_shootCdTimer = m_gun.FireCdTime;
                m_gun.Fire();
            }
        }
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, m_goal) > 0.5f && m_isMoving)
        {
            Vector3 _dir = new Vector3((m_goal.x - transform.position.x), 0f, (m_goal.z - transform.position.z));
            transform.position += _dir.normalized * m_speed;

            if(!IsShooting)
            {
                FaceTo(m_goal);
            }

            if (Vector3.Distance(transform.position, m_goal) < 0.5f)
            {
                m_isMoving = false;
            }

            SetMovingAnimatorPara(_dir);
        }
    }

    private void SetMovingAnimatorPara(Vector3 direction)
    {
        if(m_isMoving)
        {
            MotionCurve = 1f;
            // TODO: motion when shooting...
            HorizontalMotion = 1f;
            VerticalMotion = 1f;
        }
        else
        {
            MotionCurve = 0f;
            HorizontalMotion = 0f;
            VerticalMotion = 0f;
        }
    }

}
