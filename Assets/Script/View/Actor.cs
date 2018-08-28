using UnityEngine;

public class Actor : View {

    public TextMesh TestTextMesh = null;

    public const float GOAL_DETECT_RANGE = 0.5f;

    public int StatusID { get { return m_characterStatusId; } }
    public bool IsAttacking { get { return m_isAttacking; } }
    public bool isSyncAttacking = false;
    public bool IsAI { get { return m_isAI; } }
    public PhotonView PhotonView { get { return m_photonView; } }

    public event System.Action<Actor> OnActorDied;
    public event System.Action<Actor> OnActorDestroyed;

    [SerializeField] protected int m_characterStatusId = -1;
    [Header("Properties")]
    [SerializeField] protected float m_speed = 4f;
    [Header("Sub Components")]
    [SerializeField] protected Rigidbody m_rigidBody = null;
    [SerializeField] protected Collider m_collider = null;
    [SerializeField] protected ActorController m_actorController = null;
    [SerializeField] protected AIController m_aiController = null;
    [SerializeField] protected PhotonActorController m_photonActorController = null;
    [SerializeField] protected PhotonView m_photonView = null;
    [Header("Animator Setting")]
    [SerializeField] private Animator m_animator = null;
    [SerializeField] protected ActorAniamtorController m_actorAniamtorController = null;

    protected CharacterStatus m_status = null;
    protected Vector3 m_movement = default(Vector3);
    protected Vector3 m_goal = default(Vector3);
    protected bool m_isForceMoving = false;
    protected bool m_lockMovement = false;
    protected bool m_isAI = false;

    protected bool m_isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        m_actorAniamtorController = new ActorAniamtorController(m_animator);
        SetStatus(m_characterStatusId);
        EventManager.OnHit += OnGetHit;
    }

    protected virtual void Update()
    {
        TestTextMesh.text = test.ToString();
        if (transform.position.y < -1f)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (m_lockMovement)
        {
            m_movement = Vector3.zero;
        }
        else
        {
            if (m_isForceMoving)
            {
                MoveToGoal();
            }
            else
            {
                Move();
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(OnActorDestroyed != null)
        {
            OnActorDestroyed(this);
        }
    }

    private void SetStatus(int id)
    {
        CharacterStatus _status = GameDataManager.GetGameData<CharacterStatus>(id);
        if (_status != null)
        {
            m_status = new CharacterStatus(_status);
        }
        else
        {
            m_status = new CharacterStatus(CharacterStatus.Default);
        }
    }

    public CharacterStatus GetCharacterStatus()
    {
        if(m_status == null)
        {
            Debug.LogWarning("Not Set Chracter Status In " + gameObject.name + " Yet");
        }
        return m_status;
    }

    protected void Move()
    {
        transform.position += m_movement.normalized * m_speed * Time.fixedDeltaTime;
    }

    protected void MoveToGoal()
    {
        if (Vector3.Distance(transform.position, m_goal) > GOAL_DETECT_RANGE)
        {
            m_movement.Set((m_goal.x - transform.position.x), 0f, (m_goal.z - transform.position.z));

            Move();

            if (Vector3.Distance(transform.position, m_goal) < GOAL_DETECT_RANGE)
            {
                m_isForceMoving = false;
                if(m_actorAniamtorController != null)
                {
                    m_actorAniamtorController.SetMovementAniamtion(0f, 0f, 0f);
                }
            }
            else
            {
                if (m_actorAniamtorController != null)
                {
                    m_actorAniamtorController.SetMovementAniamtion(1f, 1f, 1f);
                }
            }
        }
    }

    public virtual void ForceIdle()
    {
        m_goal = transform.position;
        m_movement = Vector3.zero;
        SetMotion(0, 0, 0);
        m_actorAniamtorController.SetMovementAniamtion(0, 0, 0);
        m_isAttacking = false;
        m_isForceMoving = false;
    }

    public virtual void SetMoveTo(Vector3 position)
    {
        m_goal = position;
        m_isForceMoving = true;
    }

    public virtual void SetMotion(float horizontal, float vertical, float motionCurve)
    {
        if(!m_isForceMoving)
        {
            m_actorAniamtorController.SetMovementAniamtion(horizontal, vertical, motionCurve);
            m_movement.Set(horizontal, 0f, vertical);
        }
    }

    public virtual void SetMotion(Vector3 direction, float motionCurve)
    {
        if (!m_isForceMoving)
        {
            if(m_actorAniamtorController != null)
            {
                m_actorAniamtorController.SetMovementAniamtion(direction.x, direction.z, motionCurve);
            }
            m_movement.Set(direction.x, direction.y, direction.z);
        }
    }

    public virtual void FaceTo(Vector3 targetPosition)
    {
        targetPosition.y = 0;
        transform.LookAt(targetPosition);
    }

    public virtual void EnableAI(bool enable)
    {
        if(m_aiController == null || m_actorController == null || GetCharacterStatus().HP <= 0)
        {
            return;
        }

        m_aiController.enabled = enable;
        m_actorController.enabled = !enable;
        m_isAI = enable;
    }

    int test = 0;
    protected virtual void OnGetHit(EventManager.HitInfo hitInfo)
    {
        if (hitInfo.HitCollider == m_collider)
        {
            test++;
            if (Engine.ActorManager != null)
            {
                if (GetCharacterStatus().HP < 0)
                {
                    return;
                }

                GetCharacterStatus().AddHP(-hitInfo.Damage);

                if (GetCharacterStatus().HP <= 0) // TESTING
                {
                    ForceIdle();

                    m_aiController.enabled = false;
                    m_actorController.enabled = false;
                    m_collider.enabled = false;
                    m_rigidBody.useGravity = false;
                    m_lockMovement = true;

                    // TODO: real game over flow
                    if (CameraController.MainCameraController.TrackingGameObjectInstanceID == gameObject.GetInstanceID())
                    {
                        CameraController.MainCameraController.StopTrack();
                    }

                    if(OnActorDied != null)
                    {
                        OnActorDied(this);
                    }

                    if (m_actorAniamtorController != null)
                    {
                        m_actorAniamtorController.SetDie();
                    }
                }
            }
        }
    }

}
