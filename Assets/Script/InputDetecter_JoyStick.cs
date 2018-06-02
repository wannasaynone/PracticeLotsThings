using UnityEngine;

[CreateAssetMenu(menuName = ("Controller/Joy Stick"))]
public class InputDetecter_JoyStick : InputDetecter {

    [SerializeField] private string m_leftKey_VerticalAxis = "DUp";
    [SerializeField] private string m_leftKey_HorizontalAxis = "DRight";
    [SerializeField] private string m_rightKey_VerticalAxis = "JUp";
    [SerializeField] private string m_rightKey_HorizontalAxis = "JRight";
    [SerializeField] private string m_keyA = "Square";
    [SerializeField] private string m_keyB = "Cross";
    [SerializeField] private string m_keyC = "Circle";
    [SerializeField] private string m_keyD = "Triangle";

    public override void DetectInput()
    {
        DetectInput_RightKey(m_rightKey_VerticalAxis, m_rightKey_HorizontalAxis);
        DetectInput_LeftKey(m_leftKey_VerticalAxis, m_leftKey_HorizontalAxis);
        m_keyAState = Input.GetButtonDown(m_keyA);
        m_keyBState = Input.GetButtonDown(m_keyB);
        m_keyCState = Input.GetButtonDown(m_keyC);
        m_keyDState = Input.GetButtonDown(m_keyD);
        KeyAPressing = Input.GetButton(m_keyA);
        KeyBPressing = Input.GetButton(m_keyB);
        KeyCPressing = Input.GetButton(m_keyC);
        KeyDPressing = Input.GetButton(m_keyD);
    }

    private void DetectInput_RightKey(string vertical, string horizontal)
    {
        m_rightKey_vertical = Input.GetAxis(vertical);
        m_rightKey_horizontal = Input.GetAxis(horizontal);
    }

    private void DetectInput_LeftKey(string vertical, string horizontal)
    {
        m_leftKey_vertical = Input.GetAxis(vertical);
        m_leftKey_horizontal = Input.GetAxis(horizontal);
    }
}
