using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController_ZombieActor : AIController {

    private ZombieActor m_zombieActor = null;

    private float m_attackCdTimer = -1f;

    protected override void Start()
    {
        base.Start();

        if (m_actor is ZombieActor)
        {
            m_zombieActor = m_actor as ZombieActor;
        }
        else
        {
            Debug.LogError("MUST set a ZombieActor in AIController_ZombieActor");
            m_zombieActor = null;
        }
    }

    public override void StartAttack()
    {
        if(m_isAttacking)
        {
            return;
        }
        m_isAttacking = true;
        m_zombieActor.Attack(StopAttack);
    }

    public override void StopAttack()
    {
        m_isAttacking = false;
    }

}
