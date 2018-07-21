using System;
using UnityEngine;
using DG.Tweening;

public class Actor : View {

    public float HorizontalMotion { get; private set; }
    public float VerticalMotion { get; private set; }
    public float MotionCurve { get; private set; }

    [SerializeField] protected Camera m_camera = null;
    [SerializeField] protected float m_speed = 0.1f;
    [SerializeField] protected InputDetecter m_inputDetecter = null;
    [SerializeField] protected Animator m_animator = null;

    private ActorAniamtorController m_actorAniamtorController = null;
    private Vector3 m_mousePositionOnStage = default(Vector3);
    private Vector3 m_cameraTargetPosition = default(Vector3);
    private Vector3 m_movement = default(Vector3);

    private void Start()
    {
        m_cameraTargetPosition = m_camera.transform.position;
        m_actorAniamtorController = new ActorAniamtorController(this, m_animator);
    }

    private void Update()
    {
        m_inputDetecter.Update();
        m_actorAniamtorController.Update();
        m_movement.Set(m_inputDetecter.Horizontal, 0f, m_inputDetecter.Vertical);
        ParseMotion();
    }

    private void FixedUpdate()
    {
        transform.position += m_movement.normalized * m_speed * Time.fixedDeltaTime;

        ControlCamera();
        ParseMousePositionToStage();
        FaceTo(m_mousePositionOnStage);
    }

    private void ControlCamera()
    {
        m_cameraTargetPosition += new Vector3(m_movement.normalized.x, 0f, m_movement.normalized.z) * m_speed * Time.fixedDeltaTime;
        m_camera.transform.DOMove(m_cameraTargetPosition, 0.5f);
    }

    private void ParseMousePositionToStage()
    {
        Ray _camRay = m_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;

        if (Physics.Raycast(_camRay, out _hit))
        {
            m_mousePositionOnStage = _hit.point;
            m_mousePositionOnStage.y = 0f;
            Debug.DrawLine(m_mousePositionOnStage, transform.position, Color.red);
        }
    }

    private void ParseMotion()
    {
        HorizontalMotion = transform.forward.x > 0 ? m_inputDetecter.Horizontal : -m_inputDetecter.Horizontal;
        VerticalMotion = transform.forward.z > 0 ? m_inputDetecter.Vertical : -m_inputDetecter.Vertical;
        MotionCurve = HorizontalMotion != 0 || VerticalMotion != 0 ? 1f : 0f;
    }

    protected void FaceTo(Vector3 targerPosition)
    {
        transform.LookAt(targerPosition);
    }

}
