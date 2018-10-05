using UnityEngine;
using UnityEngine.UI;
using PracticeLotsThings.Manager;
using PracticeLotsThings.MainGameMonoBehaviour;
using PracticeLotsThings.Input;

namespace PracticeLotsThings.View.Actor
{
    public class ShooterActor : NormalActor
    {
        public float AttackCdTime { get { return m_gun.FireCdTime; } }

        [SerializeField] protected Gun m_gun = null;
        [SerializeField] protected Slider m_towerProgressSlider = null;
        [SerializeField] protected float m_buildingTowerTime = 1f;
        [SerializeField] protected int m_towerCost = 30;
        [SerializeField] protected float m_rollSpeed = 2f;
        [SerializeField] protected float m_rollTime = 0.1f;

        protected float m_attackCdTimer = -1f;
        protected float m_rollTimer = -1f;
        protected bool m_isBuildingTower = false;
        protected bool m_isRolling = false;

        protected override void Update()
        {
            base.Update();
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

            if (m_isBuildingTower)
            {
                m_towerProgressSlider.gameObject.SetActive(true);
                m_towerProgressSlider.value += m_towerProgressSlider.maxValue / (m_buildingTowerTime / Time.deltaTime);
                if (m_towerProgressSlider.value >= m_towerProgressSlider.maxValue)
                {
                    Engine.ActorManager.CreateActor(GameManager.GameSetting.TowerActorPrefabID, null, InputDetecter.MousePositionOnStage);
                    GetCharacterStatus().AddMat(-m_towerCost);
                    m_isBuildingTower = false;
                }
            }
            else
            {
                m_towerProgressSlider.value = 0f;
                if (m_towerProgressSlider.gameObject.activeSelf)
                {
                    m_towerProgressSlider.gameObject.SetActive(false);
                }
            }

            m_actorAniamtorController.LerpAttackingAnimation(m_isAttacking || isSyncAttacking, 0.5f, true);
        }

        protected override void FixedUpdate()
        {
            if (m_isRolling)
            {
                m_rollTimer -= Time.fixedDeltaTime;
                Move();
                if (m_rollTimer <= 0)
                {
                    StopRoll();
                }
            }
            else
            {
                base.FixedUpdate();
            }
        }

        public void StartRoll(Vector3 direction)
        {
            if (CameraController.MainCameraActor != null)
            {
                m_direction = direction.x * CameraController.MainCameraActor.transform.right + direction.z * CameraController.MainCameraActor.transform.forward;
            }
            else
            {
                m_direction = direction;
            }
            Roll(m_direction);
            if (!NetworkManager.IsOffline)
            {
                PhotonEventSender.Roll(this, m_direction);
            }
        }

        public void SyncRoll(Vector3 direction)
        {
            if (ActorManager.IsMyActor(this))
            {
                return;
            }
            Roll(direction);
        }

        protected void Roll(Vector3 direction)
        {
            LockInput();
            m_isRolling = true;
            m_direction = direction;
            m_speed = m_rollSpeed;
            m_rollTimer = m_rollTime;
        }

        protected void StopRoll()
        {
            m_isRolling = false;
            m_direction = Vector3.zero;
            m_speed = m_orgainSpeed;
            UnlockInput();
        }

        public void StartAttack()
        {
            m_isAttacking = true;
            isSyncAttacking = true;
            m_attackCdTimer = m_gun.FireCdTime;
        }

        public void StopAttack()
        {
            m_isAttacking = false;
            isSyncAttacking = false;
        }

        public void ForceAttack()
        {
            m_attackCdTimer = m_gun.FireCdTime;
            Vector3 _firePosition = m_gun.Fire();
            if (!NetworkManager.IsOffline)
            {
                PhotonEventSender.Fire(this, _firePosition);
            }
        }

        public void SyncAttack(Vector3 position)
        {
            if (ActorManager.IsMyActor(this))
            {
                return;
            }
            m_gun.FireToSpecificPoint(position);
        }

        public override void StartInteracting()
        {
            if (m_interactingObject == null)
            {
                if (GetCharacterStatus().Mat >= m_towerCost)
                {
                    m_isBuildingTower = true;
                }
            }
            else
            {
                m_interactingObject.SetActor(this);
                m_interactingObject.StartInteract();
            }
        }

        public override void StopInteracting()
        {
            if (m_interactingObject == null)
            {
                m_isBuildingTower = false;
            }
            else
            {
                m_interactingObject.StopInteract();
            }
        }
    }
}
