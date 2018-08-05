using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterActor : NormalActor {

    public float AttackCdTime { get { return m_gun.FireCdTime; } }

    [SerializeField] protected Gun m_gun = null;

    protected float m_attackCdTimer = -1f;

    protected virtual void Update()
    {
        if (m_isAttacking)
        {
            m_attackCdTimer -= Time.deltaTime;
            if (m_attackCdTimer <= 0)
            {
                ForceAttack();
            }
        }

        m_actorAniamtorController.LerpAttackingAnimation(m_isAttacking, 0.5f, true);
    }

    public void StartAttack()
    {
        m_isAttacking = true;
        m_attackCdTimer = m_gun.FireCdTime;
    }

    public void StopAttack()
    {
        m_isAttacking = false;
    }

    public void ForceAttack()
    {
        m_attackCdTimer = m_gun.FireCdTime;
        m_gun.Fire();
    }

}
