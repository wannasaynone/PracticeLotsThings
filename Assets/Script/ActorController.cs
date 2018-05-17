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
    private bool m_jump = false;
    private bool m_attack = false;

    private Vector3 m_thrustVector3 = Vector3.zero;
    private Vector3 m_movingVector = Vector3.zero;
    private Transform m_model = null;

    private bool m_lockUpdateInputVelocity = false;

    private void Awake()
    {
        m_model = m_modelAnimator.transform;
    }

    private void Update ()
    {
        ParseInputSingle();
        DectectCollision(); 
    }

    private void FixedUpdate()
    {
        SetVelocity();
    }

    /////////////////////////////////

    private void ParseInputSingle()
    {
        m_attack = m_input.KeyCPressed;
        m_jump = m_input.KeyBPressed;
        m_run = m_input.KeyAPressing;
    }

    private void SetVelocity()
    {
		if(!m_lockUpdateInputVelocity)
        {
            m_rigidbody.velocity = new Vector3(m_movingVector.x, m_rigidbody.velocity.y, m_movingVector.z) + m_thrustVector3;
        }

        m_thrustVector3 = Vector3.zero;
    }

    private void DetectMove()
    {
        RotateModel();
        m_modelAnimator.SetFloat(ANIMATOR_PARA_NAME_FORWARD, m_input.Direction_MotionCurveValue);
        m_movingVector = m_input.Direction_MotionCurveValue * m_model.forward * m_moveSpeed * (m_run ? m_runScale : 1f);
    }

	private bool IsAnimatorInState(string stateName, string layerName = ANIMATOR_LAYER_NAME_0)
	{
		return m_modelAnimator.GetCurrentAnimatorStateInfo(m_modelAnimator.GetLayerIndex(layerName)).IsName(stateName);
	}

    private void DectectCollision()
    {
        IsGrounded = CollisionDetector.DectectCollision(m_capcaol, LAYER_MASK_NAME_GROUND, m_adjustCollision);
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
