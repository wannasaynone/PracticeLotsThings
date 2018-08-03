using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI Condition/Status")]
public class StatusCondition : AIConditionBase {

    public enum Target
    {
        Player,
        Nearest,
        Self
    }

    [SerializeField] private StatusType m_checkStatus = StatusType.HP;
    [SerializeField] private CompareCondition m_compareCondition = CompareCondition.Less;
    [SerializeField] private Target m_targetType = Target.Self;
    [SerializeField] private int m_value = 0;
    [SerializeField] private float m_detectRange = 5f;

    private Actor m_ai = null;
    private Actor m_target = null;
    private float m_currentTargetValue = 0f;

    public override void Init(Actor ai)
    {
        m_ai = ai;
        SetActorByType();
    }

    public override bool CheckPass()
    {
        switch(m_checkStatus)
        {
            case StatusType.HP:
                {
                    m_currentTargetValue = Engine.ActorManager.GetCharacterStatus(m_target).HP;
                    break;
                }
        }

        switch (m_compareCondition)
        {
            case CompareCondition.Less:
                {
                    return m_currentTargetValue <= m_value;
                }
            case CompareCondition.More:
                {
                    return m_currentTargetValue >= m_value;
                }
        }

        return false;
    }

    private void SetActorByType()
    {
        switch(m_targetType)
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

                    m_target = ActorFilter.GetNearestActor(_actors, m_ai);
                    break;
                }
            case Target.Player:
                {
                    m_target = GameManager.Player;
                    break;
                }
            case Target.Self:
                {
                    m_target = m_ai;
                    break;
                }
        }
    }

}
