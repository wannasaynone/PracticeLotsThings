using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterActor : Actor {

    public bool IsShooting { get; protected set; }

    [SerializeField] protected Gun m_gun = null;

    protected override void ParseMotion()
    {
        base.ParseMotion();
        IsShooting = m_inputDetecter.IsAttacking;

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

}
