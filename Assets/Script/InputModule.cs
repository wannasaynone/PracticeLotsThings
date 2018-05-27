using UnityEngine;

public class InputModule : MonoBehaviour {

    public static InputModule Instance { get; private set; }

    private enum InputType
    {
        KeyBoard,
        JoyStick
    }

    // private static bool INPUT_ENABLE = true;
    private const float MOVE_MOTION_SACLE = 1f;
    private const float RUN_MOTION_SCALE = 2f;

    [SerializeField] private InputType m_currentInputType = InputType.KeyBoard;
    [SerializeField] private InputDetecter_Keyboard m_inputDetector_Keyboard = null;
    [SerializeField] private InputDetecter_JoyStick m_inputDetector_JoyStick = null;
    [Header("properties")]
    [SerializeField] private float m_moveSmoothTime = 0.1f;

    public float Direction_MotionCurveValue { get { return m_direction_motionCurveValue; } }
    public Vector3 Direction_Vector { get { return m_direction_vector; } }

    protected float m_direction_vertical = 0f;
    protected float m_direction_horizontal = 0f;

    protected float m_target_direction_vertical = 0f;
    protected float m_target_direction_horizontal = 0f;
    protected float m_velocity_direction_vertical = 0f;
    protected float m_velocity_direction_horizontal = 0f;
    protected float m_direction_motionCurveValue = 0f;
    protected float m_target_motionCurveValue = 0f;
    protected Vector3 m_direction_vector = Vector3.zero;

    public InputDetecter InputSingnal { get { return m_currentInputDetecter; } }
    private InputDetecter m_currentInputDetecter = null;

    private void Awake()
    {
        SetInstance();
        SetInputDetecter();
    }

    private void Update()
	{
        SetInputDetecter(); // For Testing
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

    private void SetInputDetecter()
    {
        if (m_currentInputType == InputType.JoyStick)
        {
            m_currentInputDetecter = m_inputDetector_JoyStick;
        }
        else if (m_currentInputType == InputType.KeyBoard)
        {
            m_currentInputDetecter = m_inputDetector_Keyboard;
        }
    }

    private void DetectInput()
    {
        m_currentInputDetecter.DetectInput();
        SetDirection(m_currentInputDetecter.LeftKey_Vertical, m_currentInputDetecter.LeftKey_Horizontal);
    }

    private void SetDirection(float vertical, float horizontal)
    {
        // "前後左右"會根據狀況改變，所以不做"位移"(Vector3.forward * speed...之類的)，先做玩家輸入的方向判斷
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        vertical = Mathf.Clamp(vertical, -1f, 1f);

        m_target_direction_horizontal = horizontal * (m_currentInputDetecter.KeyAPressing ? RUN_MOTION_SCALE : MOVE_MOTION_SACLE);
        m_target_direction_vertical = vertical * (m_currentInputDetecter.KeyAPressing ? RUN_MOTION_SCALE : MOVE_MOTION_SACLE);

        // 避免瞬間變值導致詭異的角色移動表現，用SmoothDamp製造類似遞增遞減的效果
        m_direction_horizontal = Mathf.SmoothDamp(m_direction_horizontal, m_target_direction_horizontal, ref m_velocity_direction_horizontal, m_moveSmoothTime);
        m_direction_vertical = Mathf.SmoothDamp(m_direction_vertical, m_target_direction_vertical, ref m_velocity_direction_vertical, m_moveSmoothTime);

        // 用現在的水平值跟垂直值取得目前動作動畫的變化變量
        m_direction_motionCurveValue = Mathf.Sqrt((m_direction_vertical * m_direction_vertical) + (m_direction_horizontal * m_direction_horizontal));

        // 給予目前正在移動的方向向量
        m_direction_vector = (m_direction_horizontal * Vector3.right + m_direction_vertical * Vector3.forward);
    }
}
