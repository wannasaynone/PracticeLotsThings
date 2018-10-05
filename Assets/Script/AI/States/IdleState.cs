using UnityEngine;
using PracticeLotsThings.View.Actor;

namespace PracticeLotsThings.AI
{
    [CreateAssetMenu(menuName = "AI State/Idle")]
    public class IdleState : AIStateBase
    {

        public override void Init(Actor ai)
        {
            base.Init(ai);
            m_aiActor.ForceIdle();
        }

        public override void Update()
        {
            // do nothing when idle
            return;
        }

        public override void OnExit()
        {
            // do nothing when idle
            return;
        }
    }
}