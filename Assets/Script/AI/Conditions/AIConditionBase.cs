using UnityEngine;
using PracticeLotsThings.View.Actor;

namespace PracticeLotsThings.AI
{
    public abstract class AIConditionBase : ScriptableObject
    {
        public enum CompareCondition
        {
            More,
            Less
        }

        public enum StatusType
        {
            HP
        }

        protected Actor m_aiActor = null;
        public virtual void Init(Actor ai)
        {
            m_aiActor = ai;
        }

        public abstract bool CheckPass();
    }
}
