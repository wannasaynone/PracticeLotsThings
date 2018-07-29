using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI Condition/Distance")]
public class DistanceCondition : AIConditionBase {

    [SerializeField] private int m_observerID = 0;
    [SerializeField] private int m_aiActorID = 1;
    [SerializeField] private float m_distance = 0f;
    [SerializeField] private CompareCondition m_compareCondition = CompareCondition.Less;

    private Actor m_actorA = null;
    private Actor m_actorB = null;

    public override void Init()
    {
        m_actorA = ActorManager.GetActor(m_observerID);
        m_actorB = ActorManager.GetActor(m_aiActorID);
    }

    public override bool CheckPass()
    {
        if(m_actorA == null || m_actorB == null)
        {
            return false;
        }

        if(m_compareCondition == CompareCondition.Less)
        {
            return Vector3.Distance(m_actorA.transform.position, m_actorB.transform.position) <= m_distance;
        }
        else if(m_compareCondition == CompareCondition.More)
        {
            return Vector3.Distance(m_actorA.transform.position, m_actorB.transform.position) >= m_distance;
        }
        else
        {
            return false;
        }
    }

}
