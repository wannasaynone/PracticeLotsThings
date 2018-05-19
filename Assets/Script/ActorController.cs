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
    private const string ANIMATOR_STATE_NAME_IDLE = "idle";

    private const string ANIMATOR_LAYER_NAME_0 = "Base Layer";
    private const string ANIMATOR_LAYER_NAME_1 = "attack";

    public GameObject Model { get { return m_model.gameObject; } }
    public bool IsGrounded { get; private set; }

    [SerializeField] private PhysicMaterial m_frictionOne;
    [SerializeField] private PhysicMaterial m_frictionZero;
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

    private AttackState m_currentAttackState = AttackState.None;
    private MoveState m_currentMoveState = MoveState.None;
    private bool m_run = false;

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
        AnimatorEventSender.RegistOnStateEntered("idle", OnIdleEnter);
        AnimatorEventSender.RegistOnStateEntered("attack", OnAttackEnter);

        AnimatorEventSender.RegistOnStateUpdated("ground", OnGroundUpdate);
        AnimatorEventSender.RegistOnStateUpdated("jump", OnJumpUpdate);
        AnimatorEventSender.RegistOnStateUpdated("fall", OnFallUpdate);
        AnimatorEventSender.RegistOnStateUpdated("roll", OnRollUpdate);
        AnimatorEventSender.RegistOnStateUpdated("jab", OnJabUpdate);
        AnimatorEventSender.RegistOnStateUpdated("idle", OnIdleUpdate);
        AnimatorEventSender.RegistOnStateUpdated("attack", OnAttackUpdate);
    }

    private void Update ()
    {
        ParseInputSignal();
        DectectCollision();
    }

    private void FixedUpdate()
    {
        ParseMotionSingal();
    }

    private void OnGroundEnter(AnimatorEventArgs e)
    {
        m_currentMoveState = MoveState.None;
        m_capcaol.material = m_frictionOne;
        m_lockUpdateInputVelocity = false;
        // Debug.Log("OnGroundEnter");
    }

    private void OnJumpEnter(AnimatorEventArgs e)
    {
        m_currentMoveState = MoveState.Jump;
        m_lockUpdateInputVelocity = true;
        m_rigidbody.velocity += new Vector3(0f, m_jumpThrust, 0f);
        m_capcaol.material = m_frictionZero;
        // Debug.Log("OnJumpEnter");
    }

    private void OnFallEnter(AnimatorEventArgs e)
    {
        m_currentMoveState = MoveState.Fall;
        // Debug.Log("OnFallEnter");
    }

    private void OnRollEnter(AnimatorEventArgs e)
    {
        m_currentMoveState = MoveState.Roll;
        m_lockUpdateInputVelocity = true;
        // Debug.Log("OnRollEnter");
    }

    private void OnJabEnter(AnimatorEventArgs e)
    {
        m_currentMoveState = MoveState.Jump;
        m_lockUpdateInputVelocity = true;
        m_rigidbody.velocity += new Vector3(0f, m_jumpThrust / 2f, 0f);
        m_capcaol.material = m_frictionZero;
        // Debug.Log("OnJabEnter");
    }

    private void OnAttackEnter(AnimatorEventArgs e)
    {
        m_currentAttackState = AttackState.Attack;
        // Debug.Log("OnAttackEnter");
    }

    private void OnIdleEnter(AnimatorEventArgs e)
    {
        m_currentAttackState = AttackState.None;
        m_modelAnimator.SetLayerWeight(m_modelAnimator.GetLayerIndex(ANIMATOR_LAYER_NAME_1), 0f);
        // Debug.Log("OnIdleEnter");
    }

    private void OnGroundUpdate(AnimatorEventArgs e)
    {
        if(m_currentMoveState != MoveState.None && m_currentMoveState != MoveState.Move && m_currentMoveState != MoveState.Run)
        {
            return;
        }

        if(m_input.Direction_MotionCurveValue > 0.1f)
        {
            if(m_run)
            {
                m_currentMoveState = MoveState.Run;
            }
            else
            {
                m_currentMoveState = MoveState.Move;
            }
        }
        else
        {
            m_currentMoveState = MoveState.None;
        }

        // Debug.Log("OnGroundUpdate");
    }

    private void OnJumpUpdate(AnimatorEventArgs e)
    {
        if (m_currentMoveState != MoveState.Jump)
        {
            return;
        }
        m_rigidbody.velocity = new Vector3(m_movingVector.x, m_rigidbody.velocity.y, m_movingVector.z);
        // Debug.Log("OnJumpUpdate");
    }

    private void OnFallUpdate(AnimatorEventArgs e)
    {
        if (m_currentMoveState != MoveState.Fall)
        {
            return;
        }
        // Debug.Log("OnFallUpdate");
    }

    private void OnRollUpdate(AnimatorEventArgs e)
    {
        if (m_currentMoveState != MoveState.Roll && m_currentAttackState != AttackState.None)
        {
            return;
        }
        ForceCancelAttack();
        m_rigidbody.velocity = m_model.forward * m_rollThrust;
        // Debug.Log("OnRollUpdate");
    }

    private void OnJabUpdate(AnimatorEventArgs e)
    {
        if (m_currentMoveState != MoveState.Jump)
        {
            return;
        }
        m_rigidbody.velocity = new Vector3(m_model.forward.x * -m_jadThrust, m_rigidbody.velocity.y, m_model.forward.z * -m_jadThrust);
        // Debug.Log("OnJabUpdate");
    }

    private void OnIdleUpdate(AnimatorEventArgs e)
    {
        if (m_currentAttackState != AttackState.None)
        {
            return;
        }
        // Debug.Log("OnIdleUpdate");
    }

    private void OnAttackUpdate(AnimatorEventArgs e)
    {
        if (m_currentAttackState != AttackState.Attack)
        {
            return;
        }

        // Debug.Log("OnAttackUpdate");
    }

    /////////////////////////////////

    private void ParseInputSignal()
    {
        m_run = m_input.KeyAPressing;

        if (m_input.KeyCPressed && (m_currentMoveState == MoveState.None || m_currentMoveState == MoveState.Move || m_currentMoveState == MoveState.Run))
        {
            if (!IsAnimatorInState(ANIMATOR_STATE_NAME_GROUND))
            {
                ForceCancelAttack();
                return;
            }
            m_modelAnimator.SetLayerWeight(m_modelAnimator.GetLayerIndex(ANIMATOR_LAYER_NAME_1), 1f);
            m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_ATTACK);
        }

        if(m_input.KeyBPressed && m_currentAttackState == AttackState.None)
        {
            m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_JUMP);
        }

        m_modelAnimator.SetFloat(ANIMATOR_PARA_NAME_FORWARD, m_input.Direction_MotionCurveValue);
    }

    private void ParseMotionSingal()
    {
        if (m_currentAttackState != AttackState.None)
        {
            return;
        }

        if (Mathf.Abs(m_rigidbody.velocity.y) > 5f && IsGrounded)
        {
            m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_ROLL);
        }

        if (!m_lockUpdateInputVelocity)
        {
            m_movingVector = m_input.Direction_MotionCurveValue * m_model.forward * m_moveSpeed * (m_run ? m_runScale : 1f);
            m_rigidbody.velocity = new Vector3(m_movingVector.x, m_rigidbody.velocity.y, m_movingVector.z);
            RotateModel();
        }
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

    private void ForceCancelAttack()
    {
        m_modelAnimator.SetLayerWeight(m_modelAnimator.GetLayerIndex(ANIMATOR_LAYER_NAME_1), 0f);
        m_modelAnimator.Play(ANIMATOR_STATE_NAME_IDLE, m_modelAnimator.GetLayerIndex(ANIMATOR_LAYER_NAME_1));
    }

}
