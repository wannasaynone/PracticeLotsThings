using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController_ShooterActor : ActorController {

    private ShooterActor m_shooterActor = null;

    protected virtual void Start()
    {
        if (m_actor is ShooterActor)
        {
            m_shooterActor = m_actor as ShooterActor;
        }
        else
        {
            Debug.LogError("MUST set a ShooterActor in ActorController_ShooterActor");
            m_shooterActor = null;
        }
    }

    protected override void Update()
    {
        base.Update();
        if(m_inputDetecter.IsStartingAttack)
        {
            m_shooterActor.StartAttack();
        }
        if(!m_inputDetecter.IsAttacking)
        {
            m_shooterActor.StopAttack();
        }
    }

}
