using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController_ShooterActor : AIController {

    private ShooterActor m_shooterActor = null;

    private float m_attackCdTimer = -1f;

    protected override void Start()
    {
        base.Start();

        if(m_actor is ShooterActor)
        {
            m_shooterActor = m_actor as ShooterActor;
        }
        else
        {
            Debug.LogError("MUST set a ShooterActor in AIController_ShooterActor");
            m_shooterActor = null;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (m_isAttacking)
        {
            m_attackCdTimer -= Time.deltaTime;
            if (m_attackCdTimer <= 0)
            {
                m_attackCdTimer = m_shooterActor.AttackCdTime;
                m_shooterActor.Attack();
            }
        }
    }

    public override void StartAttack()
    {
        m_isAttacking = true;
        m_attackCdTimer = m_shooterActor.AttackCdTime;
    }

    public override void StopAttack()
    {
        m_isAttacking = false;
    }

}
