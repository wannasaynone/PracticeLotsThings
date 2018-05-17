using System;
using UnityEngine;

public class ActorController : MonoBehaviour {

    private const string LAYER_MASK_NAME_GROUND = "Ground";
    private const string ANIMATOR_PARA_NAME_FORWARD = "forward";
    private const string ANIMATOR_PARA_NAME_JUMP = "jump";
    private const string ANIMATOR_PARA_NAME_ATTACK = "attack";
    private const string ANIMATOR_PARA_NAME_IS_GROUND = "isGround";
    private const string ANIMATOR_PARA_NAME_ROLL = "roll";
    private const string ANIMATOR_PARA_NAME_ATTACK_CURVE = "attackVelocityCurve";

	private const string ANIMATOR_STATE_NAME_GROUND = "ground";

    private const string ANIMATOR_LAYER_NAME_0 = "Base Layer";
    private const string ANIMATOR_LAYER_NAME_1 = "attack";

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
    [SerializeField] private float m_attackTransSpeed = 10f;
    private float m_attackLayerWeight = 0f;

    private Vector3 m_thrustVector3 = Vector3.zero;
    private Vector3 m_movingVector = Vector3.zero;
    private Transform m_model = null;

	private bool m_lockInput = false;
    private bool m_lockUpdateInputVelocity = false;

    private void Awake()
    {
        m_model = m_modelAnimator.transform;

        AnimatorEventSender.RegistOnStateEntered("jump", OnJumpEnter);
        AnimatorEventSender.RegistOnStateEntered("ground", OnGroundEnter);
        AnimatorEventSender.RegistOnStateEntered("roll", OnRollEnter);
        AnimatorEventSender.RegistOnStateEntered("jab", OnJabEnter);
        AnimatorEventSender.RegistOnStateEntered("attack", OnAttackEnter);
        AnimatorEventSender.RegistOnStateEntered("idle", OnIdleEnter);

		AnimatorEventSender.RegistOnStateExited("jump", OnJumpExit);
    }

    private void Update ()
    {
		DetectFalling();
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
		if(!m_lockUpdateInputVelocity)
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
		else
		if(m_input.Attacking && !m_lockInput)
        {
            m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_ATTACK);
        }

        m_movingVector = m_input.Direction_MotionCurveValue * m_model.forward * m_moveSpeed * (m_input.Running ? m_runScale : 1f);
    }

    private void DetectFalling()
	{
		if (m_rigidbody.velocity.magnitude > 5f)
        {
            m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_ROLL);
        }
	}

	private bool IsAnimatorInState(string stateName, string layerName = ANIMATOR_LAYER_NAME_0)
	{
		return m_modelAnimator.GetCurrentAnimatorStateInfo(m_modelAnimator.GetLayerIndex(layerName)).IsName(stateName);
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
        if((Mathf.Abs(m_input.Direction_Horizontal) <= 0.1 && Mathf.Abs(m_input.Direction_Vertical) <= 0.1) || m_lockUpdateInputVelocity)
        {
            return;
        }
        else
        {
            // 水平向量 * 輸入變量 + 垂直向量 * 輸入變量 = 斜向向量 -> EX 0度向量 + 90度向量 = 45度向量 = 模型對面方向
            m_model.forward = Vector3.Slerp(m_model.forward, transform.right * m_input.Direction_Horizontal + transform.forward * m_input.Direction_Vertical, m_rotateSpeed);
        }
    }

    private void OnIsOnGround()
    {
        m_modelAnimator.SetBool(ANIMATOR_PARA_NAME_IS_GROUND, true);
    }

    private void OnIsOutGround()
    {
        m_lockUpdateInputVelocity = true;
        m_modelAnimator.SetBool(ANIMATOR_PARA_NAME_IS_GROUND, false);
    }

    private void OnJumpEnter(AnimatorEventArgs e)
    {
		m_lockInput = true;
        m_thrustVector3 += new Vector3(0f, m_jumpThrust, 0f);
    }

	private void OnJumpExit(AnimatorEventArgs e)
	{
		m_lockInput = false;
	}

    private void OnGroundEnter(AnimatorEventArgs e)
    {
        m_lockUpdateInputVelocity = false;
    }

    private void OnRollEnter(AnimatorEventArgs e)
    {
        m_lockUpdateInputVelocity = true;
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
        m_lockUpdateInputVelocity = true;
        m_rigidbody.velocity += m_model.forward * -m_jadThrust;
        m_rigidbody.velocity += new Vector3(0f, m_jadThrust, 0f);
    }

    private void OnAttackEnter(AnimatorEventArgs e)
    {
        m_lockUpdateInputVelocity = true;
        m_modelAnimator.SetLayerWeight(m_modelAnimator.GetLayerIndex(ANIMATOR_LAYER_NAME_1), 1f);
    }

    private void OnIdleEnter(AnimatorEventArgs e)
    {
        m_lockUpdateInputVelocity = false;
        m_modelAnimator.SetLayerWeight(m_modelAnimator.GetLayerIndex(ANIMATOR_LAYER_NAME_1), 0f);
    }
}
