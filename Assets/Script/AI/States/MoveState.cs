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
        Nearest
    }

    [SerializeField] private Target m_targetType = Target.Player;
    [SerializeField] private float m_detectRange = 5f;

    private Actor m_target = null;
    private Vector3 m_goal = default(Vector3);

    public override void Init(Actor ai)
    {
        base.Init(ai);
        switch (m_targetType)
        {
            case Target.Nearest:
                {
                    List<Actor> _actors = Engine.ActorFilter.GetActors(new ActorFilter.FilteCondition()
                    {
                        filteBy = ActorFilter.FilteBy.Distance,
                        compareCondition = ActorFilter.CompareCondition.Less,
                        actorType = ActorFilter.ActorType.All, //TESTING TODO: need to set type by ai
                        value = m_detectRange
                    });

                    m_target = ActorFilter.GetNearestActor(_actors, m_aiActor);
                    break;
                }
            case Target.Random:
            case Target.AwayFromPlayer:
                {
                    SetGoal();
                    m_target = GameManager.Player;
                    break;
                }
            case Target.Player:
                {
                    m_target = GameManager.Player;
                    break;
                }
        }
    }

    public override void Update()
    {
        switch(m_targetType)
        {
            case Target.Random:
            case Target.AwayFromPlayer:
                {
                    if (Vector3.Distance(m_aiActor.transform.position, m_goal) < 0.1f
                        || m_targetType == Target.AwayFromPlayer && Vector3.Distance(m_aiActor.transform.position, m_target.transform.position) > m_detectRange)
                    {
                        SetGoal();
                    }
                    m_aiActor.SetMoveTo(m_goal);
                    m_aiActor.FaceTo(m_goal);
                    break;
                }
            default:
                {
                    m_aiActor.SetMoveTo(m_target.transform.position);
                    m_aiActor.FaceTo(m_target.transform.position);
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
        }
    }

}
