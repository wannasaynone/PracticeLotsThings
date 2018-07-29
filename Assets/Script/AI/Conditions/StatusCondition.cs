using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI Condition/Status")]
public class StatusCondition : AIConditionBase {

    [SerializeField] private int m_obverseringActorID = 0;
    [SerializeField] private StatusType m_checkStatus = StatusType.HP;
    [SerializeField] private CompareCondition m_compareCondition = CompareCondition.Less;
    [SerializeField] private int m_value = 0;

    private Actor m_obverseringActor = null;

    public override void Init()
    {
        m_obverseringActor = ActorManager.GetActor(m_obverseringActorID);
    }

    public override bool CheckPass()
    {
        int _targetValue = 0;
        switch(m_checkStatus)
        {
            case StatusType.HP:
                {
                    _targetValue = ActorManager.GetCharacterStatus(m_obverseringActor).HP;
                    break;
                }
        }

        switch(m_compareCondition)
        {
            case CompareCondition.Less:
                {
                    return _targetValue <= m_value;
                }
            case CompareCondition.More:
                {
                    return _targetValue >= m_value;
                }
        }

        return false;
    }
}
