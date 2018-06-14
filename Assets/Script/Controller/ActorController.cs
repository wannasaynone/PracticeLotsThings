using System;
using UnityEngine;

public class ActorController : Page {

    private enum InputType
    {
        KeyBoard,
        JoyStick
    }

    public enum MoveState
    {
        None,
        Move,
        Run,
        Jump,
        Fall,
        Roll,
        Hurt
    }

    public enum AttackState
    {
        None,
        Attack,
        Defense
    }

    private const float MOVE_MOTION_SACLE = 1f;
    private const float RUN_MOTION_SCALE = 2f;

    private const string LAYER_MASK_NAME_GROUND = "Ground";
    private const string LAYER_MASK_NAME_CHARACTER = "Character";

    private const string ANIMATOR_PARA_NAME_FORWARD = "forward";
    private const string ANIMATOR_PARA_NAME_JUMP = "jump";
    private const string ANIMATOR_PARA_NAME_ATTACK = "attack";
    private const string ANIMATOR_PARA_NAME_IS_GROUND = "isGround";
    private const string ANIMATOR_PARA_NAME_ROLL = "roll";
    private const string ANIMATOR_PARA_NAME_ATTACK_CURVE = "attackVelocityCurve";
    private const string ANIMATOR_PARA_NAME_DEFENSE = "defense";
    private const string ANIMATOR_PARA_NAME_HURT = "hurt";

    private const string ANIMATOR_STATE_NAME_GROUND = "ground";
    private const string ANIMATOR_STATE_NAME_IDLE = "idle";
	private const string ANIMATOR_STATE_NAME_ATTACK_1HA = "attack1hA";
	private const string ANIMATOR_STATE_NAME_ATTACK_1HB = "attack1hB";
	private const string ANIMATOR_STATE_NAME_ATTACK_1HC = "attack1hC";

    private const string ANIMATOR_LAYER_NAME_BASE_LAYER = "Base Layer";
    private const string ANIMATOR_LAYER_NAME_ATTACK = "attack";
    private const string ANIMATOR_LAYER_NAME_DEFENSE = "defense";

    /////////////////////////////////////////////////////////////////////////////////////

    public Animator ModelAnimator { get { return m_modelAnimator; } }
    public Collider ModelCollider { get { return m_capcaol; } }
    public Vector3 Direction_Vector { get { return m_direction_vector; } }
    public float Direction_MotionCurveValue { get { return m_direction_motionCurveValue; } }
    public InputDetecter InputDetecter { get { return m_inputDetector; } }
    public AttackState CurrentAttackState { get { return m_currentAttackState; } }
    public MoveState CurrentMoveState { get { return m_currentMoveState; } }
    public bool IsGrounded { get; private set; }
    public bool IsLockOn { get { return m_lockOnTarget != null; } }
    public Transform LockOnTarget { get { return m_lockOnTarget.transform; } }
    public CharacterStatus CharacterStatus { get; private set; }

    /////////////////////////////////////////////////////////////////////////////////////

    [Header("Sub UI")]
    [SerializeField] private CharacterStatusDisplayer m_characterStatusDisplayer;
    [Header("Inputer")]
    [SerializeField] private InputDetecter m_inputDetector = null;
    [Header("Properties")]
    [SerializeField] private float m_moveSmoothTime = 0.1f;
    [SerializeField] private AnimationEventReceiver m_animationEventReceiver;
    [SerializeField] private PhysicMaterial m_frictionOne;
    [SerializeField] private PhysicMaterial m_frictionZero;
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

    /////////////////////////////////////////////////////////////////////////////////////

    private float m_direction_vertical = 0f;
    private float m_direction_horizontal = 0f;

    private float m_target_direction_vertical = 0f;
    private float m_target_direction_horizontal = 0f;
    private float m_velocity_direction_vertical = 0f;
    private float m_velocity_direction_horizontal = 0f;
    private float m_direction_motionCurveValue = 0f;
    private float m_target_motionCurveValue = 0f;
    private Vector3 m_direction_vector = Vector3.zero;

    private float m_attackLayerWeight = 0f;

    private AttackState m_currentAttackState = AttackState.None;
    private MoveState m_currentMoveState = MoveState.None;
    private bool m_run = false;
    private bool m_defense = false;
    private GameObject m_lockOnTarget = null;

    private Vector3 m_movingVector = Vector3.zero;
    private Transform m_model = null;

    private bool m_lockUpdateInputVelocity = false;
    private bool m_lockAttack = false;
	private Vector3 m_animatorRootDeltaPostion = Vector3.zero;

    /////////////////////////////////////////////////////////////////////////////////////

    protected override void Awake()
    {
        base.Awake();
        m_model = m_modelAnimator.transform;

        AnimatorEventSender.RegisterOnStateEntered("ground", this, OnGroundEnter);
        AnimatorEventSender.RegisterOnStateEntered("jump", this, OnJumpEnter);
        AnimatorEventSender.RegisterOnStateEntered("fall", this, OnFallEnter);
        AnimatorEventSender.RegisterOnStateEntered("roll", this, OnRollEnter);
        AnimatorEventSender.RegisterOnStateEntered("jab", this, OnJabEnter);
        AnimatorEventSender.RegisterOnStateEntered("attack_idle", this, OnAttackIdleEnter);
        AnimatorEventSender.RegisterOnStateEntered("attack", this, OnAttackEnter);
        AnimatorEventSender.RegisterOnStateEntered("defense_idle", this, OnDefenseIdleEnter);
        AnimatorEventSender.RegisterOnStateEntered("defence", this, OnDefenceEnter);
        AnimatorEventSender.RegisterOnStateEntered("hurt", this, OnHurtEnter);

        AnimatorEventSender.RegisterOnStateUpdated("ground", this, OnGroundUpdate);
        AnimatorEventSender.RegisterOnStateUpdated("jump", this, OnJumpUpdate);
        AnimatorEventSender.RegisterOnStateUpdated("fall", this, OnFallUpdate);
        AnimatorEventSender.RegisterOnStateUpdated("roll", this, OnRollUpdate);
        AnimatorEventSender.RegisterOnStateUpdated("jab", this, OnJabUpdate);
        AnimatorEventSender.RegisterOnStateUpdated("attack_idle", this, OnAttackIdleUpdate);
        AnimatorEventSender.RegisterOnStateUpdated("attack", this, OnAttackUpdate);
        AnimatorEventSender.RegisterOnStateUpdated("defense_idle", this, OnDefenseIdleUpdate);
        AnimatorEventSender.RegisterOnStateUpdated("defense", this, OnDefenseUpdate);
        AnimatorEventSender.RegisterOnStateUpdated("hurt", this, OnHurtUpdate);

        m_animationEventReceiver.RegistOnUpdatedRootMotion(OnAnimatorRootMotionUpdate);

        HitBox.OnHitOthers += OnGetHit;

        CharacterStatus = new CharacterStatus(m_characterStatusDisplayer);
    }

    private void Update()
    {
        DetectInput();
        ParseInputSignal();
        DectectCollision();
        // TODO: update lock target by nearest game object (get game object list from game manager)
    }

    private void FixedUpdate()
    {
        ParseMotionSingal();
    }

    /////////////////////////////////////////////////////////////////////////////////////

    private void OnGroundEnter(AnimatorEventArgs e)
    {
        m_currentMoveState = MoveState.None;
        m_capcaol.material = m_frictionOne;
    }

    private void OnJumpEnter(AnimatorEventArgs e)
    {
        m_currentMoveState = MoveState.Jump;
        m_lockUpdateInputVelocity = true;
        m_rigidbody.velocity += new Vector3(0f, m_jumpThrust, 0f);
        m_capcaol.material = m_frictionZero;
    }

    private void OnFallEnter(AnimatorEventArgs e)
    {
        m_currentMoveState = MoveState.Fall;
    }

    private void OnRollEnter(AnimatorEventArgs e)
    {
        m_currentMoveState = MoveState.Roll;
        m_lockUpdateInputVelocity = true;
    }

    private void OnJabEnter(AnimatorEventArgs e)
    {
        m_currentMoveState = MoveState.Jump;
        m_lockUpdateInputVelocity = true;
        m_rigidbody.velocity += new Vector3(0f, m_jumpThrust / 2f, 0f);
        m_capcaol.material = m_frictionZero;
    }

    private void OnAttackEnter(AnimatorEventArgs e)
    {
        m_currentAttackState = AttackState.Attack;
        m_lockAttack = true;
        m_animationEventReceiver.RegistAction(UnlockAttack);
    }

    private void OnAttackIdleEnter(AnimatorEventArgs e)
    {
        m_currentAttackState = AttackState.None;
    }

    private void OnDefenseIdleEnter(AnimatorEventArgs e)
    {
        m_currentAttackState = AttackState.None;
    }

    private void OnDefenceEnter(AnimatorEventArgs e)
    {
        m_currentAttackState = AttackState.Defense;
    }

    private void OnHurtEnter(AnimatorEventArgs e)
    {
        m_currentMoveState = MoveState.Hurt;
    }

    private void OnGroundUpdate(AnimatorEventArgs e)
    {
        if (m_currentMoveState != MoveState.None
            && m_currentMoveState != MoveState.Move
            && m_currentMoveState != MoveState.Run)
        {
            return;
        }

        if (m_lockUpdateInputVelocity)
        {
            m_lockUpdateInputVelocity = false;
        }

        if (Direction_MotionCurveValue > 0.1f)
        {
            if (m_run)
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
    }

    private void OnJumpUpdate(AnimatorEventArgs e)
    {
        if (m_currentMoveState != MoveState.Jump)
        {
            return;
        }
        m_rigidbody.velocity = new Vector3(m_movingVector.x, m_rigidbody.velocity.y, m_movingVector.z);
    }

    private void OnFallUpdate(AnimatorEventArgs e)
    {
        if (m_currentMoveState != MoveState.Fall)
        {
            return;
        }
    }

    private void OnRollUpdate(AnimatorEventArgs e)
    {
        if (m_currentMoveState != MoveState.Roll && m_currentAttackState != AttackState.None)
        {
            return;
        }
        ForceCancelAttack();

        m_rigidbody.velocity = new Vector3(m_model.forward.x * m_rollThrust, m_rigidbody.velocity.y, m_model.forward.z * m_rollThrust);
    }

    private void OnJabUpdate(AnimatorEventArgs e)
    {
        if (m_currentMoveState != MoveState.Jump)
        {
            return;
        }

        m_rigidbody.velocity = new Vector3(m_model.forward.x * -m_jadThrust, m_rigidbody.velocity.y, m_model.forward.z * -m_jadThrust);
    }

    private void OnAttackIdleUpdate(AnimatorEventArgs e)
    {
        if (m_currentAttackState == AttackState.Attack)
        {
            return;
        }

        LerpMotionLayerWeight(ANIMATOR_LAYER_NAME_ATTACK, 0f, 0.025f);
    }

    private void OnAttackUpdate(AnimatorEventArgs e)
    {
        if (m_currentAttackState != AttackState.Attack)
        {
            return;
        }

        LerpMotionLayerWeight(ANIMATOR_LAYER_NAME_ATTACK, 1f, 0.25f);
    }

    private void OnDefenseIdleUpdate(AnimatorEventArgs e)
    {
        if (m_currentAttackState == AttackState.Defense)
        {
            return;
        }

        LerpMotionLayerWeight(ANIMATOR_LAYER_NAME_DEFENSE, 0f, 0.025f);
    }

    private void OnDefenseUpdate(AnimatorEventArgs e)
    {
        if (m_currentAttackState != AttackState.Defense)
        {
            return;
        }

        LerpMotionLayerWeight(ANIMATOR_LAYER_NAME_DEFENSE, 1f, 0.25f);
    }

    private void OnHurtUpdate(AnimatorEventArgs e)
    {
        if(m_currentMoveState != MoveState.Hurt)
        {
            return;
        }

        m_lockUpdateInputVelocity = true;
    }

    private void OnAnimatorRootMotionUpdate(Vector3 value)
	{
		if(IsAnimatorInState(ANIMATOR_STATE_NAME_ATTACK_1HA, ANIMATOR_LAYER_NAME_ATTACK)
		   || IsAnimatorInState(ANIMATOR_STATE_NAME_ATTACK_1HB, ANIMATOR_LAYER_NAME_ATTACK)
		   || IsAnimatorInState(ANIMATOR_STATE_NAME_ATTACK_1HC, ANIMATOR_LAYER_NAME_ATTACK))
		{
			m_animatorRootDeltaPostion += value * 0.5f;
		}
	}

    private void LerpMotionLayerWeight(string layerName, float target, float speed)
    {
        float currentWeight = m_modelAnimator.GetLayerWeight(m_modelAnimator.GetLayerIndex(layerName));
        currentWeight = Mathf.Lerp(currentWeight, target, speed);
        m_modelAnimator.SetLayerWeight(m_modelAnimator.GetLayerIndex(layerName), currentWeight);
    }

    private void OnGetHit(object sender, HitBox.HitEventArgs e)
    {
        if(e.Defender == this)
        {
            m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_HURT);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////

    private void DetectInput()
    {
        m_inputDetector.DetectInput();
    }

    private void ParseInputSignal()
    {
        m_run = m_inputDetector.KeyAPressing;
        m_defense = m_inputDetector.KeyDPressing;

        if (m_inputDetector.KeyCPressed && (m_currentMoveState == MoveState.None || m_currentMoveState == MoveState.Move || m_currentMoveState == MoveState.Run))
        {
            if (!IsAnimatorInState(ANIMATOR_STATE_NAME_GROUND))
            {
                ForceCancelAttack();
                return;
            }

            if (!m_lockAttack && !m_defense)
            {
                m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_ATTACK);
            }
        }

        m_modelAnimator.SetBool(ANIMATOR_PARA_NAME_DEFENSE, m_defense);

        if (m_inputDetector.KeyBPressed && m_currentAttackState == AttackState.None)
        {
            m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_JUMP);
        }

        m_modelAnimator.SetFloat(ANIMATOR_PARA_NAME_FORWARD, Direction_MotionCurveValue);

        if (InputDetecter.KeyEPressed)
        {
            if(IsLockOn)
            {
                m_lockOnTarget = null;
            }
            else
            {
                // m_lockOnTarget = sth;
                // m_lockOnTarget = Test; // 測試用
            }
        }
    }

    private void ParseMotionSingal()
    {
        SetDirection(m_inputDetector.LeftKey_Vertical, m_inputDetector.LeftKey_Horizontal);
        ApplyInputMotion();
		ApplyAnimatorRootMotion();
    }

    private void ApplyAnimatorRootMotion()
	{
		m_rigidbody.position += m_animatorRootDeltaPostion;
        m_animatorRootDeltaPostion = Vector3.zero;
	}

	private void ApplyInputMotion()
    {
        if (m_currentAttackState == AttackState.Attack && IsLockOn)
        {
            // 鎖定模式下攻擊的話應該要轉向攻擊目標攻擊
            m_model.LookAt(m_lockOnTarget.transform);
            return;
        }

        if (m_currentAttackState != AttackState.Attack)
        {
			if (Mathf.Abs(m_rigidbody.velocity.y) > 5f && IsGrounded)
            {
                m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_ROLL);
            }

            if (!m_lockUpdateInputVelocity)
            {
                m_movingVector = Direction_MotionCurveValue * m_model.forward * m_moveSpeed * (m_run ? m_runScale : 1f);
                m_rigidbody.velocity = new Vector3(m_movingVector.x, m_rigidbody.velocity.y, m_movingVector.z) * (1f - GetAnimatorWeight(ANIMATOR_LAYER_NAME_ATTACK));
                RotateModel();
            }
            return;
        }      
	}

    private void SetDirection(float vertical, float horizontal)
    {
        // "前後左右"會根據狀況改變，所以不做"位移"(Vector3.forward * speed...之類的)，先做玩家輸入的方向判斷
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        vertical = Mathf.Clamp(vertical, -1f, 1f);

        m_target_direction_horizontal = horizontal * (m_inputDetector.KeyAPressing ? RUN_MOTION_SCALE : MOVE_MOTION_SACLE);
        m_target_direction_vertical = vertical * (m_inputDetector.KeyAPressing ? RUN_MOTION_SCALE : MOVE_MOTION_SACLE);

        // 避免瞬間變值導致詭異的角色移動表現，用SmoothDamp製造類似遞增遞減的效果
        m_direction_horizontal = Mathf.SmoothDamp(m_direction_horizontal, m_target_direction_horizontal, ref m_velocity_direction_horizontal, m_moveSmoothTime);
        m_direction_vertical = Mathf.SmoothDamp(m_direction_vertical, m_target_direction_vertical, ref m_velocity_direction_vertical, m_moveSmoothTime);

        // 用現在的水平值跟垂直值取得目前動作動畫的變化變量
        m_direction_motionCurveValue = Mathf.Sqrt((m_direction_vertical * m_direction_vertical) + (m_direction_horizontal * m_direction_horizontal));

        // 給予目前正在移動的方向向量
        m_direction_vector = (m_direction_horizontal * Vector3.right + m_direction_vertical * Vector3.forward);
    }

    public bool IsJumping()
    {
        return m_rigidbody.velocity.y > 0.01f || m_rigidbody.velocity.y < -0.01f;
    }

    public bool IsIdle()
    {
        // 只要有其他LAYER正在執行，就判定為非idle
        if (m_modelAnimator.GetLayerWeight(m_modelAnimator.GetLayerIndex(ANIMATOR_LAYER_NAME_ATTACK)) > 0.1f)
        {
            return false;
        }

        if (m_currentMoveState != MoveState.None && m_currentMoveState != MoveState.Move
            && m_currentMoveState != MoveState.Run && m_currentAttackState != AttackState.None)
        {
            return false;
        }

        return Direction_MotionCurveValue <= 0.1f;
    }

    private void UnlockAttack()
    {
        m_animationEventReceiver.UnregistAction(UnlockAttack);
        m_lockAttack = false;
    }

    private bool IsAnimatorInState(string stateName, string layerName = ANIMATOR_LAYER_NAME_BASE_LAYER)
    {
        return m_modelAnimator.GetCurrentAnimatorStateInfo(m_modelAnimator.GetLayerIndex(layerName)).IsName(stateName);
    }

    private float GetAnimatorWeight(string layerName)
    {
        return m_modelAnimator.GetLayerWeight(m_modelAnimator.GetLayerIndex(layerName));
    }

    private void DectectCollision()
    {
        Vector3 _center = m_capcaol.transform.position + new Vector3(0f, m_capcaol.height / 2f, 0f) + m_adjustCollision;
        Debug.DrawRay(_center, Vector3.down, Color.red);
        IsGrounded = Physics.Raycast(_center, Vector3.down, m_capcaol.height, LayerMask.GetMask(LAYER_MASK_NAME_GROUND));
        m_modelAnimator.SetBool(ANIMATOR_PARA_NAME_IS_GROUND, IsGrounded);
    }

    private void RotateModel()
    {
        // 避免在水平值和垂直值過低時重設模型為"正面"
        if ((Mathf.Abs(m_inputDetector.LeftKey_Horizontal) <= 0.1 && Mathf.Abs(m_inputDetector.LeftKey_Vertical) <= 0.1) || m_lockUpdateInputVelocity)
        {
            return;
        }
        else
        {
            // 水平向量 * 輸入變量 + 垂直向量 * 輸入變量 = 斜向向量 -> EX 0度向量 + 90度向量 = 45度向量 = 模型對面方向
            m_model.forward = Vector3.Slerp(m_model.forward, transform.right * m_inputDetector.LeftKey_Horizontal + transform.forward * m_inputDetector.LeftKey_Vertical, m_rotateSpeed);
        }
    }

    private void ForceCancelAttack()
    {
        m_modelAnimator.SetLayerWeight(m_modelAnimator.GetLayerIndex(ANIMATOR_LAYER_NAME_ATTACK), 0f);
        m_modelAnimator.Play(ANIMATOR_STATE_NAME_IDLE, m_modelAnimator.GetLayerIndex(ANIMATOR_LAYER_NAME_ATTACK));
    }

}
