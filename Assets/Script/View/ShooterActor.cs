using UnityEngine;
using UnityEngine.UI;

public class ShooterActor : NormalActor {

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

    protected Vector3 m_rollMovement = default(Vector3);

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

        if(m_isBuildingTower)
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
            if(m_towerProgressSlider.gameObject.activeSelf)
            {
                m_towerProgressSlider.gameObject.SetActive(false);
            }
        }

        m_actorAniamtorController.LerpAttackingAnimation(m_isAttacking || isSyncAttacking, 0.5f, true);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(m_isRolling)
        {
            transform.position += m_rollMovement.normalized * m_rollSpeed * Time.fixedDeltaTime;

            m_rollTimer -= Time.fixedDeltaTime;
            if(m_rollTimer <= 0)
            {
                StopRoll();
            }
        }
    }

    public void StartRoll(Vector3 direction)
    {
        m_isRolling = true;
        m_lockMovement = true;
        m_rollMovement = direction;
        LockInput();
        m_rollTimer = m_rollTime;

        if (CameraController.MainCameraActor != null)
        {
            m_rollMovement = direction.x * CameraController.MainCameraActor.transform.right + direction.z * CameraController.MainCameraActor.transform.forward;
        }
        else
        {
            m_rollMovement = direction;
        }
    }

    protected void StopRoll()
    {
        m_isRolling = false;
        m_lockMovement = false;
        m_rollMovement = Vector3.zero;
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
        if(!NetworkManager.IsOffline)
        {
            PhotonEventSender.Fire(this, _firePosition);
        }
    }

    public void SyncAttack(Vector3 position)
    {
        if(ActorManager.IsMyActor(this))
        {
            return;
        }
        m_gun.FireToSpecificPoint(position);
    }

    public override void StartInteracting()
    {
        if (m_interactingObject == null)
        {
            if(GetCharacterStatus().Mat >= m_towerCost)
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
