using UnityEngine;
using PracticeLotsThings.Manager;
using PracticeLotsThings.MainGameMonoBehaviour;

namespace PracticeLotsThings.View.Actor
{
    public class TowerActor : ShooterActor
    {

        [SerializeField] private GameObject m_upperCube = null;
        [SerializeField] private GameObject m_downCube = null;
        [SerializeField] private float m_maxRotateSpeed = 0.1f;
        [SerializeField] private float m_lerpTime = 1f;
        [SerializeField] private float m_changeTime = 1.5f;

        private float m_currentRotateSpeed = 0f;
        private float m_changeTimer = 1f;

        private bool m_goPositive = true;

        protected override void Update()
        {
            if (m_isAttacking)
            {
                m_attackCdTimer -= Time.deltaTime;
                if (m_attackCdTimer <= 0)
                {
                    {
                        ForceAttack();
                    }
                }
            }
        }

        protected override void FixedUpdate()
        {
            if (m_goPositive)
            {
                if (m_currentRotateSpeed < m_maxRotateSpeed)
                {
                    m_currentRotateSpeed += m_maxRotateSpeed / (m_lerpTime / Time.fixedDeltaTime);
                }
                else
                {
                    RunChangeSpeedTimer();
                }
            }
            else
            {
                if (m_currentRotateSpeed > -m_maxRotateSpeed)
                {
                    m_currentRotateSpeed -= m_maxRotateSpeed / (m_lerpTime / Time.fixedDeltaTime);
                }
                else
                {
                    RunChangeSpeedTimer();
                }
            }

            m_upperCube.transform.Rotate(new Vector3(0f, m_currentRotateSpeed, 0f));
            m_downCube.transform.Rotate(new Vector3(0f, -m_currentRotateSpeed, 0f));
        }

        private void RunChangeSpeedTimer()
        {
            if (m_changeTimer > 0)
            {
                m_changeTimer -= Time.fixedDeltaTime;
                if (m_changeTimer < 0)
                {
                    m_goPositive = !m_goPositive;
                    m_changeTimer = m_changeTime;
                }
            }
        }

        protected override void OnGetHit(EventManager.HitInfo hitInfo)
        {
            if (hitInfo.HitCollider == m_collider)
            {
                GetCharacterStatus().AddHP(-hitInfo.Damage);
                if (GetCharacterStatus().HP <= 0)
                {
                    if (this == null || Engine.ActorManager == null)
                    {
                        return;
                    }

                    ActorManager.DestroyActor(this);
                }
            }
        }
    }
}
