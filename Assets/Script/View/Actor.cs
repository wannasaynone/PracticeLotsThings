using UnityEngine;

public class Actor : View {

    public int StatusID { get { return m_characterStatusId; } }
    public bool IsAttacking { get { return m_isAttacking; } }
    public bool IsAI { get { return m_isAI; } }

    [SerializeField] protected int m_characterStatusId = -1;
    [Header("Properties")]
    [SerializeField] protected float m_speed = 0.1f;
    [SerializeField] protected bool m_isAI = false;
    [Header("Sub Components")]
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

    protected bool m_isAttacking = false;

    protected virtual void Start()
    {
        m_actorAniamtorController = new ActorAniamtorController(m_animator);
        EventManager.OnHit += OnGetHit;
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
        m_aiController.enabled = enable;
        m_actorController.enabled = !enable;
        m_isAI = enable;
    }

    protected virtual void OnGetHit(EventManager.HitInfo hitInfo)
    {
        if (hitInfo.HitCollider == m_collider && Engine.ActorManager != null)
        {
            Engine.ActorManager.GetCharacterStatus(this).HP -= hitInfo.Damage; // TESTING

            Debug.Log(name + " get hit, remaining hp=" + Engine.ActorManager.GetCharacterStatus(this).HP);

            if (Engine.ActorManager.GetCharacterStatus(this).HP <= 0) // TESTING
            {
                m_collider.enabled = false; // TESTING

                if (GameManager.Player == this)
                    CameraController.MainCameraController.StopTrack();

                TimerManager.Schedule(1f, delegate { Destroy(gameObject); });

                Debug.Log(name + " died");
            }
        }
    }

}
