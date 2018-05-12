using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    private const string ANIMATOR_PARA_NAME_FORWARD = "forward";
    private const string ANIMATOR_PARA_NAME_JUMP = "jump";

    [SerializeField] private InputModule m_input = null;
    [SerializeField] private Animator m_modelAnimator = null;
    [SerializeField] private Rigidbody m_rigidbody = null;
    [SerializeField] private float m_rotateSpeed = 10f;
    [SerializeField] private float m_moveSpeed = 1.5f;
    [SerializeField] private float m_runScale = 2.5f;

    private Vector3 m_movingVector = Vector3.zero;
    private Transform m_model = null;

    private void Awake()
    {
        m_model = m_modelAnimator.transform;
    }

    private void Update ()
    {
        SetMoveMotion();
    }

    private void FixedUpdate()
    {
        m_rigidbody.velocity = new Vector3(m_movingVector.x, m_rigidbody.velocity.y, m_movingVector.z);
    }

    private void SetMoveMotion()
    {
        m_modelAnimator.SetFloat(ANIMATOR_PARA_NAME_FORWARD, m_input.Direction_MotionCurveValue);
        if(m_input.Jump)
        {
            m_modelAnimator.SetTrigger(ANIMATOR_PARA_NAME_JUMP);
        }
        m_movingVector = m_input.Direction_MotionCurveValue * m_model.forward * m_moveSpeed * (m_input.Running ? m_runScale  : 1f);
        RotateModel();
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

}
