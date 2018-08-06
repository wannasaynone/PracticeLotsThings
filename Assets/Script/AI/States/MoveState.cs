using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI State/Move")]
public class MoveState : AIStateBase {

    public enum Target
    {
        Player,
        AwayFromPlayer,
        Random,
        NearestNormal,
        NearestShooter,
        NearestZombie,
        AwayFromNearestNormal,
        AwayFromNearestShooter,
        AwayFromNearestZombie
    }

    [SerializeField] private Target m_targetType = Target.Player;
    [SerializeField] private float m_detectRange = 5f;

    private Actor m_targetActor = null;
    private Vector3 m_goal = default(Vector3);

    public override void Init(Actor aiActor)
    {
        base.Init(aiActor);
        SetTarget();
    }

    public override void Update()
    {
        if(m_targetActor == null)
        {
            SetTarget();
        }

    }

    public override void OnExit()
    {
        m_aiActor.ForceIdle();
    }

    private void SetTarget()
    {
        switch(m_targetType)
        {
            case Target.AwayFromPlayer:
            case Target.Player:
                {
                    m_targetActor = GameManager.Player;
                    break;
                }
            case Target.AwayFromNearestNormal:
            case Target.NearestNormal:
                {
                    m_targetActor = ActorFilter.GetNearestActor(ActorFilter.ActorType.Normal, m_aiActor);
                    break;
                }
            case Target.AwayFromNearestShooter:
            case Target.NearestShooter:
                {
                    m_targetActor = ActorFilter.GetNearestActor(ActorFilter.ActorType.Shooter, m_aiActor);
                    break;
                }
            case Target.AwayFromNearestZombie:
            case Target.NearestZombie:
                {
                    m_targetActor = ActorFilter.GetNearestActor(ActorFilter.ActorType.Zombie, m_aiActor);
                    break;
                }
            case Target.Random:
                {
                    m_targetActor = m_aiActor;
                    break;
                }
        }
    }

    private void SetGoal()
    {
        switch (m_targetType)
        {
            case Target.AwayFromPlayer:
            case Target.AwayFromNearestShooter:
            case Target.AwayFromNearestNormal:
            case Target.AwayFromNearestZombie:
                {
                    m_goal = m_targetActor.transform.position + m_targetActor.transform.forward * m_detectRange + new Vector3(Random.Range(-m_detectRange, m_detectRange), 0f, Random.Range(-m_detectRange, m_detectRange));
                    break;
                }
            case Target.Player:
            case Target.NearestNormal:
            case Target.NearestShooter:
            case Target.NearestZombie:
                {
                    m_goal = m_targetActor.transform.position;
                    break;
                }
            case Target.Random:
                {
                    m_goal = m_targetActor.transform.position + new Vector3(Random.Range(-m_detectRange, m_detectRange), 0f, Random.Range(-m_detectRange, m_detectRange));
                    break;
                }
        }
    }

}
