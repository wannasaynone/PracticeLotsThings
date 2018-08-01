using UnityEngine;

public class Actor : View {

    public float HorizontalMotion { get; protected set; }
    public float VerticalMotion { get; protected set; }
    public float MotionCurve { get; protected set; }
    public float MoveSpeed { get { return m_speed; } }
    public int ID { get { return m_id; } }

    [SerializeField] protected int m_id = -1;
    [Header("Properties")]
    [SerializeField] protected float m_speed = 0.1f;
    [Header("Sub Components")]
    [SerializeField] protected InputDetecter m_inputDetecter = null;
    [SerializeField] protected Collider m_collider = null;
    [Header("Animator Setting")]
    [SerializeField] private Animator m_animator = null;
    [SerializeField] protected ActorAniamtorController m_actorAniamtorController = null;

    protected Vector3 m_mousePositionOnStage = default(Vector3);
    protected Vector3 m_movement = default(Vector3);
    protected bool m_isAI = false;

    protected virtual void Start()
    {
        m_actorAniamtorController = new ActorAniamtorController(m_animator);

        if(m_inputDetecter == null)
        {
            m_inputDetecter = ScriptableObject.CreateInstance<InputDetecter_AI>();
        }

        m_isAI = GetComponent<AIController>() != null;

        if (!m_isAI)
        {
            CameraController.MainCameraController.Track(gameObject);
        }

        Gun.OnHit += OnGetHit;
    }

    private void Update()
    {
        if (m_isAI)
        {
            return;
        }
        ParseMotion();
    }

    private void FixedUpdate()
    {
        transform.position += m_movement.normalized * m_speed * Time.fixedDeltaTime;
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

    protected virtual void ParseMotion()
    {
        m_inputDetecter.Update();

        HorizontalMotion = transform.forward.x > 0 ? m_inputDetecter.Horizontal : -m_inputDetecter.Horizontal;
        VerticalMotion = transform.forward.z > 0 ? m_inputDetecter.Vertical : -m_inputDetecter.Vertical;
        MotionCurve = HorizontalMotion != 0 || VerticalMotion != 0 ? 1f : 0f;

        m_actorAniamtorController.SetMovementAniamtion(HorizontalMotion, VerticalMotion, MotionCurve);
        m_movement.Set(m_inputDetecter.Horizontal, 0f, m_inputDetecter.Vertical);

        ParseMousePositionToStage();
    }

    public void FaceTo(Vector3 targetPosition)
    {
        targetPosition.y = 0;
        transform.LookAt(targetPosition);
    }

    protected virtual void OnGetHit(Gun.HitInfo hitInfo)
    {
        if(hitInfo.HitCollider == m_collider)
        {
            ActorManager.GetCharacterStatus(this).HP -= 10; // TESTING
            Debug.Log(name + " get hit, remaining hp=" + ActorManager.GetCharacterStatus(this).HP);
            if (ActorManager.GetCharacterStatus(this).HP <= 0) // TESTING
            {
                m_collider.enabled = false; // TESTING
                CameraController.MainCameraController.StopTrack();
                Debug.Log(name + " died");
            }
        }
    }

}
