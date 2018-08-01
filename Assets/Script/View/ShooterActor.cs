using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterActor : Actor {

    public bool IsAttacking { get { return m_isAttacking; } }
    public float AttackCdTime { get { return m_gun.FireCdTime; } }

    [SerializeField] protected Gun m_gun = null;

    protected float m_attackCdTimer = -1f;
    protected bool m_isAttacking = false;

    protected override void ParseMotion()
    {
        base.ParseMotion();
        m_isAttacking = m_inputDetecter.IsAttacking;

        if (m_inputDetecter.StartAttack)
        {
            m_attackCdTimer = m_gun.FireCdTime;
        }

        if (m_inputDetecter.IsAttacking)
        {
            m_attackCdTimer -= Time.deltaTime;
            if (m_attackCdTimer <= 0)
            {
                m_attackCdTimer = m_gun.FireCdTime;
                m_gun.Fire();
            }
        }

        m_actorAniamtorController.LerpAttackingAnimation(m_inputDetecter.IsAttacking, 0.5f, true);
    }

    public void Attack()
    {
        m_gun.Fire();
    }

}
