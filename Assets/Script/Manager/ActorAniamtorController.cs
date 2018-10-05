using UnityEngine;

namespace PracticeLotsThings.Manager
{
    public class ActorAniamtorController
    {
        private const string PARA_NAME_HORIZONTAL = "horizontal";
        private const string PARA_NAME_VERTICAL = "vertical";
        private const string PARA_NAME_MOTION = "motion";

        private const string CLIP_NAME_ATTACKING = "Attacking";
        private const string CLIP_NAME_DIE = "Die";
        private const string CLIP_NAME_DIE_BACKWARD = "Die_Backward";

        private const string LAYER_NAME_BASE_LAYER = "Base Layer";
        private const string LAYER_NAME_ATTCKING_HANDS = "Attacking_Hands";
        private const string LAYER_NAME_ATTACKING_FULL_BODY = "Attacking_FullBody";

        private Animator m_animator = null;

        private bool m_lockUpdate = false;

        private float m_currentWeight_shootingLayer = 0f;

        public ActorAniamtorController(Animator animator)
        {
            m_animator = animator;
        }

        public void SetMovementAniamtion(float horizontal, float vertical, float motion)
        {
            if (m_lockUpdate || m_animator == null)
            {
                return;
            }
            m_animator.SetFloat(PARA_NAME_HORIZONTAL, horizontal);
            m_animator.SetFloat(PARA_NAME_VERTICAL, vertical);
            m_animator.SetFloat(PARA_NAME_MOTION, motion);
        }

        public void ForceRestartAttackAnimation()
        {
            m_animator.Play(CLIP_NAME_ATTACKING, m_animator.GetLayerIndex(LAYER_NAME_ATTCKING_HANDS), 0f);
            m_animator.Play(CLIP_NAME_ATTACKING, m_animator.GetLayerIndex(LAYER_NAME_ATTACKING_FULL_BODY), 0f);
        }

        public float LerpAttackingAnimation(bool isAttacking, float lerpT, bool onlyHands)
        {
            if (m_lockUpdate || m_animator == null)
            {
                return 0f;
            }

            if (isAttacking)
            {
                m_currentWeight_shootingLayer = Mathf.Lerp(m_currentWeight_shootingLayer, 1f, lerpT);
            }
            else
            {
                m_currentWeight_shootingLayer = Mathf.Lerp(m_currentWeight_shootingLayer, 0f, lerpT);
            }

            if (onlyHands)
            {
                m_animator.SetLayerWeight(m_animator.GetLayerIndex(LAYER_NAME_ATTCKING_HANDS), m_currentWeight_shootingLayer);
                return m_animator.GetLayerWeight(m_animator.GetLayerIndex(LAYER_NAME_ATTCKING_HANDS));
            }
            else
            {
                m_animator.SetLayerWeight(m_animator.GetLayerIndex(LAYER_NAME_ATTACKING_FULL_BODY), m_currentWeight_shootingLayer);
                return m_animator.GetLayerWeight(m_animator.GetLayerIndex(LAYER_NAME_ATTACKING_FULL_BODY));
            }
        }

        public void SetDie()
        {
            if (m_animator == null)
            {
                return;
            }
            m_animator.SetFloat(PARA_NAME_HORIZONTAL, 0f);
            m_animator.SetFloat(PARA_NAME_VERTICAL, 0f);
            m_animator.SetFloat(PARA_NAME_MOTION, 0f);
            m_animator.SetLayerWeight(m_animator.GetLayerIndex(LAYER_NAME_ATTCKING_HANDS), 0f);
            m_animator.SetLayerWeight(m_animator.GetLayerIndex(LAYER_NAME_ATTACKING_FULL_BODY), 0f);
            m_animator.Play(CLIP_NAME_DIE, m_animator.GetLayerIndex(LAYER_NAME_BASE_LAYER), 0f);
            m_lockUpdate = true;
        }

        public void SetBackToLife(float waitTime)
        {
            if (m_animator == null)
            {
                return;
            }
            m_lockUpdate = true;
            m_animator.Play(CLIP_NAME_DIE_BACKWARD, m_animator.GetLayerIndex(LAYER_NAME_BASE_LAYER), 0f);
            TimerManager.Schedule(waitTime, delegate { m_lockUpdate = false; });
        }
    }
}