using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDetecter_Keyboard : InputDetecter {

    [Header("Key Board")]
    [SerializeField] private string m_leftKey_Up = "w";
    [SerializeField] private string m_leftKey_Down = "s";
    [SerializeField] private string m_leftKey_Right = "d";
    [SerializeField] private string m_leftKey_Left = "a";
    [SerializeField] private string m_rightKey_Up = "up";
    [SerializeField] private string m_rightKey_Down = "down";
    [SerializeField] private string m_rightKey_Right = "right";
    [SerializeField] private string m_rightKey_Left = "left";
    [SerializeField] private string m_keyA = "left shift";
    [SerializeField] private string m_keyB = "space";
    [SerializeField] private string m_keyC = "j";
    [SerializeField] private string m_keyD = "l";

    public override void DetectInput()
    {
        DetectInput_RightKey(m_rightKey_Up, m_rightKey_Down, m_rightKey_Right, m_rightKey_Left);
        DetectInput_LeftKey(m_leftKey_Up, m_leftKey_Down, m_leftKey_Right, m_leftKey_Left, m_keyA, m_keyB);
        m_keyAState = Input.GetKeyDown(m_keyA);
        m_keyBState = Input.GetKeyDown(m_keyB);
        m_keyCState = Input.GetKeyDown(m_keyC);
        KeyAPressing = Input.GetKey(m_keyA);
        KeyBPressing = Input.GetKey(m_keyB);
        KeyCPressing = Input.GetKey(m_keyC);
    }

    private void DetectInput_RightKey(string up, string down, string right, string left)
    {
        m_rightKey_vertical = (Input.GetKey(up) ? 1f : 0f) - (Input.GetKey(down) ? 1f : 0f);
        m_rightKey_horizontal = (Input.GetKey(right) ? 1f : 0f) - (Input.GetKey(left) ? 1f : 0f);
    }

    private void DetectInput_LeftKey(string up, string down, string right, string left, string a, string b)
    {
        m_leftKey_vertical = (Input.GetKey(up) ? 1f : 0f) - (Input.GetKey(down) ? 1f : 0f);
        m_leftKey_horizontal = (Input.GetKey(right) ? 1f : 0f) - (Input.GetKey(left) ? 1f : 0f);
    }

}
