using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterActor : Actor {

    public bool IsShooting { get; protected set; }

    [SerializeField] protected Gun m_gun = null;

    private bool m_isAttacking = false;

    public void StartAttack()
    {
        m_attackCdTimer = m_gun.FireCdTime;
        m_isAttacking = true;
    }

    protected override void ParseMotion()
    {
        base.ParseMotion();
        IsShooting = m_isAttacking;

        if (m_inputDetecter.StartAttack)
        {
            StartAttack();
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

}
