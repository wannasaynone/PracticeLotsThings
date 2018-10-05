using UnityEngine;
using PracticeLotsThings.View.Actor;

namespace PracticeLotsThings.View.Controller
{
    public class ActorController_ZombieActor : ActorController
    {
        private ZombieActor m_zombieActor = null;

        protected virtual void Start()
        {
            if (m_actor is ZombieActor)
            {
                m_zombieActor = m_actor as ZombieActor;
            }
            else
            {
                Debug.LogError("MUST set a ZombieActor in ActorController_ZombieActor");
                m_zombieActor = null;
            }
        }

        protected override void Update()
        {
            base.Update();
            if (m_inputDetecter.IsStartingAttack)
            {
                m_zombieActor.StartAttack();
            }
        }
    }
}
