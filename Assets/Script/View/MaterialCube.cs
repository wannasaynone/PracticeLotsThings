using UnityEngine;
using UnityEngine.UI;

namespace PracticeLotsThings.View
{
    public class MaterialCube : InteractableObject
    {
        [SerializeField] private Light m_light = null;
        [SerializeField] private Slider m_progressBar = null;
        [Header("Properties")]
        [SerializeField] private int m_materialCount = 10;
        [SerializeField] private float m_lightMotionSpeed = 0.1f;
        [SerializeField] private float m_maxLightRange = 5f;
        [SerializeField] private float m_minLightRange = 1f;
        [SerializeField] private float m_collectTime = 1f;

        private bool m_goBigger = false;

        private void Start()
        {
            m_light.range = m_minLightRange;
            m_goBigger = true;
        }

        protected override void Update_WhileNormal()
        {
            if (m_goBigger)
            {
                m_light.range += m_lightMotionSpeed;
                if (m_light.range > m_maxLightRange)
                {
                    m_goBigger = false;
                }
            }
            else
            {
                m_light.range -= m_lightMotionSpeed;
                if (m_light.range < m_minLightRange)
                {
                    m_goBigger = true;
                }
            }
        }

        protected override void Update_OnInteractingStarted()
        {
            m_light.range = m_maxLightRange;
            m_progressBar.gameObject.SetActive(true);
            m_state = State.Interactering;
        }

        protected override void Update_WhileInteracting()
        {
            m_progressBar.value += m_progressBar.maxValue / (m_collectTime / Time.deltaTime);
            if (m_progressBar.value >= m_progressBar.maxValue)
            {
                m_state = State.Interacted;
            }
        }

        protected override void Update_WhileCanceling()
        {
            m_progressBar.value -= m_progressBar.maxValue / (m_collectTime / Time.deltaTime);
            if (m_progressBar.value <= 0f)
            {
                m_progressBar.value = 0f;
                m_progressBar.gameObject.SetActive(false);
                m_state = State.Normal;
            }
        }

        protected override void Update_OnInteracted()
        {
            m_progressBar.gameObject.SetActive(false);
            m_actor.GetCharacterStatus().AddMat(m_materialCount);
            m_state = State.Ending;
        }

        protected override void Update_InteractionEnding()
        {
            DestroyInteractableObject(this);
        }
    }
}

