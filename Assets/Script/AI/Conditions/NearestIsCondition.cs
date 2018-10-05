using UnityEngine;
using PracticeLotsThings.Manager;
using PracticeLotsThings.View.Actor;
using PracticeLotsThings.MainGameMonoBehaviour;

namespace PracticeLotsThings.AI
{
    [CreateAssetMenu(menuName = "AI Condition/Nearest Is")]
    public class NearestIsCondition : AIConditionBase
    {

        [SerializeField] private ActorFilter.ActorType m_targetType = ActorFilter.ActorType.All;

        private Actor m_targetActor = null;

#if UNITY_EDITOR
        public void SetData(ActorFilter.ActorType target)
        {
            m_targetType = target;
        }
#endif

        public override void Init(Actor ai)
        {
            base.Init(ai);
        }

        public override bool CheckPass()
        {
            m_targetActor = ActorFilter.GetNearestActor(Engine.ActorFilter.GetActors(new ActorFilter.FilteCondition()
            {
                filteBy = ActorFilter.FilteBy.Type,
                compareCondition = ActorFilter.CompareCondition.Is,
                actorType = ActorFilter.ActorType.All,
                value = 0
            }), m_aiActor);

            switch (m_targetType)
            {
                case ActorFilter.ActorType.Normal:
                    {
                        return m_targetActor is NormalActor && !(m_targetActor is ShooterActor);
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
}
