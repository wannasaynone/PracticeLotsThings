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
        NearestZombie
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

        switch(m_targetType)
        {
            case Target.Random:
            case Target.AwayFromPlayer:
                {
                    if (Vector3.Distance(m_aiActor.transform.position, m_goal) < 0.1f
                        || m_targetType == Target.AwayFromPlayer && Vector3.Distance(m_aiActor.transform.position, m_targetActor.transform.position) > m_detectRange)
                    {
                        SetGoal();
                    }
                    m_aiActor.SetMoveTo(m_goal);
                    m_aiActor.FaceTo(m_goal);
                    break;
                }
            default:
                {
                    m_aiActor.SetMoveTo(m_targetActor.transform.position);
                    m_aiActor.FaceTo(m_targetActor.transform.position);
                    break;
                }
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

    private void SetGoal()
    {
        switch (m_targetType)
        {
            case Target.Random:
                {
                    m_goal = new Vector3(Random.Range(-40f, 40f), 0f, Random.Range(-40f, 40f));
                    break;
                }
            case Target.AwayFromPlayer:
                {
                    m_goal = m_aiActor.transform.position - (m_aiActor.transform.forward * 5f);
                    break;
                }
            default:
                {
                    m_goal = m_targetActor.transform.position;
                    break;
                }
        }
    }

}
