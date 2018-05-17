using UnityEngine;

public class InputModule : MonoBehaviour {

    private static bool INPUT_ENABLE = true;
    private const float MOVE_MOTION_SACLE = 1f;
    private const float RUN_MOTION_SCALE = 2f;

    [Header("Key Board")]
    [SerializeField] private string m_keyUp = "w";
	[SerializeField] private string m_keyDown = "s";
	[SerializeField] private string m_keyRight = "d";
	[SerializeField] private string m_keyLeft = "a";
    [SerializeField] private string m_keyA = "left shift";
    [SerializeField] private string m_keyB = "space";
    [SerializeField] private string m_keyC = "j";
    [SerializeField] private string m_keyD = "l";
    [Header("Joy Stick")]
    [SerializeField] private string m_keyUp_Joy = "up";
    [SerializeField] private string m_keyDown_Joy = "down";
    [SerializeField] private string m_keyRight_Joy = "right";
    [SerializeField] private string m_keyLeft_Joy = "left";
    [Header("properties")]
    [SerializeField] private float m_moveSmoothTime = 0.1f;

    public float Direction_Vertical { get { return m_direction_vertical; } }
    public float Direction_Horizontal { get { return m_direction_horizontal; } }
    public float Direction_MotionCurveValue { get { return m_direction_motionCurveValue; } }
    public Vector3 Direction_Vector { get { return m_direction_vector; } }

    public bool KeyAPressed { get { return m_keyAState; } }
    public bool KeyBPressed { get { return m_keyBState;} }
    public bool KeyCPressed { get { return m_keyCState; } }
    public bool KeyAPressing { get; private set; }
    public bool KeyBPressing { get; private set; }
    public bool KeyCPressing { get; private set; }

    public float JoyStick_Vertical { get { return m_joyStick_vertical; } }
    public float JoyStick_Horizontal { get { return m_joyStick_horizontal; } }

    private float m_direction_vertical = 0f;
	private float m_direction_horizontal = 0f;

    private float m_target_direction_vertical = 0f;
    private float m_target_direction_horizontal = 0f;
    private float m_velocity_direction_vertical = 0f;
    private float m_velocity_direction_horizontal = 0f;

    private float m_direction_motionCurveValue = 0f;
    private Vector3 m_direction_vector = Vector3.zero;

    private float m_joyStick_vertical = 0f;
    private float m_joyStick_horizontal = 0f;

    private float m_target_motionCurveValue = 0f;
    private bool m_keyAState = false;
    private bool m_lastKeyAState = false;
    private bool m_keyBState = false;
    private bool m_lastKeyBState = false;
    private bool m_keyCState = false;
    private bool m_lastkeyCState = false;

    private void Update()
	{
        DetectInput_JoyStick(m_keyUp_Joy, m_keyDown_Joy, m_keyRight_Joy, m_keyLeft_Joy);
        DetectInput_Direction(m_keyUp, m_keyDown, m_keyRight, m_keyLeft, m_keyA, m_keyB);
        DetectTriggerOnce(m_keyA, ref m_keyAState, ref m_lastKeyAState);
        DetectTriggerOnce(m_keyB, ref m_keyBState, ref m_lastKeyBState);
        DetectTriggerOnce(m_keyC, ref m_keyCState, ref m_lastkeyCState);
        KeyAPressing = Input.GetKey(m_keyA);
        KeyBPressing = Input.GetKey(m_keyB);
        KeyCPressing = Input.GetKey(m_keyC);
    }

    private void DetectInput_JoyStick(string up, string down, string right, string left)
    {
        m_joyStick_vertical = (Input.GetKey(up) ? 1f : 0f) - (Input.GetKey(down) ? 1f : 0f);
        m_joyStick_horizontal = (Input.GetKey(right) ? 1f : 0f) - (Input.GetKey(left) ? 1f : 0f);
    }

    private void DetectInput_Direction(string up, string down, string right, string left, string a, string b)
    {
        if (INPUT_ENABLE)
        {
            float vertical = 0f;
            float horizontal = 0f;

            vertical = (Input.GetKey(up) ? 1f : 0f) - (Input.GetKey(down) ? 1f : 0f);
            horizontal = (Input.GetKey(right) ? 1f : 0f) - (Input.GetKey(left) ? 1f : 0f);
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
        m_target_direction_horizontal = target_direction_InCircle_horizontal * (KeyAPressing ? RUN_MOTION_SCALE : MOVE_MOTION_SACLE);
        m_target_direction_vertical = target_direction_InCircle_vertical * (KeyAPressing ? RUN_MOTION_SCALE : MOVE_MOTION_SACLE);

        // 避免瞬間變值導致詭異的角色移動表現，用SmoothDamp製造類似遞增遞減的效果
        m_direction_horizontal = Mathf.SmoothDamp(m_direction_horizontal, m_target_direction_horizontal, ref m_velocity_direction_horizontal, m_moveSmoothTime);
        m_direction_vertical = Mathf.SmoothDamp(m_direction_vertical, m_target_direction_vertical, ref m_velocity_direction_vertical, m_moveSmoothTime);

        // 用現在的水平值跟垂直值取得目前動作動畫的變化變量
        m_direction_motionCurveValue = Mathf.Sqrt((m_direction_vertical * m_direction_vertical) + (m_direction_horizontal * m_direction_horizontal));

        // 給予目前正在移動的方向向量
        m_direction_vector = (m_direction_horizontal * Vector3.right + m_direction_vertical * Vector3.forward);
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

    private void DetectTriggerOnce(string buttonString, ref bool state, ref bool lastState)
    {
        bool pressed = Input.GetKey(buttonString);
        if (pressed != lastState)
        {
            state = true;
        }
        else
        {
            state = false;
        }
        lastState = pressed;
    }

}
