using UnityEngine;
using UnityEngine.UI;

public class ShooterActor : NormalActor {

    public float AttackCdTime { get { return m_gun.FireCdTime; } }

    [SerializeField] protected Gun m_gun = null;
    [SerializeField] private GameObject m_tower = null;
    [SerializeField] private Slider m_towerProgressSlider = null;
    [SerializeField] private float m_buildingTowerTime = 1f;
    [SerializeField] private int m_towerCost = 30;

    protected float m_attackCdTimer = -1f;
    private bool m_buildingTower = false;

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

        if(m_buildingTower)
        {
            m_towerProgressSlider.gameObject.SetActive(true);
            m_towerProgressSlider.value += m_towerProgressSlider.maxValue / (m_buildingTowerTime / Time.deltaTime);
            if (m_towerProgressSlider.value >= m_towerProgressSlider.maxValue)
            {
                Engine.ActorManager.CreateActor(GameManager.GameSetting.TowerActorPrefabID, null, InputDetecter.MousePositionOnStage);
                GetCharacterStatus().AddMat(-m_towerCost);
                m_buildingTower = false;
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
                m_buildingTower = true;
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
            m_buildingTower = false;
        }
        else
        {
            m_interactingObject.StopInteract();
        }
    }

}
