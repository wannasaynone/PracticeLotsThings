using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    private const string LAYER_MASK_NAME_GROUND = "Ground";
    private const string ANIMATOR_PARA_NAME_FORWARD = "forward";
    private const string ANIMATOR_PARA_NAME_JUMP = "jump";
    private const string ANIMATOR_PARA_NAME_IS_GROUND = "isGround";

    [SerializeField] private InputModule m_input = null;
    [SerializeField] private Animator m_modelAnimator = null;
    [SerializeField] private Rigidbody m_rigidbody = null;
    [SerializeField] private CapsuleCollider m_capcaol = null;
    [SerializeField] private Vector3 m_adjustCollision = Vector3.zero;
    [SerializeField] private float m_rotateSpeed = 10f;
    [SerializeField] private float m_moveSpeed = 1.5f;
    [SerializeField] private float m_runScale = 2.5f;
    [SerializeField] private float m_jumpThrust = 4f;

    private Vector3 m_thrustVector3 = Vector3.zero;
    private Vector3 m_movingVector = Vector3.zero;
    private Transform m_model = null;

    private void Awake()
    {
        m_model = m_modelAnimator.transform;
        AnimatorEventSender.RegistOnStateEntered("jump", OnJumpEnter);
        AnimatorEventSender.RegistOnStateExited("jump", OnJumpExit);
    }

    private void Update ()
    {
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
        m_rigidbody.velocity = new Vector3(m_movingVector.x, m_rigidbody.velocity.y, m_movingVector.z) + m_thrustVector3;
        m_thrustVector3 = Vector3.zero;
    }

    private void SetMoveMotion()
    {
        m_modelAnimator.SetFloat(ANIMATOR_PARA_NAME_FORWARD, m_input.Direction_MotionCurveValue);
        m_modelAnimator.SetBool(ANIMATOR_PARA_NAME_JUMP, m_input.Jump);
        m_movingVector = m_input.Direction_MotionCurveValue * m_model.forward * m_moveSpeed * (m_input.Running ? m_runScale  : 1f);
        RotateModel();
    }

    private void SetDectectCollision()
    {
        if (CollisionDetector.DectectCollision(m_capcaol, "Ground", m_adjustCollision))
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
        if(Mathf.Abs(m_input.Direction_Horizontal) <= 0.1 && Mathf.Abs(m_input.Direction_Vertical) <= 0.1)
        {
            return;
        }
        else
        {
            m_model.forward = Vector3.Slerp(m_model.forward, m_input.Direction_Vector, m_rotateSpeed);
        }
    }

    private void OnIsOnGround()
    {
        m_modelAnimator.SetBool(ANIMATOR_PARA_NAME_IS_GROUND, true);
    }

    private void OnIsOutGround()
    {
        m_modelAnimator.SetBool(ANIMATOR_PARA_NAME_IS_GROUND, false);
    }

    private void OnJumpEnter(AnimatorEventArgs e)
    {
        m_thrustVector3 = new Vector3(0f, m_jumpThrust, 0f);
    }

    private void OnJumpExit(AnimatorEventArgs e)
    {

    }
}
