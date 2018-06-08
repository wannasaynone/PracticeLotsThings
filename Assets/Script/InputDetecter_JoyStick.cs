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
    [SerializeField] private string m_keyE = "R3";

    public override void DetectInput()
    {
        DetectInput_RightKey(m_rightKey_VerticalAxis, m_rightKey_HorizontalAxis);
        DetectInput_LeftKey(m_leftKey_VerticalAxis, m_leftKey_HorizontalAxis);
        KeyAPressed = Input.GetButtonDown(m_keyA);
        KeyBPressed = Input.GetButtonDown(m_keyB);
        KeyCPressed = Input.GetButtonDown(m_keyC);
        KeyDPressed = Input.GetButtonDown(m_keyD);
        KeyEPressed = Input.GetButtonDown(m_keyE);
        KeyAPressing = Input.GetButton(m_keyA);
        KeyBPressing = Input.GetButton(m_keyB);
        KeyCPressing = Input.GetButton(m_keyC);
        KeyDPressing = Input.GetButton(m_keyD);
        KeyEPressing = Input.GetButton(m_keyE);
        KeyAUp= Input.GetButtonUp(m_keyA);
        KeyBUp = Input.GetButtonUp(m_keyB);
        KeyCUp = Input.GetButtonUp(m_keyC);
        KeyDUp = Input.GetButtonUp(m_keyD);
        KeyEUp = Input.GetButtonUp(m_keyE);
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
