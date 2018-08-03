using UnityEngine;
using System;

public class ZombieActor : Actor {

    public bool IsAttacking { get { return m_isAttacking; } }
    public float AttackCdTime { get { return m_attackCd; } }

    [SerializeField] protected float m_attackCd = 1f;
    [SerializeField] protected float m_attackAnimationTime = 2.2f;
    [SerializeField] protected float m_lerpBackTime = 0.5f;
    [SerializeField] protected float m_startAttackDashTime = 0.2f;
    [SerializeField] protected float m_attackDashSpeed = 5f;
    [SerializeField] protected float m_attackDashTime = 0.1f;

    protected bool m_isEndingAttack = true;
    protected bool m_isDashing = false;

    protected float m_attackCdTimer = -1f;
    private float m_orgainSpeed = 0f;

    private void Awake()
    {
        m_orgainSpeed = m_speed;
    }

    protected virtual void Update()
    {
        if (m_isDashing)
        {
            Dash();
        }

        if (m_isEndingAttack)
        {
            m_actorAniamtorController.LerpAttackingAnimation(false, m_lerpBackTime, false);
        }

        if (m_attackCdTimer > 0)
        {
            m_attackCdTimer -= Time.deltaTime;
        }
    }

    public override void SetMotion(float horizontal, float vertical, float motionCurve)
    {
        if(m_isAttacking || m_isDashing || !m_isEndingAttack)
        {
            return;
        }
        base.SetMotion(horizontal, vertical, motionCurve);
    }

    public override void FaceTo(Vector3 targetPosition)
    {
        if (m_isAttacking || m_isDashing || !m_isEndingAttack)
        {
            return;
        }
        base.FaceTo(targetPosition);
    }

    private void Dash()
    {
        m_movement = transform.forward;
        m_speed = m_attackDashSpeed;
    }

    private void StopDash()
    {
        m_speed = m_orgainSpeed;
        m_isDashing = false;
        m_movement = Vector3.zero;
    }

    public void Attack()
    {
        if (m_attackCdTimer > 0f)
        {
            return;
        }

        m_isEndingAttack = false;
        m_isAttacking = true;

        m_actorAniamtorController.ForceRestartAttackAnimation();
        m_actorAniamtorController.LerpAttackingAnimation(true, 1f, false);

        m_attackCdTimer = m_attackCd + m_attackAnimationTime;
        TimerManager.Schedule(m_startAttackDashTime, delegate
        {
            m_isDashing = true;
            TimerManager.Schedule(m_attackDashTime, StopDash);
        });
        TimerManager.Schedule(m_attackAnimationTime, delegate 
        {
            m_isEndingAttack = true;
            m_isAttacking = false;
        });
    }

}
