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

    [SerializeField] private Target m_targetType = Target.Player;
    [SerializeField] private float m_detectRange = 5f;

    private Actor m_targetActor = null;

    public override void Init(Actor aiActor)
    {
        base.Init(aiActor);

        m_aiActor.ForceIdle();

        if (m_aiActor is ShooterActor)
        {
            ((ShooterActor)m_aiActor).StartAttack();
        }
    }

    public override void Update()
    {
        if (m_targetActor == null)
        {
            SetTarget();
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
