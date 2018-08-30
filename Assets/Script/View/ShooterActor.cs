using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterActor : NormalActor {

    public float AttackCdTime { get { return m_gun.FireCdTime; } }

    [SerializeField] protected Gun m_gun = null;

    protected float m_attackCdTimer = -1f;

    protected override void Update()
    {
        base.Update();
        if (m_isAttacking)
        {
            m_attackCdTimer -= Time.deltaTime;
            if (m_attackCdTimer <= 0)
            {
                {
                    ForceAttack();
                }
            }
        }

        m_actorAniamtorController.LerpAttackingAnimation(m_isAttacking || isSyncAttacking, 0.5f, true);
    }

    public void StartAttack()
    {
        m_isAttacking = true;
        isSyncAttacking = true;
        m_attackCdTimer = m_gun.FireCdTime;
    }

    public void StopAttack()
    {
        m_isAttacking = false;
        isSyncAttacking = false;
    }

    public void ForceAttack()
    {
        m_attackCdTimer = m_gun.FireCdTime;
        Vector3 _firePosition = m_gun.Fire();
        if(!NetworkManager.IsOffline)
        {
            PhotonEventSender.Fire(this, _firePosition);
        }
    }

    public void SyncAttack(Vector3 position)
    {
        if(ActorManager.IsMyActor(this))
        {
            return;
        }
        m_gun.FireToSpecificPoint(position);
    }

}
