using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController_ShooterActor : AIController {

    private ShooterActor m_shooterActor = null;

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

    public override void StartAttack()
    {
        m_isAttacking = true;
        m_shooterActor.StartAttack();
    }

    public override void StopAttack()
    {
        m_isAttacking = false;
        m_shooterActor.StopAttack();
    }

}
