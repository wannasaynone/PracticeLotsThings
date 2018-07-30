using UnityEngine;

public class ZombieActor : Actor {

    public bool IsAttacking { get; protected set; }

    [SerializeField] float m_attackCd = 1f;
    [SerializeField] float m_attackAnimationTime = 2.2f;
    [SerializeField] float m_lerpBackTime = 0.5f;
    [SerializeField] float m_startAttackDashTime = 0.2f;
    [SerializeField] float m_attackDashSpeed = 5f;
    [SerializeField] float m_attackDashTime = 0.1f;

    private bool m_lerpBack = false;
    private bool m_isAttacking = false;
    private bool m_dash = false;

    private float m_orgainSpeed = 0f;

    private void Awake()
    {
        m_orgainSpeed = m_speed;
    }

    protected override void ParseMotion()
    {
        base.ParseMotion();

        if (m_inputDetecter.StartAttack && m_attackCdTimer < 0f)
        {
            m_lerpBack = false;
            m_isAttacking = true;

            m_actorAniamtorController.StartAttackAnimation();
            m_actorAniamtorController.LerpAttackingAnimation(true, 1f, false);
            m_attackCdTimer = m_attackCd + m_attackAnimationTime;
            TimerManager.Schedule(m_startAttackDashTime, delegate
            {
                m_dash = true;
                TimerManager.Schedule(m_attackDashTime, delegate { m_dash = false; m_movement = Vector3.zero;});
            });
            TimerManager.Schedule(m_attackAnimationTime, delegate { m_lerpBack = true; m_isAttacking = false; });
            
        }

        if(m_isAttacking)
        {
            HorizontalMotion = 0f;
            VerticalMotion = 0f;
            MotionCurve = 0f;
            m_movement = Vector3.zero;

            if (m_dash)
            {
                m_movement = transform.forward;
                m_speed = m_attackDashSpeed;
            }
            else
            {
                m_speed = m_orgainSpeed;
            }
        }

        if(m_lerpBack)
        {
            m_actorAniamtorController.LerpAttackingAnimation(false, m_lerpBackTime, false);
        }

        if (m_attackCdTimer > 0)
        {
            m_attackCdTimer -= Time.deltaTime;
        }
    }
}
