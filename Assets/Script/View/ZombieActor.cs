using UnityEngine;
using PracticeLotsThings.Manager;
using PracticeLotsThings.MainGameMonoBehaviour;

namespace PracticeLotsThings.View.Actor
{
    public class ZombieActor : Actor
    {
        public float AttackCdTime { get { return m_attackCd; } }

        [SerializeField] protected float m_attackCd = 1f;
        [SerializeField] protected float m_attackAnimationTime = 2.2f;
        [SerializeField] protected float m_lerpBackTime = 0.5f;
        [SerializeField] protected float m_startAttackDashTime = 0.2f;
        [SerializeField] protected float m_attackDashSpeed = 5f;
        [SerializeField] protected float m_attackDashTime = 0.1f;
        [SerializeField] protected float m_backToLifeAnimationTime = 2f;
        [SerializeField] protected Punch m_punch = null;
        [SerializeField] protected Light m_light = null;

        protected bool m_isEndingAttack = false;
        protected bool m_isDashing = false;

        protected float m_attackCdTimer = -1f;

        protected override void Update()
        {
            base.Update();
            if (m_isEndingAttack)
            {
                float _currentWeight = m_actorAniamtorController.LerpAttackingAnimation(false, m_lerpBackTime, false);

                if (_currentWeight < 0.1f)
                {
                    m_isEndingAttack = false;
                    UnlockInput();
                }
            }

            if (m_attackCdTimer > 0)
            {
                m_attackCdTimer -= Time.deltaTime;
            }
        }

        protected override void FixedUpdate()
        {
            if (IsLockingInput())
            {
                if (m_isDashing)
                {
                    Dash();
                }
                else
                {
                    m_direction = Vector3.zero;
                    m_actorAniamtorController.SetMovementAniamtion(0, 0, 0);
                }
            }
            else
            {
                base.FixedUpdate();
            }
        }

        public override void SetMotion(float horizontal, float vertical, float motionCurve)
        {
            if (m_isAttacking || m_isDashing || m_isEndingAttack || IsLockingInput())
            {
                return;
            }
            base.SetMotion(horizontal, vertical, motionCurve);
        }

        public override void FaceTo(Vector3 targetPosition)
        {
            if (m_isAttacking || m_isDashing || m_isEndingAttack || IsLockingInput())
            {
                return;
            }
            base.FaceTo(targetPosition);
        }

        private void Dash()
        {
            if (!IsLockingInput())
            {
                Debug.LogWarning("Need to lock input when set dash");
                return;
            }
            m_direction = transform.forward;
            m_speed = m_attackDashSpeed;
            Move();
        }

        private void StopDash()
        {
            if (this == null)
            {
                return;
            }
            m_speed = m_orgainSpeed;
            m_isDashing = false;
            m_punch.gameObject.SetActive(false);
            m_direction = Vector3.zero;
        }

        public void StartAttack()
        {
            if (m_isAttacking)
            {
                return;
            }
            Attack();
            if (!NetworkManager.IsOffline)
            {
                PhotonEventSender.Attack(this);
            }
        }

        public void SyncAttack()
        {
            if (ActorManager.IsMyActor(this))
            {
                return;
            }
            m_speed = m_attackDashSpeed;
            Attack();
        }

        private void Attack()
        {
            if (m_attackCdTimer > 0f)
            {
                return;
            }

            LockInput();
            m_isEndingAttack = false;
            m_isAttacking = true;

            m_actorAniamtorController.ForceRestartAttackAnimation();
            m_actorAniamtorController.LerpAttackingAnimation(true, 1f, false);

            m_attackCdTimer = m_attackCd + m_attackAnimationTime;
            TimerManager.Schedule(m_startAttackDashTime, delegate
            {
                if (this == null)
                {
                    return;
                }
                m_punch.gameObject.SetActive(true);
                m_isDashing = true;
                TimerManager.Schedule(m_attackDashTime, StopDash);
            });
            TimerManager.Schedule(m_attackAnimationTime, delegate
            {
                m_isEndingAttack = true;
                m_isAttacking = false;
            });
        }

        public void SetIsTransformedFromOthers()
        {
            m_attackCdTimer = m_backToLifeAnimationTime;
            LockInput();
            m_actorController.enabled = false;
            m_aiController.enabled = false;
            m_isAI = false;
            m_actorAniamtorController.SetBackToLife(m_backToLifeAnimationTime);
            TimerManager.Schedule(m_backToLifeAnimationTime,
                delegate
                {
                    if (ActorManager.IsMyActor(this))
                    {
                        EnableAI(true);
                    }
                    UnlockInput();
                });
        }

        public void SyncIsTransformedFromOthers()
        {
            if (ActorManager.IsMyActor(this))
            {
                return;
            }
            SetIsTransformedFromOthers();
        }

        protected override void OnGetHit(EventManager.HitInfo hitInfo)
        {
            if (hitInfo.actorType == ActorFilter.ActorType.Zombie || hitInfo.HitCollider != m_collider)
            {
                return;
            }

            base.OnGetHit(hitInfo);
            if (GetCharacterStatus().HP <= 0)
            {
                EnableLight(false);
                ReplacePlayerWithEmpty();
            }
        }

        public override void EnableAI(bool enable)
        {
            base.EnableAI(enable);
            EnableLight(!enable);
        }

        public void EnableLight(bool enable)
        {
            if (m_light != null && this != null)
            {
                m_light.gameObject.SetActive(enable);
            }
        }
    }
}
