using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModule : MonoBehaviour {

    private static bool INPUT_ENABLE = true;
    private const float MOVE_MOTION_SACLE = 1f;
    private const float RUN_MOTION_SCALE = 2f;

    [SerializeField] private string m_keyUp = "w";
	[SerializeField] private string m_keyDown = "s";
	[SerializeField] private string m_keyRight = "d";
	[SerializeField] private string m_keyLeft = "a";
    [SerializeField] private string m_keyA = "left shift";
    [SerializeField] private string m_keyB = "space";
    [SerializeField] private string m_keyC = "q";
    [SerializeField] private string m_keyD = "r";
    [SerializeField] private float m_moveSmoothTime = 0.1f;

    public float Direction_Vertical { get { return m_direction_vertical; } }
    public float Direction_Horizontal { get { return m_direction_horizontal; } }
    public float Direction_MotionCurveValue { get { return m_direction_motionCurveValue; } }
    public Vector3 Direction_Vector { get { return m_direction_vector; } }
    public bool Running { get { return m_isRun; } }
    public bool Jump { get { return m_jumpState;} }

    private float m_direction_vertical = 0f;
	private float m_direction_horizontal = 0f;

    private float m_target_direction_vertical = 0f;
    private float m_target_direction_horizontal = 0f;
    private float m_velocity_direction_vertical = 0f;
    private float m_velocity_direction_horizontal = 0f;

    private float m_direction_motionCurveValue = 0f;
    private Vector3 m_direction_vector = Vector3.zero;

    private float m_target_motionCurveValue = 0f;
    private bool m_isRun = false;
    private bool m_jumpState = false;
    private bool m_lockMovement = false;

    private void Update()
	{
        DetectInput();
    }

    private void DetectInput()
    {
        // 未來有可能會根據情況，讓玩家不能輸入，但還是要做角色移動(EX 過場)
        if (INPUT_ENABLE)
        {
            float vertical = 0f;
            float horizontal = 0f;

            if(!m_lockMovement)
            {
                vertical = (Input.GetKey(m_keyUp) ? 1f : 0f) - (Input.GetKey(m_keyDown) ? 1f : 0f);
                horizontal = (Input.GetKey(m_keyRight) ? 1f : 0f) - (Input.GetKey(m_keyLeft) ? 1f : 0f);
                m_isRun = Input.GetKey(m_keyA);
            }

            m_jumpState = Input.GetKeyDown(m_keyB);
            SetDirection(vertical, horizontal);
        }
    }

    private void SetDirection(float vertical, float horizontal)
    {
        // "前後左右"會根據狀況改變，所以不做"位移"(Vector3.forward * speed...之類的)，先做玩家輸入的方向判斷
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        vertical = Mathf.Clamp(vertical, -1f, 1f);

        // 先做轉換
        Vector2 target_direction_InCircle = TransferSquareViewToCircleView(new Vector2(horizontal, vertical));
        float target_direction_InCircle_horizontal = target_direction_InCircle.x;
        float target_direction_InCircle_vertical = target_direction_InCircle.y;

        // 才做Run Scale放大縮小，避免float NaN的問題
        m_target_direction_horizontal = target_direction_InCircle_horizontal * (m_isRun ? RUN_MOTION_SCALE : MOVE_MOTION_SACLE);
        m_target_direction_vertical = target_direction_InCircle_vertical * (m_isRun ? RUN_MOTION_SCALE : MOVE_MOTION_SACLE);

        // 避免瞬間變值導致詭異的角色移動表現，用SmoothDamp製造類似遞增遞減的效果
        m_direction_horizontal = Mathf.SmoothDamp(m_direction_horizontal, m_target_direction_horizontal, ref m_velocity_direction_horizontal, m_moveSmoothTime);
        m_direction_vertical = Mathf.SmoothDamp(m_direction_vertical, m_target_direction_vertical, ref m_velocity_direction_vertical, m_moveSmoothTime);

        // 用現在的水平值跟垂直值取得目前動作動畫的變化變量
        m_direction_motionCurveValue = Mathf.Sqrt((m_direction_vertical * m_direction_vertical) + (m_direction_horizontal * m_direction_horizontal));

        // 給予目前正在移動的方向向量
        m_direction_vector = (m_direction_horizontal * transform.right + m_direction_vertical * transform.forward);
    }

    // https://arxiv.org/ftp/arxiv/papers/1509/1509.06344.pdf
    private Vector2 TransferSquareViewToCircleView(Vector2 input)
    {
        Vector2 output = Vector2.zero;

        input.x = Mathf.Round(input.x);
        input.y = Mathf.Round(input.y);

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2f);

        return output;
    }
}
