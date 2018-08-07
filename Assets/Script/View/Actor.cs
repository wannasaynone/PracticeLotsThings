using UnityEngine;

public class Actor : View {

    public int StatusID { get { return m_characterStatusId; } }
    public bool IsAttacking { get { return m_isAttacking; } }
    public bool IsAI { get { return m_isAI; } }

    public event System.Action<Actor> OnActorDied;

    [SerializeField] protected int m_characterStatusId = -1;
    [Header("Properties")]
    [SerializeField] protected float m_speed = 4f;
    [Header("Sub Components")]
    [SerializeField] protected Rigidbody m_rigidBody = null;
    [SerializeField] protected Collider m_collider = null;
    [SerializeField] protected ActorController m_actorController = null;
    [SerializeField] protected AIController m_aiController = null;
    [Header("Animator Setting")]
    [SerializeField] private Animator m_animator = null;
    [SerializeField] protected ActorAniamtorController m_actorAniamtorController = null;

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
        EventManager.OnHit += OnGetHit;
    }

    protected virtual void Update()
    {
        if(transform.position.y < -1f)
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

    protected void Move()
    {
        transform.position += m_movement.normalized * m_speed * Time.fixedDeltaTime;
    }

    protected void MoveToGoal()
    {
        if (Vector3.Distance(transform.position, m_goal) > 0.5f)
        {
            m_movement.Set((m_goal.x - transform.position.x), 0f, (m_goal.z - transform.position.z));

            Move();

            if (Vector3.Distance(transform.position, m_goal) < 0.5f)
            {
                m_isForceMoving = false;
                m_actorAniamtorController.SetMovementAniamtion(0f, 0f, 0f);
            }
            else
            {
                m_actorAniamtorController.SetMovementAniamtion(1f, 1f, 1f);
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

    public virtual void FaceTo(Vector3 targetPosition)
    {
        targetPosition.y = 0;
        transform.LookAt(targetPosition);
    }

    public void EnableAI(bool enable)
    {
        if(m_aiController == null || m_actorController == null)
        {
            return;
        }

        m_aiController.enabled = enable;
        m_actorController.enabled = !enable;
        m_isAI = enable;
    }

    protected virtual void OnGetHit(EventManager.HitInfo hitInfo)
    {
        if (hitInfo.HitCollider == m_collider)
        {
            if (Engine.ActorManager != null)
            {
                if (Engine.ActorManager.GetCharacterStatus(this).HP < 0)
                {
                    return;
                }

                Engine.ActorManager.GetCharacterStatus(this).AddHP(-hitInfo.Damage);

                if (Engine.ActorManager.GetCharacterStatus(this).HP <= 0) // TESTING
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

                    m_actorAniamtorController.SetDie();
                }
            }
        }
    }

}
