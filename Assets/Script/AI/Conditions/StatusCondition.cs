using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI Condition/Status")]
public class StatusCondition : AIConditionBase {

    public enum Target
    {
        Player,
        NearestNormal,
        NearestShooter,
        NearestZombie,
        Self
    }

    [SerializeField] private StatusType m_checkStatus = StatusType.HP;
    [SerializeField] private CompareCondition m_compareCondition = CompareCondition.Less;
    [SerializeField] private Target m_targetType = Target.Self;
    [SerializeField] private int m_value = 0;

    private Actor m_targetActor = null;
    private float m_currentTargetValue = 0f;

#if UNITY_EDITOR
    public void SetData(StatusType statusType, CompareCondition compareCondition, Target target, int value)
    {
        m_checkStatus = statusType;
        m_compareCondition = compareCondition;
        m_targetType = target;
        m_value = value;
    }
#endif

    public override void Init(Actor aiActor)
    {
        base.Init(aiActor);
    }

    public override bool CheckPass()
    {
        SetActorByType();

        switch (m_checkStatus)
        {
            case StatusType.HP:
                {
                    m_currentTargetValue = m_targetActor.GetCharacterStatus().HP;
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
        switch (m_targetType)
        {
            case Target.NearestNormal:
                {
                    m_targetActor = ActorFilter.GetNearestActor(ActorFilter.ActorType.Normal, m_aiActor);
                    break;
                }
            case Target.NearestShooter:
                {
                    m_targetActor = ActorFilter.GetNearestActor(ActorFilter.ActorType.Normal, m_aiActor);
                    break;
                }
            case Target.NearestZombie:
                {
                    m_targetActor = ActorFilter.GetNearestActor(ActorFilter.ActorType.Normal, m_aiActor);
                    break;
                }
            case Target.Player:
                {
                    m_targetActor = GameManager.Player;
                    break;
                }
            case Target.Self:
                {
                    m_targetActor = m_aiActor;
                    break;
                }
        }
    }
}
