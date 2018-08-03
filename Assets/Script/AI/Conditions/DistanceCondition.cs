using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI Condition/Distance")]
public class DistanceCondition : AIConditionBase {

    public enum Target
    {
        Player,
        Nearest
    }

    [SerializeField] private float m_distance = 0f;
    [SerializeField] private CompareCondition m_compareCondition = CompareCondition.Less;
    [SerializeField] private Target m_targetType = Target.Player;

    private Actor m_ai = null;
    private Actor m_target = null;
    private float m_currentDistance = 0f;

    public override void Init(Actor ai)
    {
        m_ai = ai;
        switch (m_targetType)
        {
            case Target.Nearest:
                {
                    List<Actor> _actors = Engine.ActorFilter.GetActors(new ActorFilter.FilteCondition()
                    {
                        filteBy = ActorFilter.FilteBy.Distance,
                        compareCondition = ActorFilter.CompareCondition.Less,
                        actorType = ActorFilter.ActorType.All, //TESTING TODO: need to set type by ai
                        value = m_distance * 2f
                    });

                    m_target = ActorFilter.GetNearestActor(_actors, m_ai);
                    break;
                }
            case Target.Player:
                {
                    m_target = GameManager.Player;
                    break;
                }
        }
    }

    public override bool CheckPass()
    {
        // Debug.Log(m_ai.name+"=>Player:" +Vector3.Distance(m_ai.transform.position, m_target.transform.position));
        if(m_ai == null || m_target == null)
        {
            return false;
        }

        m_currentDistance = Vector3.Distance(m_ai.transform.position, m_target.transform.position);

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
