using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI Condition/Distance")]
public class DistanceCondition : AIConditionBase {

    public enum Target
    {
        Player,
        NearestNormal,
        NearestShooter,
        NearestZombie
    }

    [SerializeField] private float m_distance = 0f;
    [SerializeField] private CompareCondition m_compareCondition = CompareCondition.Less;
    [SerializeField] private Target m_targetType = Target.Player;

    private Actor m_aiActor = null;
    private Actor m_targetActor = null;
    private float m_currentDistance = 0f;

    public override void Init(Actor aiActor)
    {
        m_aiActor = aiActor;
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

    public override bool CheckPass()
    {
        // Debug.Log(m_ai.name+"=>Player:" +Vector3.Distance(m_ai.transform.position, m_target.transform.position));
        if(m_aiActor == null || m_targetActor == null)
        {
            return false;
        }

        m_currentDistance = Vector3.Distance(m_aiActor.transform.position, m_targetActor.transform.position);

        if (m_compareCondition == CompareCondition.Less)
        {
            return m_currentDistance <= m_distance;
        }
        else if(m_compareCondition == CompareCondition.More)
        {
            return m_currentDistance >= m_distance;
        }
        else
        {
            return false;
        }
    }

}
