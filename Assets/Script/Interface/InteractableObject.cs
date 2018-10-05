using UnityEngine;
using PracticeLotsThings.Manager;

namespace PracticeLotsThings.View
{
    public abstract class InteractableObject : View
    {
        public static void InstantiateInteractableObject(InteractableObject obj, Vector3 position, Vector3 angle)
        {
            if (NetworkManager.IsOffline)
            {
                Instantiate(obj, position, Quaternion.Euler(angle));
            }
            else
            {
                PhotonNetwork.Instantiate(obj.name, position, Quaternion.Euler(angle), 0);
            }
        }

        public static void DestroyInteractableObject(InteractableObject obj)
        {
            if (NetworkManager.IsOffline)
            {
                Destroy(obj.gameObject);
            }
            else
            {
                PhotonNetwork.Destroy(obj.gameObject);
            }
        }

        protected enum State
        {
            Normal,
            StartInteracting,
            Interactering,
            Canceling,
            Interacted,
            Ending
        }

        protected Actor.Actor m_actor = null;

        public void StartInteract()
        {
            m_state = State.StartInteracting;
        }

        public void StopInteract()
        {
            m_state = State.Canceling;
            m_actor = null;
        }

        public void SetActor(Actor.Actor actor)
        {
            if (m_actor != null)
            {
                return;
            }
            m_actor = actor;
        }

        protected abstract void Update_WhileNormal();
        protected abstract void Update_OnInteractingStarted();
        protected abstract void Update_WhileInteracting();
        protected abstract void Update_WhileCanceling();
        protected abstract void Update_OnInteracted();
        protected abstract void Update_InteractionEnding();

        protected State m_state = State.Normal;

        private void Update()
        {
            switch (m_state)
            {
                case State.Normal:
                    {
                        Update_WhileNormal();
                        break;
                    }
                case State.StartInteracting:
                    {
                        Update_OnInteractingStarted();
                        break;
                    }
                case State.Interactering:
                    {
                        Update_WhileInteracting();
                        break;
                    }
                case State.Canceling:
                    {
                        Update_WhileCanceling();
                        break;
                    }
                case State.Interacted:
                    {
                        Update_OnInteracted();
                        break;
                    }
                case State.Ending:
                    {
                        Update_InteractionEnding();
                        break;
                    }
            }
        }
    }
}