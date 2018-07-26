using System;
using UnityEngine;
using DG.Tweening;

public class Actor : View {

    public float HorizontalMotion { get; private set; }
    public float VerticalMotion { get; private set; }
    public float MotionCurve { get; private set; }
    public bool IsShooting { get; private set; }

    [Header("Properties")]
    [SerializeField] protected float m_speed = 0.1f;
    [Header("Sub Components")]
    [SerializeField] protected InputDetecter m_inputDetecter = null;
    [SerializeField] protected Gun m_gun = null;
    [SerializeField] protected Collider m_collider = null;
    [Header("Animator Setting")]
    [SerializeField] protected Animator m_animator = null;
    [SerializeField] protected ActorAniamtorController m_actorAniamtorController = null;

    private float m_shootCdTimer = -1f;

    private Vector3 m_mousePositionOnStage = default(Vector3);
    private Vector3 m_movement = default(Vector3);

    protected virtual void Start()
    {
        m_actorAniamtorController = new ActorAniamtorController(this, m_animator);
        Gun.OnHit += OnGetHit;
    }

    protected virtual void Update()
    {
        m_inputDetecter.Update();
        m_actorAniamtorController.Update();
        m_movement.Set(m_inputDetecter.Horizontal, 0f, m_inputDetecter.Vertical);
        ParseMotion();
    }

    private void FixedUpdate()
    {
        transform.position += m_movement.normalized * m_speed * Time.fixedDeltaTime;

        CameraController.MainCameraController.AddPosition(new Vector3(m_movement.normalized.x, 0f, m_movement.normalized.z) * m_speed * Time.fixedDeltaTime);
        ParseMousePositionToStage();
        FaceTo(m_mousePositionOnStage);
    }

    private void ParseMousePositionToStage()
    {
        Ray _camRay = CameraController.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;

        if (Physics.Raycast(_camRay, out _hit, 100f, LayerMask.GetMask("Ground")))
        {
            m_mousePositionOnStage = _hit.point;
            m_mousePositionOnStage.y = 0f;
        }

        Debug.DrawLine(transform.position, m_mousePositionOnStage);
    }

    private void ParseMotion()
    {
        HorizontalMotion = transform.forward.x > 0 ? m_inputDetecter.Horizontal : -m_inputDetecter.Horizontal;
        VerticalMotion = transform.forward.z > 0 ? m_inputDetecter.Vertical : -m_inputDetecter.Vertical;
        MotionCurve = HorizontalMotion != 0 || VerticalMotion != 0 ? 1f : 0f;
        IsShooting = m_inputDetecter.IsShooting;

        if(m_inputDetecter.StartShoot)
        {
            m_shootCdTimer = m_gun.FireCdTime;
        }

        if(m_inputDetecter.IsShooting)
        {
            m_shootCdTimer -= Time.deltaTime;
            if (m_shootCdTimer <= 0)
            {
                m_shootCdTimer = m_gun.FireCdTime;
                m_gun.Fire();
            }
        }
    }

    protected void FaceTo(Vector3 targerPosition)
    {
        transform.LookAt(targerPosition);
    }

    private void OnGetHit(Gun.HitInfo hitInfo)
    {
        if(hitInfo.HitCollider == m_collider)
        {
            Debug.Log("Get Hit");
        }
    }

}
