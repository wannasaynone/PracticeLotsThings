using UnityEngine;

public class Actor : View {

    public int ID { get { return m_id; } }

    [SerializeField] protected int m_id = -1;
    [Header("Properties")]
    [SerializeField] protected float m_speed = 0.1f;
    [Header("Sub Components")]
    [SerializeField] protected Collider m_collider = null;
    [Header("Animator Setting")]
    [SerializeField] private Animator m_animator = null;
    [SerializeField] protected ActorAniamtorController m_actorAniamtorController = null;

    protected Vector3 m_movement = default(Vector3);
    protected Vector3 m_goal = default(Vector3);
    protected bool m_isForceMoving = false;
    protected bool m_isAI = false;

    protected virtual void Start()
    {
        m_actorAniamtorController = new ActorAniamtorController(m_animator);
        m_isAI = GetComponent<AIController>() != null;

        if (!m_isAI)
        {
            CameraController.MainCameraController.Track(gameObject);
        }

        Gun.OnHit += OnGetHit;
    }

    private void FixedUpdate()
    {
        if (m_isForceMoving)
        {
            if (Vector3.Distance(transform.position, m_goal) > 0.5f && m_isForceMoving)
            {
                Vector3 _dir = new Vector3((m_goal.x - transform.position.x), 0f, (m_goal.z - transform.position.z));
                transform.position += _dir.normalized * m_speed;

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
        else
        {
            transform.position += m_movement.normalized * m_speed * Time.fixedDeltaTime;
        }
    }

    public void ForceMoveTo(Vector3 position)
    {
        m_goal = position;
        m_isForceMoving = true;
    }

    public void SetMotion(float horizontal, float virtical, float motionCurve)
    {
        if(!m_isForceMoving)
        {
            m_actorAniamtorController.SetMovementAniamtion(horizontal, virtical, motionCurve);
            m_movement.Set(horizontal, 0f, virtical);
        }
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
