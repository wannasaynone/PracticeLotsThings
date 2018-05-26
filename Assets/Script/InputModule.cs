using UnityEngine;

public class InputModule : MonoBehaviour {

    public static InputModule Instance { get; private set; }

    private enum InputType
    {
        KeyBoard,
        JoyStick
    }

    private static bool INPUT_ENABLE = true;
    private const float MOVE_MOTION_SACLE = 1f;
    private const float RUN_MOTION_SCALE = 2f;

    [SerializeField] private InputType m_currentInputType = InputType.KeyBoard;

    [Header("Key Board")]
    [SerializeField] private string m_keyBoard_leftKey_Up = "w";
	[SerializeField] private string m_keyBoard_leftKey_Down = "s";
	[SerializeField] private string m_keyBoard_leftKey_Right = "d";
	[SerializeField] private string m_keyBoard_leftKey_Left = "a";
    [SerializeField] private string m_keyBoard_rightKey_Up = "up";
    [SerializeField] private string m_keyBoard_rightKey_Down = "down";
    [SerializeField] private string m_keyBoard_rightKey_Right = "right";
    [SerializeField] private string m_keyBoard_rightKey_Left = "left";
    [SerializeField] private string m_keyBoard_keyASetting = "left shift";
    [SerializeField] private string m_keyBoard_keyBSetting = "space";
    [SerializeField] private string m_keyBoard_keyCSetting = "j";
    [SerializeField] private string m_keyBoard_keyDSetting = "l";
    [Header("Joy Stick")]
    [SerializeField] private string m_joyStick_leftKey_VerticalAxis = "DUp";
    [SerializeField] private string m_joyStick_leftKey_HorizontalAxis = "DRight";
    [SerializeField] private string m_joyStick_rightKey_VerticalAxis = "JUp";
    [SerializeField] private string m_joyStick_rightKey_HorizontalAxis = "JRight";
    [SerializeField] private string m_joyStick_keyASetting = "Square";
    [SerializeField] private string m_joyStick_keyBSetting = "Cross";
    [SerializeField] private string m_joyStick_keyCSetting = "Circle";
    [SerializeField] private string m_joyStick_keyDSetting = "Triangle";
    [Header("properties")]
    [SerializeField] private float m_moveSmoothTime = 0.1f;

    private string m_keyA = "";
    private string m_keyB = "";
    private string m_keyC = "";
    private string m_keyD = "";

    public float LeftKey_Vertical { get { return m_direction_vertical; } }
    public float LeftKey_Horizontal { get { return m_direction_horizontal; } }
    public float RightKey_Vertical { get { return m_rightKey_vertical; } }
    public float RightKey_Horizontal { get { return m_rightKey_horizontal; } }

    public float Direction_MotionCurveValue { get { return m_direction_motionCurveValue; } }
    public Vector3 Direction_Vector { get { return m_direction_vector; } }

    public bool KeyAPressed { get { return m_keyAState; } }
    public bool KeyBPressed { get { return m_keyBState;} }
    public bool KeyCPressed { get { return m_keyCState; } }
    public bool KeyAPressing { get; private set; }
    public bool KeyBPressing { get; private set; }
    public bool KeyCPressing { get; private set; }

    private float m_direction_vertical = 0f;
	private float m_direction_horizontal = 0f;

    private float m_target_direction_vertical = 0f;
    private float m_target_direction_horizontal = 0f;
    private float m_velocity_direction_vertical = 0f;
    private float m_velocity_direction_horizontal = 0f;

    private float m_direction_motionCurveValue = 0f;
    private Vector3 m_direction_vector = Vector3.zero;

    private float m_rightKey_vertical = 0f;
    private float m_rightKey_horizontal = 0f;

    private float m_target_motionCurveValue = 0f;
    private bool m_keyAState = false;
    private bool m_lastKeyAState = false;
    private bool m_keyBState = false;
    private bool m_lastKeyBState = false;
    private bool m_keyCState = false;
    private bool m_lastkeyCState = false;

    private void Awake()
    {
        SetInstance();
    }

    private void Start()
    {
        SetInputButton();
    }

    private void Update()
	{
        DetectInput();
    }

    private void SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("InputModule Instance already exist");
        }
    }

    private void SetInputButton()
    {
        if (m_currentInputType == InputType.JoyStick)
        {
            m_keyA = m_joyStick_keyASetting;
            m_keyB = m_joyStick_keyBSetting;
            m_keyC = m_joyStick_keyCSetting;
            m_keyD = m_joyStick_keyDSetting;
        }
        else if (m_currentInputType == InputType.KeyBoard)
        {
            m_keyA = m_keyBoard_keyASetting;
            m_keyB = m_keyBoard_keyBSetting;
            m_keyC = m_keyBoard_keyCSetting;
            m_keyD = m_keyBoard_keyDSetting;
        }
    }

    private void DetectInput()
    {
        if (m_currentInputType == InputType.JoyStick)
        {
            DetectJoyStickInput();
        }
        else if (m_currentInputType == InputType.KeyBoard)
        {
            DetectKeyBoardInput();
        }
    }

    private void DetectJoyStickInput()
    {
        DetectInput_RightKey(m_joyStick_rightKey_VerticalAxis, m_joyStick_rightKey_HorizontalAxis);
        DetectInput_LeftKey(m_joyStick_leftKey_VerticalAxis, m_joyStick_leftKey_HorizontalAxis);
        m_keyAState = Input.GetButtonDown(m_keyA);
        m_keyBState = Input.GetButtonDown(m_keyB);
        m_keyCState = Input.GetButtonDown(m_keyC);
        KeyAPressing = Input.GetButton(m_keyA);
        KeyBPressing = Input.GetButton(m_keyB);
        KeyCPressing = Input.GetButton(m_keyC);
    }

    private void DetectKeyBoardInput()
    {
        DetectInput_RightKey(m_keyBoard_rightKey_Up, m_keyBoard_rightKey_Right, m_keyBoard_rightKey_Right, m_keyBoard_rightKey_Left);
        DetectInput_LeftKey(m_keyBoard_leftKey_Up, m_keyBoard_leftKey_Down, m_keyBoard_leftKey_Right, m_keyBoard_leftKey_Left, m_keyA, m_keyB);
        m_keyAState = Input.GetKeyDown(m_keyA);
        m_keyBState = Input.GetKeyDown(m_keyB);
        m_keyCState = Input.GetKeyDown(m_keyC);
        KeyAPressing = Input.GetKey(m_keyA);
        KeyBPressing = Input.GetKey(m_keyB);
        KeyCPressing = Input.GetKey(m_keyC);
    }

    private void DetectInput_RightKey(string vertical, string horizontal)
    {
        m_rightKey_vertical = Input.GetAxis(vertical);
        m_rightKey_horizontal = Input.GetAxis(horizontal);
    }

    private void DetectInput_LeftKey(string vertical, string horizontal)
    {
        if (INPUT_ENABLE)
        {
            SetDirection(Input.GetAxis(vertical), Input.GetAxis(horizontal));
        }
    }

    private void DetectInput_RightKey(string up, string down, string right, string left)
    {
        m_rightKey_vertical = (Input.GetKey(up) ? 1f : 0f) - (Input.GetKey(down) ? 1f : 0f);
        m_rightKey_horizontal = (Input.GetKey(right) ? 1f : 0f) - (Input.GetKey(left) ? 1f : 0f);
    }

    private void DetectInput_LeftKey(string up, string down, string right, string left, string a, string b)
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

        // 使用鍵盤的時候才有套用球面公式的必要(因為手柄採用讀取Axis)
        if (m_currentInputType == InputType.KeyBoard)
        {
            // 先做轉換
            Vector2 target_direction_InCircle = TransferSquareViewToCircleView(new Vector2(horizontal, vertical));
            float target_direction_InCircle_horizontal = target_direction_InCircle.x;
            float target_direction_InCircle_vertical = target_direction_InCircle.y;

            // 才做Run Scale放大縮小，避免float NaN的問題
            m_target_direction_horizontal = target_direction_InCircle_horizontal * (KeyAPressing ? RUN_MOTION_SCALE : MOVE_MOTION_SACLE);
            m_target_direction_vertical = target_direction_InCircle_vertical * (KeyAPressing ? RUN_MOTION_SCALE : MOVE_MOTION_SACLE);
        }
        else if (m_currentInputType == InputType.JoyStick)
        {
            m_target_direction_horizontal = horizontal * (KeyAPressing ? RUN_MOTION_SCALE : MOVE_MOTION_SACLE);
            m_target_direction_vertical = vertical * (KeyAPressing ? RUN_MOTION_SCALE : MOVE_MOTION_SACLE);
        }

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
