using UnityEngine;
using PracticeLotsThings.View.Actor;
using PracticeLotsThings.Manager;

namespace PracticeLotsThings.AI
{
    [CreateAssetMenu(menuName = "AI Condition/Distance")]
    public class DistanceCondition : AIConditionBase
    {

        public enum Target
        {
            Player,
            NearestNormal,
            NearestShooter,
            NearestZombie
        }

        [SerializeField] private Target m_targetType = Target.Player;
        [SerializeField] private CompareCondition m_compareCondition = CompareCondition.Less;
        [SerializeField] private float m_distance = 0f;

        private Actor m_targetActor = null;
        private float m_currentDistance = 0f;

#if UNITY_EDITOR
        public void SetData(Target target, CompareCondition compareCondition, float value)
        {
            m_targetType = target;
            m_compareCondition = compareCondition;
            m_distance = value;
        }
#endif

        public override void Init(Actor aiActor)
        {
            base.Init(aiActor);
        }

        public override bool CheckPass()
        {
            SetActorByType();

            if (m_aiActor == null || m_targetActor == null)
            {
                return false;
            }

            m_currentDistance = Vector3.Distance(m_aiActor.transform.position, m_targetActor.transform.position);

            if (m_compareCondition == CompareCondition.Less)
            {
                return m_currentDistance <= m_distance;
            }
            else if (m_compareCondition == CompareCondition.More)
            {
                return m_currentDistance >= m_distance;
            }
            else
            {
                return false;
            }
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
    }
}
