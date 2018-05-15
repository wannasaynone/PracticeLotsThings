using System;
using UnityEngine;

public class ActorController : MonoBehaviour {

    private const string LAYER_MASK_NAME_GROUND = "Ground";
    private const string ANIMATOR_PARA_NAME_FORWARD = "forward";
    private const string ANIMATOR_PARA_NAME_JUMP = "jump";
    private const string ANIMATOR_PARA_NAME_ATTACK = "attack";
    private const string ANIMATOR_PARA_NAME_IS_GROUND = "isGround";
    private const string ANIMATOR_PARA_NAME_ROLL = "roll";

    public GameObject Model { get { return m_model.gameObject; } }

    [SerializeField] private InputModule m_input = null;
    [SerializeField] private Animator m_modelAnimator = null;
    [SerializeField] private Rigidbody m_rigidbody = null;
    [SerializeField] private CapsuleCollider m_capcaol = null;
    [SerializeField] private Vector3 m_adjustCollision = Vector3.zero;
    [SerializeField] private float m_rotateSpeed = 10f;
    [SerializeField] private float m_moveSpeed = 1.5f;
    [SerializeField] private float m_runScale = 2.5f;
    [SerializeField] private float m_jumpThrust = 4f;
    [SerializeField] private float m_rollThrust = 4f;
    [SerializeField] private float m_jadThrust = 3f;

    private Vector3 m_thrustVector3 = Vector3.zero;
    private Vector3 m_movingVector = Vector3.zero;
    private Transform m_model = null;

    private bool m_lockUpdateInput = false;

    private void Awake()
    {
        m_model = m_modelAnimator.transform;

        AnimatorEventSender.RegistOnStateEntered("jump", OnJumpEnter);
        AnimatorEventSender.RegistOnStateEntered("ground", OnGroundEnter);
        AnimatorEventSender.RegistOnStateEntered("roll", OnRollEnter);
        AnimatorEventSender.RegistOnStateEntered("jab", OnJabEnter);
    }

    private void Update ()
    {
        if(m_rigidbody.velocity.magnitude > 5f)
        {
            m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_ROLL);
        }
        SetMoveMotion();
        SetDectectCollision();
    }

    private void FixedUpdate()
    {
        SetVelocity();
    }

    /////////////////////////////////

    private void SetVelocity()
    {
        if(!m_lockUpdateInput)
        {
            m_rigidbody.velocity = new Vector3(m_movingVector.x, m_rigidbody.velocity.y, m_movingVector.z) + m_thrustVector3;
        }

        m_thrustVector3 = Vector3.zero;
    }

    private void SetMoveMotion()
    {
        RotateModel();
        m_modelAnimator.SetFloat(ANIMATOR_PARA_NAME_FORWARD, m_input.Direction_MotionCurveValue);
        if(m_input.Jumping)
        {
            m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_JUMP);
        }

        if(m_input.Attacking)
        {
            m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_ATTACK);
        }

        m_movingVector = m_input.Direction_MotionCurveValue * m_model.forward * m_moveSpeed * (m_input.Running ? m_runScale : 1f);
    }

    private void SetDectectCollision()
    {
        if (CollisionDetector.DectectCollision(m_capcaol, LAYER_MASK_NAME_GROUND, m_adjustCollision))
        {
            OnIsOnGround();
        }
        else
        {
            OnIsOutGround();
        }
    }

    private void RotateModel()
    {
        // 避免在水平值和垂直值過低時重設模型為"正面"
        if((Mathf.Abs(m_input.Direction_Horizontal) <= 0.1 && Mathf.Abs(m_input.Direction_Vertical) <= 0.1) || m_lockUpdateInput)
        {
            return;
        }
        else
        {
            // 水平向量 * 輸入變量 + 垂直向量 * 輸入變量 = 斜向向量 -> EX 0度向量 + 90度向量 = 45度向量 aka 模型對面方向
            m_model.forward = Vector3.Slerp(m_model.forward, transform.right * m_input.Direction_Horizontal + transform.forward * m_input.Direction_Vertical, m_rotateSpeed);
        }
    }

    private void OnIsOnGround()
    {
        m_modelAnimator.SetBool(ANIMATOR_PARA_NAME_IS_GROUND, true);
    }

    private void OnIsOutGround()
    {
        m_lockUpdateInput = true;
        m_modelAnimator.SetBool(ANIMATOR_PARA_NAME_IS_GROUND, false);
    }

    private void OnJumpEnter(AnimatorEventArgs e)
    {
        m_thrustVector3 += new Vector3(0f, m_jumpThrust, 0f);
    }

    private void OnGroundEnter(AnimatorEventArgs e)
    {
        m_lockUpdateInput = false;
    }

    private void OnRollEnter(AnimatorEventArgs e)
    {
        m_lockUpdateInput = true;
        if (m_rigidbody.velocity.magnitude > 5f)
        {
            m_rigidbody.velocity += m_model.forward * m_rollThrust * 1.5f;
        }
        else
        {
            m_rigidbody.velocity += m_model.forward * m_rollThrust;
        }
    }

    private void OnJabEnter(AnimatorEventArgs e)
    {
        m_lockUpdateInput = true;
        m_rigidbody.velocity += m_model.forward * -m_jadThrust;
        m_rigidbody.velocity += new Vector3(0f, m_jadThrust, 0f);
    }
}
