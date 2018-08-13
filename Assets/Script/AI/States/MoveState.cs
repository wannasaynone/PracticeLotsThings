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

    [SerializeField] private AIStateBase m_idleState = null;
    [SerializeField] private Target m_targetType = Target.Player;
    [SerializeField] private float m_detectRange = 5f;

    private Actor m_targetActor = null;
    private Vector3 m_goal = default(Vector3);

#if UNITY_EDITOR
    public void SetData(AIStateBase idleState, Target targetType, float detectRange)
    {
        m_idleState = idleState;
        m_targetType = targetType;
        m_detectRange = detectRange;
    }
#endif

    public override void Init(Actor aiActor)
    {
        base.Init(aiActor);
        m_goal = m_aiActor.transform.position;
        SetTarget();
    }

    public override void Update()
    {
        if (m_targetActor == null || m_targetActor.GetCharacterStatus().HP <= 0)
        {
            ForceGoTo(m_idleState);
            return;
        }

        TryToSetGoal();
        m_aiActor.SetMoveTo(m_goal);
        m_aiActor.FaceTo(m_goal);
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

    private void TryToSetGoal()
    {
        if (Vector3.Distance(m_goal, m_aiActor.transform.position) > Actor.GOAL_DETECT_RANGE)
        {
            return;
        }

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

        if(Engine.IsOutOfRange(m_goal))
        {
            m_goal = m_aiActor.transform.position;
            TryToSetGoal();
        }
    }

}
