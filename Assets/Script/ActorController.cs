using System;
using UnityEngine;

public class ActorController : MonoBehaviour {

    private enum MoveState
    {
        None,
        Move,
        Run,
        Jump,
        Fall,
        Roll
    }

    private enum AttackState
    {
        None,
        Attack
    }

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
    public bool IsGrounded { get; private set; }

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

    private AttackState m_currentAttack = AttackState.None;
    private MoveState m_currentMove = MoveState.None;
    private bool m_run = false;

    private Vector3 m_thrustVector3 = Vector3.zero;
    private Vector3 m_movingVector = Vector3.zero;
    private Transform m_model = null;

    private bool m_lockUpdateInputVelocity = false;

    private void Awake()
    {
        m_model = m_modelAnimator.transform;

        AnimatorEventSender.RegistOnStateEntered("ground", OnGroundEnter);
        AnimatorEventSender.RegistOnStateEntered("jump", OnJumpEnter);
        AnimatorEventSender.RegistOnStateEntered("fall", OnFallEnter);
        AnimatorEventSender.RegistOnStateEntered("roll", OnRollEnter);
        AnimatorEventSender.RegistOnStateEntered("jab", OnJabEnter);

        AnimatorEventSender.RegistOnStateExited("ground", OnGroundExit);
        AnimatorEventSender.RegistOnStateExited("jump", OnJumpExit);
        AnimatorEventSender.RegistOnStateExited("fall", OnFallExit);
        AnimatorEventSender.RegistOnStateExited("roll", OnRollExit);
        AnimatorEventSender.RegistOnStateExited("jab", OnJabExit);

        AnimatorEventSender.RegistOnStateUpdated("ground", OnGroundUpdate);
        AnimatorEventSender.RegistOnStateUpdated("jump", OnJumpUpdate);
        AnimatorEventSender.RegistOnStateUpdated("fall", OnFallUpdate);
        AnimatorEventSender.RegistOnStateUpdated("roll", OnRollUpdate);
        AnimatorEventSender.RegistOnStateUpdated("jab", OnJabUpdate);
    }

    private void Update ()
    {
        ParseInputSignal();
        ParseMotionSingal();
        DectectCollision(); 
    }

    private void FixedUpdate()
    {

    }

    private void OnGroundEnter(AnimatorEventArgs e)
    {
        Debug.Log("OnGroundEnter");
    }

    private void OnJumpEnter(AnimatorEventArgs e)
    {
        Debug.Log("OnJumpEnter");
    }

    private void OnFallEnter(AnimatorEventArgs e)
    {
        Debug.Log("OnFallEnter");
    }

    private void OnRollEnter(AnimatorEventArgs e)
    {
        Debug.Log("OnRollEnter");
    }

    private void OnJabEnter(AnimatorEventArgs e)
    {
        Debug.Log("OnJabEnter");
    }

    private void OnGroundExit(AnimatorEventArgs e)
    {
        Debug.Log("OnGroundExit");
    }

    private void OnJumpExit(AnimatorEventArgs e)
    {
        Debug.Log("OnJumpExit");
    }

    private void OnFallExit(AnimatorEventArgs e)
    {
        Debug.Log("OnFallExit");
    }

    private void OnRollExit(AnimatorEventArgs e)
    {
        Debug.Log("OnRollExit");
    }

    private void OnJabExit(AnimatorEventArgs e)
    {
        Debug.Log("OnJabExit");
    }

    private void OnGroundUpdate(AnimatorEventArgs e)
    {
        Debug.Log("OnGroundUpdate");
    }

    private void OnJumpUpdate(AnimatorEventArgs e)
    {
        Debug.Log("OnJumpUpdate");
    }

    private void OnFallUpdate(AnimatorEventArgs e)
    {
        Debug.Log("OnFallUpdate");
    }

    private void OnRollUpdate(AnimatorEventArgs e)
    {
        Debug.Log("OnRollUpdate");
    }

    private void OnJabUpdate(AnimatorEventArgs e)
    {
        Debug.Log("OnJabUpdate");
    }

    /////////////////////////////////

    private void ParseInputSignal()
    {
        m_run = m_input.KeyAPressing;

        if (m_input.KeyCPressed)
        {
            m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_ATTACK);
        }

        if(m_input.KeyBPressed)
        {
            m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_JUMP);
        }

        m_modelAnimator.SetFloat(ANIMATOR_PARA_NAME_FORWARD, m_input.Direction_MotionCurveValue);
    }

    private void ParseMotionSingal()
    {
        m_movingVector = m_input.Direction_MotionCurveValue * m_model.forward * m_moveSpeed * (m_run ? m_runScale : 1f);
        m_rigidbody.velocity = new Vector3(m_movingVector.x, m_rigidbody.velocity.y, m_movingVector.z) + m_thrustVector3;
        RotateModel();
    }

    private bool IsJumping()
    {
        return m_rigidbody.velocity.y > 0.01f || m_rigidbody.velocity.y < -0.01f;
    }

    private bool IsIdle()
    {
        return m_input.Direction_MotionCurveValue <= 0.1f;
    }

    private bool IsAnimatorInState(string stateName, string layerName = ANIMATOR_LAYER_NAME_0)
	{
		return m_modelAnimator.GetCurrentAnimatorStateInfo(m_modelAnimator.GetLayerIndex(layerName)).IsName(stateName);
	}

    private void DectectCollision()
    {
        IsGrounded = CollisionDetector.DectectCollision(m_capcaol, LAYER_MASK_NAME_GROUND, m_adjustCollision);
        m_modelAnimator.SetBool(ANIMATOR_PARA_NAME_IS_GROUND, IsGrounded);
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
}
