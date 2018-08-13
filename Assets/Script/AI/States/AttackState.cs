using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI State/Attack")]
public class AttackState : AIStateBase {

    public enum Target
    {
        Player,
        NearestNormal,
        NearestShooter,
        NearestZombie
    }

    [SerializeField] private AIStateBase m_idleState = null;
    [SerializeField] private Target m_targetType = Target.Player;

    private Actor m_targetActor = null;

#if UNITY_EDITOR
    public void SetData(AIStateBase idleState, Target targetType)
    {
        m_idleState = idleState;
        m_targetType = targetType;
    }
#endif

    public override void Init(Actor aiActor)
    {
        base.Init(aiActor);

        SetTarget();
        m_aiActor.ForceIdle();

        if (m_aiActor is ShooterActor)
        {
            ((ShooterActor)m_aiActor).StartAttack();
        }
    }

    public override void Update()
    {
        if (m_targetActor == null || m_targetActor.GetCharacterStatus().HP <= 0)
        {
            ForceGoTo(m_idleState);
            return;
        }

        m_aiActor.FaceTo(m_targetActor.transform.position);
        if (m_aiActor is ZombieActor)
        {
            ((ZombieActor)m_aiActor).Attack();
        }
    }

    public override void OnExit()
    {
        m_aiActor.ForceIdle();
    }

    private void SetTarget()
    {
        switch (m_targetType)
        {
            case Target.NearestNormal:
                {
                    m_targetActor = ActorFilter.GetNearestActor(ActorFilter.ActorType.Normal, m_aiActor);
                    break;
                }
            case Target.NearestShooter:
                {
                    m_targetActor = ActorFilter.GetNearestActor(ActorFilter.ActorType.Shooter, m_aiActor);
                    break;
                }
            case Target.NearestZombie:
                {
                    m_targetActor = ActorFilter.GetNearestActor(ActorFilter.ActorType.Zombie, m_aiActor);
                    break;
                }
            case Target.Player:
                {
                    m_targetActor = GameManager.Player;
                    break;
                }
        }
    }

}
