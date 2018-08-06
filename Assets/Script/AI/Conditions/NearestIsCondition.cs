using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI Condition/Nearest Is")]
public class NearestIsCondition : AIConditionBase {

    [SerializeField] private ActorFilter.ActorType m_targetType = ActorFilter.ActorType.All;

    private Actor m_targetActor = null;

    public override void Init(Actor ai)
    {
        base.Init(ai);
        m_targetActor = ActorFilter.GetNearestActor(Engine.ActorFilter.GetActors(new ActorFilter.FilteCondition()
        {
            filteBy = ActorFilter.FilteBy.Type,
            compareCondition = ActorFilter.CompareCondition.Is,
            actorType = ActorFilter.ActorType.All
        }), m_aiActor);
    }

    public override bool CheckPass()
    {
        switch(m_targetType)
        {
            case ActorFilter.ActorType.Normal:
                {
                    return m_targetActor is NormalActor;
                }
            case ActorFilter.ActorType.Shooter:
                {
                    return m_targetActor is ShooterActor;
                }
            case ActorFilter.ActorType.Zombie:
                {
                    return m_targetActor is ZombieActor;
                }
        }

        return false;
    }


}
